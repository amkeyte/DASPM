using System.Collections.Generic;
using CsvHelper;
using DASPM.Table;
using DASPM_PCTEL.ControlPanel;

namespace DASPM_PCTEL.Table
{
    internal class PCTEL_Table_Core
    {
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
        public void AddRow(PCTEL_TableRow row)
        {
            if (!Locations.ContainsKey(row.Location))
            {
                Locations.Add(row.Location, row);
            }
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

        public static PCTEL_Table Create(string name, string fullPath)
        {
            return (PCTEL_Table)Create(name, fullPath, typeof(PCTEL_Table), typeof(PCTEL_TableRow), typeof(PCTEL_TableRowModel));
        }

        private PCTEL_Table_Core Core { get; set; }

        public PCTEL_Table()
        {
            Core = new PCTEL_Table_Core(this);
        }

        public static implicit operator PCTEL_Table(PCTEL_Table<PCTEL_TableRowModel> other)
        {
            //definately test this!!!
            return (PCTEL_Table)(ITable)other;
        }

        #endregion ctor

        public override void AddRow(IRowModel model)
        {
            base.AddRow(model);
            Core.AddRow((PCTEL_TableRow)Rows[Rows.Count - 1]);
        }

        //hiding return Row class type
        public new PCTEL_TableRow Row(int id)
        {
            return (PCTEL_TableRow)Rows[id];
        }

        protected override void ConfigureCsvReader(CsvReader csv)
        {
            Core.ConfigureCsvReader(csv);
        }

        protected override void ConfigureCsvWriter(CsvWriter csv)
        {
            Core.ConfigureCsvWriter(csv);
        }

        //short form access for location - can still use int index?
        public virtual PCTEL_TableRowModel this[PCTEL_Location loc]
        {
            get => (PCTEL_TableRowModel)Core.Locations[loc].Fields;
        }
    }

    public class PCTEL_Table<TModel> : CSVTable<TModel>
        where TModel : PCTEL_TableRowModel
    {
        #region ctor

        public static PCTEL_Table<TModel> CreateGeneric(string name, string fullPath)
        {
            return (PCTEL_Table<TModel>)CreateGeneric(name, fullPath,
                typeof(PCTEL_Table<TModel>),
                typeof(PCTEL_TableRow<TModel>));
        }

        private PCTEL_Table_Core Core { get; set; }

        public PCTEL_Table()
        {
            //double cast to get past generic TModel can't be used by implicit cast
            Core = new PCTEL_Table_Core((PCTEL_Table)(ITable)this);
        }

        #endregion ctor

        public override void AddRow(IRowModel model)
        {
            base.AddRow(model);
            Core.AddRow((PCTEL_TableRow)Rows[Rows.Count - 1]);
        }

        //hiding return Row class type
        public new PCTEL_TableRow Row(int id)
        {
            return (PCTEL_TableRow)Rows[id];
        }

        protected override void ConfigureCsvReader(CsvReader csv)
        {
            Core.ConfigureCsvReader(csv);
        }

        protected override void ConfigureCsvWriter(CsvWriter csv)
        {
            Core.ConfigureCsvWriter(csv);
        }

        //short form access for location - can still use int index?
        public virtual PCTEL_TableRowModel this[PCTEL_Location loc]
        {
            get => (PCTEL_TableRowModel)Core.Locations[loc].Fields;
        }
    }
}