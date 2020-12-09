using System;
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

    public class PCTEL_DataSet : PCTEL_Table
    {
        public const string PCTEL_DATASET_TYPE_NAME_AREA = "AreaTestPoints";
        public const string PCTEL_DATASET_TYPE_NAME_CP = "CriticalTestPoints";
        public const string PCTEL_DATASET_TYPE_NAME_REF = "*****";

        public static PCTEL_DataSet Create(string name, string fullPath)
        {
            var result = (PCTEL_DataSet)CSVTableBuilder.CreateCSVTable(
                name, fullPath,
                typeof(PCTEL_DataSet),
                typeof(PCTEL_DataSetRow),
                typeof(PCTEL_DataSetRowModel),
                typeof(PCTEL_DataSetRowMap));
            result.InitDataSet();
            return result;
        }

        #region ctor

        public PCTEL_DataSet()
        {
        }

        public void InitDataSet()
        {
            if (DataSetType == 0)
            {
                throw new ArgumentException("Invalid DatasetType: is 'fullPath' a valid Area, CP, or Ref csv file?");
            }
        }

        #endregion ctor

        #region CSVTable

        ////Callback to define RowMap type
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

        //hiding return Row class type
        public new PCTEL_DataSetRow Row(int id)
        {
            return (PCTEL_DataSetRow)base.Row(id);
        }

        #endregion CSVTable

        #region ClassMembers

        public PCTEL_DataSetTypes DataSetType
        {
            get
            {
                if (FullPath.Contains(PCTEL_DATASET_TYPE_NAME_AREA))
                {
                    return PCTEL_DataSetTypes.PCTEL_DST_AREA;
                }
                else if (FullPath.Contains(PCTEL_DATASET_TYPE_NAME_CP))
                {
                    return PCTEL_DataSetTypes.PCTEL_DST_CP;
                }
                else
                {
                    return 0;
                }
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