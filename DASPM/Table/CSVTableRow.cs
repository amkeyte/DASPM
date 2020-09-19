using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    public class CSVTableRow<TModel> : ITableRow<TModel> where TModel : IRowModel
    {
        #region ctor

        public CSVTableRow(CSVTable<TModel> table, TModel row)
        {
            this.Table = table;
            this.Fields = row;
        }

        #endregion ctor

        #region ClassMembers

        //public CSVTable<T> Table => (CSVTable<T>)this.Table;

        #endregion ClassMembers

        #region ImplimentITableRow

        public TModel Fields { get; protected set; }
        public long ID { get; protected set; }
        public ITable<TModel> Table { get; protected set; }


        }
        #endregion ImplimentITableRow
    }
}