using System;
using System.Collections.Generic;
using DASPM.Table;
using DASPM_PCTEL.Table;

namespace DASPM_PCTEL.DataSet
{
    public enum PCTEL_DataSetTypes
    {
        PCTEL_DST_REF = 100,
        PCTEL_DST_CP,
        PCTEL_DST_AREA
    }

    public class PCTEL_DataSet : PCTEL_Table, IHasDataSetType
    {
        //public const string PCTEL_DATASET_TYPE_NAME_AREA = "AreaTestPoints";
        //public const string PCTEL_DATASET_TYPE_NAME_CP = "CriticalTestPoints";
        //public const string PCTEL_DATASET_TYPE_NAME_REF = "*****";

        public static PCTEL_DataSet Create(string name, string fullPath)
        {
            var dataSetClassMapType = PCTEL_DataSetTypeHelper.GetClassMapType(fullPath);

            var result = (PCTEL_DataSet)CSVTableBuilder.CreateCSVTable(
                name, fullPath,
                typeof(PCTEL_DataSet),
                typeof(PCTEL_DataSetRow),
                typeof(PCTEL_DataSetRowModel),
                dataSetClassMapType);
            return result;
        }

        #region ctor

        public PCTEL_DataSet()
        {
        }

        //public void InitDataSet()
        //{
        //    if (DataSetType == 0)
        //    {
        //        throw new ArgumentException("Invalid DatasetType: is 'fullPath' a valid Area, CP, or Ref csv file?");
        //    }
        //    //else
        //    //{
        //    //    switch (DataSetType)
        //    //    {
        //    //        case PCTEL_DataSetTypes.PCTEL_DST_AREA:
        //    //            ClassMap
        //    //            break;
        //    //    }
        //    //}
        //}

        #endregion ctor

        #region CSVTable

        //////Callback to define RowMap type
        //public override void ConfigureCsvReader(CsvReader csv)
        //{
        //    var map = new PCTEL_DataSetRowMap(DataSetType);
        //    csv.Configuration.RegisterClassMap(map);
        //}

        ////callback to define RowMap type
        //public override void ConfigureCsvWriter(CsvWriter csv)
        //{
        //    var map = new PCTEL_DataSetRowMap(DataSetType);
        //    csv.Configuration.RegisterClassMap(map);
        //}
        public new PCTEL_DataSetRowMap ClassMap { get => (PCTEL_DataSetRowMap)base.ClassMap; }

        #region Accessors

        public new IList<PCTEL_DataSetRow> Rows => base.GetRows<PCTEL_DataSetRow>();

        public new PCTEL_DataSetRowModel this[int index] => Rows[index].Fields;

        public new IList<PCTEL_DataSetRow> this[PCTEL_Location loc] => GetRowsByLocation<PCTEL_DataSetRow>(loc);

        public new PCTEL_DataSetRow Row(int id) => (PCTEL_DataSetRow)base.Row(id);

        #endregion Accessors

        public new PCTEL_DataSetRow AddRow(IRowModel model)
        {
            var dstModel = model as PCTEL_DataSetRowModel;
            if (dstModel is null)
            {
                throw new InvalidOperationException("model must support IHasDataSetType interface");
            }
            else if (dstModel.DataSetType != 0 && dstModel.DataSetType != DataSetType)
            {
                throw new ArgumentException("model DataSetType mismatch");
            }
            else
            {
                dstModel.DataSetType = DataSetType;
            }

            return (PCTEL_DataSetRow)base.AddRow(model);
        }

        public new PCTEL_DataSetRow AddRow() => (PCTEL_DataSetRow)base.AddRow();

        public override void LoadFromFile()
        {
            base.LoadFromFile();
            foreach (var row in Rows)
            {
                row.Fields.DataSetType = ClassMap.DataSetType;
            }
        }

        #endregion CSVTable

        #region ClassMembers

        public PCTEL_DataSetTypes DataSetType
        {
            get
            {
                return ClassMap.DataSetType;
            }
        }

        #endregion ClassMembers
    }
}

//    public class PCTEL_DataSet<TModel> : PCTEL_Table<TModel>
//        where TModel : PCTEL_DataSetRowModel
//    {
//        #region ctor

//        private PCTEL_DataSetCore Core { get; set; }

//        public PCTEL_DataSet()
//        {
//            //assumes CSVTable construction is complete... true?
//            Core = new PCTEL_DataSetCore((PCTEL_DataSet)(ITable)this);

//            if (DataSetType == 0)
//            {
//                throw new ArgumentException("'fullPath' must be valid Area, CP, or Ref csv file.");
//            }
//        }

//        public static new PCTEL_DataSet<TModel> CreateGeneric(string name, string fullPath)
//        {
//            return (PCTEL_DataSet<TModel>)CreateGeneric(name, fullPath,
//                typeof(PCTEL_DataSet<TModel>),
//                typeof(PCTEL_DataSetRow<TModel>));
//        }

//        #endregion ctor

//        #region CSVTable

//        ////callback to define Row class
//        //public override ITableRow CreateRow(TModel model)
//        //{
//        //    model.DataSetType = DataSetType;
//        //    return new PCTEL_DataSetRow<TModel>(this, model);
//        //}

//        //Callback to define RowMap type
//        protected override void ConfigureCsvReader(CsvReader csv)
//        {
//            Core.ConfigureCsvReader(csv);
//        }

//        //callback to define RowMap type
//        protected override void ConfigureCsvWriter(CsvWriter csv)
//        {
//            Core.ConfigureCsvWriter(csv);
//        }

//        //hiding return Row class type
//        public new PCTEL_DataSetRow<TModel> Row(int id)
//        {
//            return (PCTEL_DataSetRow<TModel>)Rows[id];
//        }

//        #endregion CSVTable

//        #region ClassMembers

//        public PCTEL_DataSetTypes DataSetType => Core.DataSetType;

//        #endregion ClassMembers
//    }
//}