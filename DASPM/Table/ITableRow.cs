using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    /// <summary>
    /// The row in a table
    /// </summary>
    /// <typeparam name="TModel">The Model type for this table</typeparam>
    public interface ITableRow<TModel> where TModel : IRowModel
    {
        /// <summary>
        /// The fields in this row
        /// </summary>
        TModel Fields { get; }

        /// <summary>
        /// This rows number in the table
        /// </summary>
        long ID { get; }

        /// <summary>
        /// The table this row belongs to
        /// </summary>
        ITable<TModel> Table { get; }
    }
}