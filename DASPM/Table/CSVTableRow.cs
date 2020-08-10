using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    public class CSVTableRow<T> : ITableRow<T> where T : IRowModel
    {
        #region ctor

        public CSVTableRow(CSVTable<T> table, T row)
        {
            this.Table = table;
            this.Fields = row;
        }

        #endregion ctor

        #region ClassMembers

        //public CSVTable<T> Table => (CSVTable<T>)this.Table;

        #endregion ClassMembers

        #region ImplimentITableRow

        public T Fields { get; protected set; }
        public long ID { get; protected set; }
        public ITable<T> Table { get; protected set; }

        #endregion ImplimentITableRow
    }
}