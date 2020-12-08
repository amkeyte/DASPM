using System;
using System.Collections.Generic;
using CsvHelper;
using DASPM.Table;
using DASPM_PCTEL.ControlPanel;

namespace DASPM_PCTEL.Table
{
    public abstract class PCTEL_Table : CSVTable, IHasLocations, IDoesCalculation
    {
        #region ctor

        public PCTEL_Table()
        {
        }

        #endregion ctor

        #region CSVTable

        public new IList<PCTEL_TableRow> Rows => GetRows<PCTEL_TableRow>();

        public new PCTEL_TableRow AddRow(IRowModel model) => (PCTEL_TableRow)base.AddRow(model);

        public new PCTEL_TableRow AddRow() => (PCTEL_TableRow)base.AddRow();

        public override void ConfigureCsvReader(CsvReader csv)
        {
            //var map = new PCTEL_TableRowMap();
            csv.Configuration.RegisterClassMap(ClassMap);
        }

        public override void ConfigureCsvWriter(CsvWriter csv)
        {
            //var map = new PCTEL_TableRowMap();
            csv.Configuration.RegisterClassMap(ClassMap);//this could probably just be moved to table builder and deleted
        }

        public new void LoadFromFile()
        {
            base.LoadFromFile();
            RefreshLocations();
        }

        #region LongFormAccess

        public PCTEL_TableRowModel GetModelByID(int id) => GetRowByID(id).Fields;

        public PCTEL_TableRow GetRowByID(int id) => (PCTEL_TableRow)base.Row(id);

        public IList<TTableRow> GetRowsByLocation<TTableRow>(PCTEL_Location loc)
            where TTableRow : IHasLocation
        {
            var result = new List<TTableRow>();
            foreach (IHasLocation row in Rows)
            {
                if (row.Location == loc)
                {
                    result.Add((TTableRow)row);
                }
            }

            return result;
        }

        public IList<PCTEL_TableRow> GetRowsByLocation(PCTEL_Location loc)
        {
            return GetRowsByLocation<PCTEL_TableRow>(loc);
        }

        #endregion LongFormAccess

        #region ShortFormAccess

        //hiding return Row class type
        public new PCTEL_TableRowModel this[int id] => GetModelByID(id);

        /// <summary>
        /// Return a list of all rows containing the requested location.
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public virtual IList<PCTEL_TableRow> this[PCTEL_Location loc] => GetRowsByLocation(loc);

        //hiding return Row class type
        public new PCTEL_TableRow Row(int id) => GetRowByID(id);

        #endregion ShortFormAccess

        #endregion CSVTable

        #region ClassMembers

        //this might just be legacy now...
        public PCTEL_ControlPanel ControlPanel { get; }

        public void Calculate()
        {
            foreach (var row in Rows)
            {
                row.Calculate();
            }
        }

        #region Locations

        private List<PCTEL_Location> _locations = new List<PCTEL_Location>();

        IList<IHasLocation> IHasLocations.Locations => (IList<IHasLocation>)_locations;
        public IList<PCTEL_Location> Locations => _locations;

        /// <summary>
        /// Add a location and, optionally, a row to the table.
        /// </summary>
        /// <param name="obj">The object containing the location. If the table does not contain a row with the location already
        /// </param>
        public void AddLocation(PCTEL_Location loc)
        {
            if (_locations.Contains(loc)) return;

            //if the location does not exist in the current table, add a default row
            if (GetRowsByLocation(loc).Count == 0)
            {
                var model = (PCTEL_TableRowModel)Activator.CreateInstance(ModelType); // maybe add GedDefaultModel?
                PCTEL_Location.ApplyLocation(model, loc);

                AddRow(model);
            }

            Locations.Add(loc);
        }

        /// <summary>
        /// Sync the TableRows and Locations lists
        /// </summary>
        public void RefreshLocations()
        {
            _locations.Clear();
            foreach (var row in Rows)
            {
                AddLocation(row.Location);
            }
        }

        /// <summary>
        /// Remove any TableRows based on the location given.
        /// </summary>
        /// <param name="loc">The location object designating which TableRows to remove</param>
        public void RemoveLocation(PCTEL_Location loc)
        {
            foreach (var row in GetRowsByLocation(loc))
            {
                RemoveRow(row.ID);
            }
        }

        #endregion Locations

        #endregion ClassMembers
    }

    //public class PCTEL_Table<TModel> : PCTEL_Table, ITable<TModel>
    //    where TModel : PCTEL_TableRowModel
    //{
    //    #region ctor

    //    private PCTEL_Table_Core Core { get; set; }

    //    public PCTEL_Table()
    //    {
    //        //double cast to get past generic TModel can't be used by implicit cast
    //        Core = new PCTEL_Table_Core((PCTEL_Table)(CSVTable)this);
    //    }

    //    public static PCTEL_Table<TModel> CreateGeneric(string name, string fullPath)
    //    {
    //        return (PCTEL_Table<TModel>)CSVTable<TModel>Create(name, fullPath,
    //            typeof(PCTEL_Table<TModel>),
    //            typeof(PCTEL_TableRow<TModel>));
    //    }

    //    #endregion ctor

    //    #region CSVTable

    //    protected override void ConfigureCsvReader(CsvReader csv)
    //    {
    //        Core.ConfigureCsvReader(csv);
    //    }

    //    protected override void ConfigureCsvWriter(CsvWriter csv)
    //    {
    //        Core.ConfigureCsvWriter(csv);
    //    }

    //    //short form access for location -
    //    //can still use int index?
    //    public new PCTEL_TableRowModel this[int id] => Core.GetModelByID(id);

    //    public virtual PCTEL_TableRowModel this[PCTEL_Location loc] => Core.GetModelByLocation(loc);

    //    public new PCTEL_TableRow<TModel> AddRow(IRowModel model) => Core.AddRow(model);

    //    //hiding return Row class type
    //    public new PCTEL_TableRow<TModel> Row(int id) => Core.GetRowByID(id);

    //    #endregion CSVTable
    //}
}