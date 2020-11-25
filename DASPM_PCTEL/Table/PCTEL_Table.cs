using System.Collections.Generic;
using CsvHelper;
using DASPM.Table;
using DASPM_PCTEL.ControlPanel;

namespace DASPM_PCTEL.Table
{
    internal class PCTEL_Table_Core
    {
        protected CSVTable BaseTable => Table;
        protected PCTEL_Table Table { get; set; }

        public PCTEL_Table_Core(PCTEL_Table table)
        {
            Table = table;
            Locations = new SortedList<PCTEL_Location, PCTEL_TableRow>();
        }

        #region CSVTable

        /// <summary>
        /// Do the specific things needed to add a row to PCTEL_Table
        /// </summary>
        /// <param name="row"></param>
        public PCTEL_TableRow AddRow(IRowModel model)
        {
            PCTEL_TableRow row = (PCTEL_TableRow)BaseTable.AddRow(model);

            if (!Locations.ContainsKey(row.Location))
            {
                Locations.Add(row.Location, row);
            }

            return row;
        }

        public void ConfigureCsvReader(CsvReader csv)
        {
            var map = new PCTEL_TableRowMap();
            csv.Configuration.RegisterClassMap(map);
        }

        public void ConfigureCsvWriter(CsvWriter csv)
        {
            var map = new PCTEL_TableRowMap();
            csv.Configuration.RegisterClassMap(map);
        }

        public PCTEL_TableRowModel GetModelByID(int id) => GetRowByID(id).Fields;

        public PCTEL_TableRowModel GetModelByLocation(PCTEL_Location loc) => GetRowByLoc(loc).Fields;

        public PCTEL_TableRow GetRowByID(int id) => (PCTEL_TableRow)BaseTable.Row(id);

        public PCTEL_TableRow GetRowByLoc(PCTEL_Location loc) => Locations[loc];

        #endregion CSVTable

        #region ClassMembers

        //this might just be legacy now...
        public PCTEL_ControlPanel ControlPanel { get; }

        public SortedList<PCTEL_Location, PCTEL_TableRow> Locations { get; protected set; }

        public void Calculate()
        {
            foreach (var l in Locations.Values)
            {
                l.Calculate();
            }
        }

        #endregion ClassMembers
    }

    public class PCTEL_Table : CSVTable
    {
        #region ctor

        private PCTEL_Table_Core Core { get; set; }

        public PCTEL_Table()
        {
            Core = new PCTEL_Table_Core(this);
        }

        public static PCTEL_Table Create(string name, string fullPath)
        {
            return (PCTEL_Table)Create(name, fullPath, typeof(PCTEL_Table), typeof(PCTEL_TableRow), typeof(PCTEL_TableRowModel));
        }

        //definately test this!!!
        public static implicit operator PCTEL_Table(PCTEL_Table<PCTEL_TableRowModel> other)
            => (PCTEL_Table)(ITable)other;

        #endregion ctor

        #region CSVTable

        protected override void ConfigureCsvReader(CsvReader csv)
        {
            Core.ConfigureCsvReader(csv);
        }

        protected override void ConfigureCsvWriter(CsvWriter csv)
        {
            Core.ConfigureCsvWriter(csv);
        }

        //short form access for location -
        //can still use int index?
        public new PCTEL_TableRowModel this[int id] => Core.GetModelByID(id);

        public virtual PCTEL_TableRowModel this[PCTEL_Location loc] => Core.GetModelByLocation(loc);

        public new PCTEL_TableRow AddRow(IRowModel model) => Core.AddRow(model);

        //hiding return Row class type
        public new PCTEL_TableRow Row(int id) => Core.GetRowByID(id);

        #endregion CSVTable
    }

    public class PCTEL_Table<TModel> : PCTEL_Table, ITable<TModel>
        where TModel : PCTEL_TableRowModel
    {
        #region ctor

        private PCTEL_Table_Core Core { get; set; }

        public PCTEL_Table()
        {
            //double cast to get past generic TModel can't be used by implicit cast
            Core = new PCTEL_Table_Core((PCTEL_Table)(CSVTable)this);
        }

        public static PCTEL_Table<TModel> CreateGeneric(string name, string fullPath)
        {
            return (PCTEL_Table<TModel>)CSVTable<TModel>Create(name, fullPath,
                typeof(PCTEL_Table<TModel>),
                typeof(PCTEL_TableRow<TModel>));
        }

        #endregion ctor

        #region CSVTable

        protected override void ConfigureCsvReader(CsvReader csv)
        {
            Core.ConfigureCsvReader(csv);
        }

        protected override void ConfigureCsvWriter(CsvWriter csv)
        {
            Core.ConfigureCsvWriter(csv);
        }

        //short form access for location -
        //can still use int index?
        public new PCTEL_TableRowModel this[int id] => Core.GetModelByID(id);

        public virtual PCTEL_TableRowModel this[PCTEL_Location loc] => Core.GetModelByLocation(loc);

        public new PCTEL_TableRow<TModel> AddRow(IRowModel model) => Core.AddRow(model);

        //hiding return Row class type
        public new PCTEL_TableRow<TModel> Row(int id) => Core.GetRowByID(id);

        #endregion CSVTable
    }
}