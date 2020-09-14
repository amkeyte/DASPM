using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    /// <summary>
    /// The data table. Contains a set of rows with a pre-defined set of data fields.
    /// </summary>
    /// <typeparam name="TModel">The data model used for this table</typeparam>
    public interface ITable<TModel> where TModel : IRowModel
    {
        /// <summary>
        /// Number of rows in table
        /// </summary>
        long Count { get; }
        
        /// <summary>
        /// The filename assigned to this table (should be moved to concrete because a table should not be required to be tied to a file)
        /// </summary>
        string Filename { get; }

        /// <summary>
        /// A list of field headers
        /// </summary>
        IList<string> Headers { get; }

        /// <summary>
        /// The user friendly name of the table (is needed?)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The file path assiciated with this table (shoule be moved to concrete, and merged into FullPath)
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Return a List of rows for accessing data
        /// </summary>
        IList<ITableRow<TModel>> Rows { get; }

        
        /// <summary>
        /// Depreciate... no longer applicable or used.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Access an individual row by number.
        /// </summary>
        /// <param name="id">The Row number</param>
        /// <returns>The requested row. Exception if out of range.</returns>
        ITableRow<TModel> Row(int id);

        /// <summary>
        /// The ClassMap for TModel. Available only after file access. It is assumed that only one ClassMap is being used.
        /// </summary>
        ClassMap ClassMap { get; }
    }
}