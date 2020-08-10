using System.Collections.Generic;
using CsvHelper;
using DASPM.Table;
using DASPM_PCTEL.ControlPanel;

namespace DASPM_PCTEL.Table
{
    public class PCTEL_Table
    {
        public static PCTEL_Table<PCTEL_TableRowModel> Create(string name, string path, string filename)
        {
            return new PCTEL_Table<PCTEL_TableRowModel>(name, path, filename);
        }
    }

    public class PCTEL_Table<T> : CSVTable<T>
        where T : PCTEL_TableRowModel
    {
        #region ctor

        public PCTEL_Table(string name, string path, string filename) : base(name, path, filename)
        {
            Locations = new SortedList<PCTEL_Location, PCTEL_TableRow<T>>();
        }

        #endregion ctor

        #region CSVTable

        public override void AddRow(T model)
        {
            var row = (PCTEL_TableRow<T>)CreateRow(model);
            Rows.Add(row);

            if (!Locations.ContainsKey(row.Location))
            {
                Locations.Add(row.Location, row);
            }
        }

        //Callback to define Row class
        public override ITableRow<T> CreateRow(T model)
        {
            return new PCTEL_TableRow<T>(this, model);
        }

        //hiding return Row class type
        public new PCTEL_TableRow<T> Row(int id)
        {
            return (PCTEL_TableRow<T>)Rows[id];
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
        public virtual T this[PCTEL_Location loc]
        {
            get => Locations[loc].Fields;
        }

        #endregion CSVTable

        #region ClassMembers

        public PCTEL_ControlPanel ControlPanel { get; }
        public SortedList<PCTEL_Location, PCTEL_TableRow<T>> Locations { get; protected set; }

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