using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DASPM.Table;
using System.IO;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using DASPM_PCTEL.Table;

namespace DASPM_PCTEL.DataSet
{
    public enum PCTEL_DataSetTypes
    {
        PCTEL_DST_REF = 100,
        PCTEL_DST_CP,
        PCTEL_DST_AREA
    }

    internal class PCTEL_DataSetCore
    {
        protected PCTEL_DataSet Table { get; set; }

        public PCTEL_DataSetCore(PCTEL_DataSet table)
        {
            Table = table;
        }

        public PCTEL_DataSetTypes DataSetType
        {
            get
            {
                if (Table.FullPath.Contains(PCTEL_DataSet.PCTEL_DATASET_TYPE_NAME_AREA))
                {
                    return PCTEL_DataSetTypes.PCTEL_DST_AREA;
                }
                else if (Table.FullPath.Contains(PCTEL_DataSet.PCTEL_DATASET_TYPE_NAME_CP))
                {
                    return PCTEL_DataSetTypes.PCTEL_DST_CP;
                }
                else
                {
                    return 0;
                }
            }
        }

        //Callback to define RowMap type
        public void ConfigureCsvReader(CsvReader csv)
        {
            var map = new PCTEL_DataSetRowMap(DataSetType);
            csv.Configuration.RegisterClassMap(map);
        }

        //callback to define RowMap type
        public void ConfigureCsvWriter(CsvWriter csv)
        {
            var map = new PCTEL_DataSetRowMap(DataSetType);
            csv.Configuration.RegisterClassMap(map);
        }
    }

    public class PCTEL_DataSet : PCTEL_Table
    {
        public const string PCTEL_DATASET_TYPE_NAME_AREA = "AreaTestPoints";
        public const string PCTEL_DATASET_TYPE_NAME_CP = "CriticalTestPoints";
        public const string PCTEL_DATASET_TYPE_NAME_REF = "*****";

        public static new PCTEL_DataSet<PCTEL_DataSetRowModel> Create(string name, string fullPath)
        {
            return (PCTEL_DataSet<PCTEL_DataSetRowModel>)Create(name, fullPath,
                typeof(PCTEL_DataSet),
                typeof(PCTEL_DataSetRow),
                typeof(PCTEL_DataSetRowModel));
        }

        #region ctor

        public PCTEL_DataSet()
        {
            //assumes CSVTable construction is complete... true?
            Core = new PCTEL_DataSetCore(this);

            if (DataSetType == 0)
            {
                throw new ArgumentException("'fullPath' must be valid Area, CP, or Ref csv file.");
            }
        }

        public static implicit operator PCTEL_DataSet(PCTEL_DataSet<PCTEL_DataSetRowModel> other)
        {
            //definately test this!!!
            return (PCTEL_DataSet)(ITable)other;
        }

        #endregion ctor

        #region CSVTable

        ////callback to define Row class
        //public override ITableRow CreateRow(TModel model)
        //{
        //    model.DataSetType = DataSetType;
        //    return new PCTEL_DataSetRow<TModel>(this, model);
        //}

        //hiding return Row class type
        public new PCTEL_DataSetRow Row(int id)
        {
            return (PCTEL_DataSetRow)Rows[id];
        }

        //Callback to define RowMap type
        protected override void ConfigureCsvReader(CsvReader csv)
        {
            Core.ConfigureCsvReader(csv);
        }

        //callback to define RowMap type
        protected override void ConfigureCsvWriter(CsvWriter csv)
        {
            Core.ConfigureCsvWriter(csv);
        }

        #endregion CSVTable

        #region ClassMembers

        private PCTEL_DataSetCore Core { get; set; }
        public PCTEL_DataSetTypes DataSetType => Core.DataSetType;

        #endregion ClassMembers
    }

    public class PCTEL_DataSet<TModel> : PCTEL_Table<TModel>
        where TModel : PCTEL_DataSetRowModel
    {
        #region ctor

        public static new PCTEL_DataSet<TModel> CreateGeneric(string name, string fullPath)
        {
            return (PCTEL_DataSet<TModel>)CreateGeneric(name, fullPath,
                typeof(PCTEL_DataSet<TModel>),
                typeof(PCTEL_DataSetRow<TModel>));
        }

        public PCTEL_DataSet()
        {
            //assumes CSVTable construction is complete... true?
            Core = new PCTEL_DataSetCore((PCTEL_DataSet)(ITable)this);

            if (DataSetType == 0)
            {
                throw new ArgumentException("'fullPath' must be valid Area, CP, or Ref csv file.");
            }
        }

        private PCTEL_DataSetCore Core { get; set; }

        #endregion ctor

        #region CSVTable

        ////callback to define Row class
        //public override ITableRow CreateRow(TModel model)
        //{
        //    model.DataSetType = DataSetType;
        //    return new PCTEL_DataSetRow<TModel>(this, model);
        //}

        //hiding return Row class type
        public new PCTEL_DataSetRow<TModel> Row(int id)
        {
            return (PCTEL_DataSetRow<TModel>)Rows[id];
        }

        //Callback to define RowMap type
        protected override void ConfigureCsvReader(CsvReader csv)
        {
            Core.ConfigureCsvReader(csv);
        }

        //callback to define RowMap type
        protected override void ConfigureCsvWriter(CsvWriter csv)
        {
            Core.ConfigureCsvWriter(csv);
        }

        #endregion CSVTable

        #region ClassMembers

        public PCTEL_DataSetTypes DataSetType => Core.DataSetType;

        #endregion ClassMembers
    }
}