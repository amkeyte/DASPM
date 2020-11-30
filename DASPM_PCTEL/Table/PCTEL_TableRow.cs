using DASPM.Table;

namespace DASPM_PCTEL.Table
{
    internal class PCTEL_TableRowCore
    {
        protected PCTEL_TableRow TableRow { get; set; }

        public PCTEL_TableRowCore(PCTEL_TableRow tableRow)
        {
            TableRow = tableRow;
        }

        #region ClassMembers

        protected PCTEL_Location _location;

        public PCTEL_Location Location
        {
            get
            {
                if (_location is null)
                {
                    _location = new PCTEL_Location(TableRow.Fields);
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

    public class PCTEL_TableRow : CSVTableRow
    {
        #region ctor

        private PCTEL_TableRowCore Core { get; set; }

        //public static implicit operator PCTEL_TableRow(PCTEL_TableRow<PCTEL_TableRowModel> other)
        //{
        //    //double cast allows cross-generic conversion??
        //    return (PCTEL_TableRow)(ITableRow)other;
        //}
        public PCTEL_TableRow()
        {
            Core = new PCTEL_TableRowCore(this);
        }

        public static PCTEL_TableRow Create(PCTEL_Table table, PCTEL_TableRowModel model)
        {
            return (PCTEL_TableRow)Create(table, model, typeof(PCTEL_TableRow));
        }

        #endregion ctor

        #region CSVTableRow

        //Convenience casting
        public new PCTEL_TableRowModel Fields
        {
            get
            {
                return (PCTEL_TableRowModel)base.Fields;
            }
        }

        #endregion CSVTableRow

        #region ClassMembers

        public PCTEL_Location Location
        {
            get
            {
                return Core.Location;
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