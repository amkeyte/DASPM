using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    public interface ITableRow
    {
        /// <summary>
        /// The fields in this row
        /// </summary>
        IRowModel Fields { get; }

        /// <summary>
        /// This rows number in the table
        /// </summary>
        int ID { get; }

        /// <summary>
        /// The model type used for this table
        /// </summary>
        Type ModelType { get; }

        /// <summary>
        /// The table this row belongs to
        /// </summary>
        ITable Table { get; }
    }

    /// <summary>
    /// The row in a table
    /// </summary>
    /// <typeparam name="TModel">The Model type for this table</typeparam>
    public interface ITableRow<TModel> : ITableRow where TModel : IRowModel
    {
        /// <summary>
        /// The fields in this row
        /// </summary>
        new TModel Fields { get; }

        /// <summary>
        /// The table this row belongs to
        /// </summary>
        new ITable<TModel> Table { get; }
    }
}