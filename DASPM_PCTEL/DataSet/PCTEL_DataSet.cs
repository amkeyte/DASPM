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

namespace DASPM_PCTEL.DataSet
{
    public enum PCTEL_DataSetTypes
    {
        PCTEL_DST_REF = 100,
        PCTEL_DST_CP,
        PCTEL_DST_AREA
    }

    public class PCTEL_DataSet
    {
        public const string PCTEL_DATASET_TYPE_NAME_AREA = "AreaTestPoints";
        public const string PCTEL_DATASET_TYPE_NAME_CP = "CriticalTestPoints";
        public const string PCTEL_DATASET_TYPE_NAME_REF = "*****";

        public static PCTEL_DataSet<PCTEL_DataSetRowModel> Create(string name, string path, string filename)
        {
            return new PCTEL_DataSet<PCTEL_DataSetRowModel>(name, path, filename);
        }
    }

    public class PCTEL_DataSet<T> : CSVTable<T>
        where T : PCTEL_DataSetRowModel
    {
        #region ctor

        public PCTEL_DataSet(string name, string path, string filename) : base(name, path, filename)
        {
            if (filename.Contains(PCTEL_DataSet.PCTEL_DATASET_TYPE_NAME_AREA))
            {
                DataSetType = PCTEL_DataSetTypes.PCTEL_DST_AREA;
            }
            else if (filename.Contains(PCTEL_DataSet.PCTEL_DATASET_TYPE_NAME_CP))
            {
                DataSetType = PCTEL_DataSetTypes.PCTEL_DST_CP;
            }
            else
            {
                throw new ArgumentException("'filename' must be valid Area, CP, or Ref csv file.");
            }
        }

        #endregion ctor

        #region CSVTable

        //callback to define Row class
        public override ITableRow<T> CreateRow(T model)
        {
            model.DataSetType = DataSetType;
            return new PCTEL_DataSetRow<T>(this, model);
        }

        //hiding return Row class type
        public new PCTEL_DataSetRow<T> Row(int id)
        {
            return (PCTEL_DataSetRow<T>)Rows[id];
        }

        //Callback to define RowMap type
        protected override void ConfigureCsvReader(CsvReader csv)
        {
            var map = new PCTEL_DataSetRowMap(DataSetType);
            csv.Configuration.RegisterClassMap(map);
        }

        //callback to define RowMap type
        protected override void ConfigureCsvWriter(CsvWriter csv)
        {
            var map = new PCTEL_DataSetRowMap(DataSetType);
            csv.Configuration.RegisterClassMap(map);
        }

        #endregion CSVTable

        #region ClassMembers

        public PCTEL_DataSetTypes DataSetType { get; private set; }

        #endregion ClassMembers
    }
}