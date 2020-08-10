using DASPM.Table;

namespace DASPM_PCTEL.Table
{
    public class PCTEL_TableRow<T> : CSVTableRow<T>
        where T : PCTEL_TableRowModel
    {
        public PCTEL_TableRow(CSVTable<T> table, T row) : base(table, row)
        {
        }

        #region ClassMembers

        public PCTEL_Location Location
        {
            get
            {
                return new PCTEL_Location(Fields);
            }
        }

        public virtual void Calculate()
        {
            //no default calculation
        }

        #endregion ClassMembers
    }
}