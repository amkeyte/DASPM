using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using DASPM.Table;
using DASPM_PCTEL.Table;

namespace DASPM_PCTEL.Updater
{
    public class PCTEL_UpdaterTable
    {
        public static PCTEL_UpdaterTable<PCTEL_UpdaterTableRowModel> Create(string name, string fullPath)
        {
            return new PCTEL_UpdaterTable<PCTEL_UpdaterTableRowModel>(name, fullPath);
        }
    }

    public class PCTEL_UpdaterTable<TModel> : PCTEL_Table<TModel>
         where TModel : PCTEL_UpdaterTableRowModel
    {
        public PCTEL_UpdaterTable(string name, string fullPath) : base(name, fullPath)
        {
        }

        #region CSVTable

        //Callback to define Row class
        public override ITableRow<TModel> CreateRow(TModel model)
        {
            return new PCTEL_UpdaterTableRow<TModel>(this, model);
        }

        //hiding return Row class type
        public new PCTEL_UpdaterTableRow<TModel> Row(int id)
        {
            return (PCTEL_UpdaterTableRow<TModel>)Rows[id];
        }

        protected override void ConfigureCsvReader(CsvReader csv)
        {
            var map = new PCTEL_UpdaterTableRowMap();
            csv.Configuration.RegisterClassMap(map);
        }

        protected override void ConfigureCsvWriter(CsvWriter csv)
        {
            var map = new PCTEL_UpdaterTableRowMap();
            csv.Configuration.RegisterClassMap(map);
        }

        #endregion CSVTable

        #region ClassMembers

        private PCTEL_UpdateWriter<TModel> _DataSetUpdater;
        public string DataSetPath { get; set; }

        public PCTEL_UpdateWriter<TModel> DataSetUpdater
        {
            get
            {
                if (_DataSetUpdater is null)
                {
                    if (DataSetPath == "") DataSetPath = Path;
                    _DataSetUpdater = new PCTEL_UpdateWriter<TModel>(this);
                }
                return _DataSetUpdater;
            }
        }

        #endregion ClassMembers
    }
}