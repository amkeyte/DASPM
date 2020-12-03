using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    /// <summary>
    /// The data table. Contains a set of rows with a pre-defined set of data fields.
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Number of rows in table
        /// </summary>
        int Count { get; }

        /// <summary>
        /// A list of field headers
        /// </summary>
        IList<string> Headers { get; }

        /// <summary>
        /// The user friendly name of the table (is needed?)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Return a List of TableRows for accessing data
        /// </summary>
        IList<ITableRow> Rows { get; }

        /// <summary>
        /// Indexer to return the TableRow's model by row index of the Rows table for short form access
        /// Equivalent to [Table.Row(index).Fields].
        /// Assumes client code will hide the return type with the applicable model.
        /// Example usage: Table[10].Col1 = 5
        /// </summary>
        /// <param name="index">The row number</param>
        /// <returns></returns>
        IRowModel this[int index] { get; }

        /// <summary>
        /// Access an individual TableRow by number.
        /// </summary>
        /// <param name="id">The Row number</param>
        /// <returns>The requested row. Exception if out of range.</returns>
        ITableRow Row(int id);
    }

    ///// <typeparam name="TModel">The data model used for this table</typeparam>
    //public interface ITable<TModel> : ITable where TModel : IRowModel
    //{
    //    /// <summary>
    //    /// The file path assiciated with this table (shoule be moved to concrete, and merged into FullPath)
    //    /// </summary>
    //    //string FilePath { get; }

    //    /// <summary>
    //    /// Return a List of rows for accessing data
    //    /// </summary>
    //    new IList<ITableRow<TModel>> Rows { get; }

    //    /// <summary>
    //    /// Access an individual row by number.
    //    /// </summary>
    //    /// <param name="id">The Row number</param>
    //    /// <returns>The requested row. Exception if out of range.</returns>
    //    new ITableRow<TModel> Row(int id);
    //}
}