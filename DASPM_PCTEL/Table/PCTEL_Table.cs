using System.Collections.Generic;
using CsvHelper;
using DASPM.Table;
using DASPM_PCTEL.ControlPanel;

namespace DASPM_PCTEL.Table
{
    public class PCTEL_Table
    {
        public static PCTEL_Table<PCTEL_TableRowModel> Create(string name, string fullPath)
        {
            return new PCTEL_Table<PCTEL_TableRowModel>(name, fullPath);
        }
    }

    public class PCTEL_Table<TModel> : CSVTable<TModel>
        where TModel : PCTEL_TableRowModel
    {
        #region ctor

        public PCTEL_Table(string name, string fullPath) : base(name, fullPath)
        {
            Locations = new SortedList<PCTEL_Location, PCTEL_TableRow<TModel>>();
        }

        #endregion ctor

        #region CSVTable

        public override void AddRow(TModel model)
        {
            var row = (PCTEL_TableRow<TModel>)CreateRow(model);
            Rows.Add(row);

            if (!Locations.ContainsKey(row.Location))
            {
                Locations.Add(row.Location, row);
            }
        }

        //Callback to define Row class
        public override ITableRow<TModel> CreateRow(TModel model)
        {
            return new PCTEL_TableRow<TModel>(this, model);
        }

        //hiding return Row class type
        public new PCTEL_TableRow<TModel> Row(int id)
        {
            return (PCTEL_TableRow<TModel>)Rows[id];
        }

        protected override void ConfigureCsvReader(CsvReader csv)
        {
            var map = new PCTEL_TableRowMap();
            csv.Configuration.RegisterClassMap(map);
        }

        protected override void ConfigureCsvWriter(CsvWriter csv)
        {
            var map = new PCTEL_TableRowMap();
            csv.Configuration.RegisterClassMap(map);
        }

        //short form access
        public virtual TModel this[PCTEL_Location loc]
        {
            get => Locations[loc].Fields;
        }

        #endregion CSVTable

        #region ClassMembers

        public PCTEL_ControlPanel ControlPanel { get; }
        public SortedList<PCTEL_Location, PCTEL_TableRow<TModel>> Locations { get; protected set; }

        public void Calculate()
        {
            foreach (var l in Locations.Values)
            {
                l.Calculate();
            }
        }

        #endregion ClassMembers
    }
}