using DASPM.Table;

namespace DASPM_PCTEL.Table
{
    public abstract class PCTEL_TableRow : CSVTableRow, IHasLocation, IDoesCalculation
    {
        #region ctor

        public PCTEL_TableRow()
        {
        }

        public static PCTEL_TableRow Create(PCTEL_Table table, PCTEL_TableRowModel model)
        {
            return (PCTEL_TableRow)CSVTableRowBuilder.Create(table, model, typeof(PCTEL_TableRow));
        }

        #endregion ctor

        #region CSVTableRow

        //Convenience casting
        public new PCTEL_TableRowModel Fields => (PCTEL_TableRowModel)base.Fields;

        #endregion CSVTableRow

        #region ClassMembers

        private PCTEL_Location _location;

        public PCTEL_Location Location
        {
            get
            {
                if (_location is null)
                {
                    _location = new PCTEL_Location(Fields);
                }
                return _location;
            }
        }

        public virtual void Calculate()
        {
            //no default calculation
        }

        #endregion ClassMembers
    }

    //public class PCTEL_TableRow<TModel> : CSVTableRow<TModel>
    //    where TModel : PCTEL_TableRowModel
    //{
    //    #region ctor

    //    public static PCTEL_TableRow<TModel> CreateGeneric(PCTEL_Table<TModel> table, TModel model)
    //    {
    //        return (PCTEL_TableRow<TModel>)CreateGeneric(table, model, typeof(PCTEL_TableRow<TModel>));
    //    }

    //    private PCTEL_TableRowCore Core { get; set; }

    //    public PCTEL_TableRow()
    //    {
    //        //double cast because TModel cant cast to PCTEL_TableRowModel
    //        Core = new PCTEL_TableRowCore((PCTEL_TableRow)(ITableRow)this);
    //    }

    //    public static implicit operator PCTEL_TableRow<TModel>(PCTEL_TableRow other)
    //    {
    //        //double cast allows cross-generic conversion??
    //        return (PCTEL_TableRow<TModel>)(ITableRow)other;
    //    }

    //    #endregion ctor

    //    #region CSVTableRow

    //    //Convenience casting
    //    public new PCTEL_TableRowModel Fields
    //    {
    //        get
    //        {
    //            return base.Fields;
    //        }
    //    }

    //    #endregion CSVTableRow

    //    #region ClassMembers

    //    public PCTEL_Location Location
    //    {
    //        get
    //        {
    //            return Core.Location;
    //        }
    //    }

    //    public virtual void Calculate()
    //    {
    //        //no default calculation
    //    }

    //    #endregion ClassMembers
    //}
}