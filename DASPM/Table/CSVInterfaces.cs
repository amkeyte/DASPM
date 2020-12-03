using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    #region aggregate

    /// <summary>
    /// Provides functions for a table that uses the CSVHelper library.
    /// </summary>
    public interface ICSVHelperTable :
        ITable,
        IFileReadWritable,
        IHasCSVConfig,
        IHasClassMap
    {
    }

    /// <summary>
    /// Provides functions for a TableRow that uses the CSVHelper library
    /// </summary>
    public interface ICSVHelperTableRow :
        ITableRow,
        IHasClassMap
    { }

    #endregion aggregate

    /// <summary>
    /// Provides members for reflective creation of ITable descendants
    /// </summary>
    public interface ICreatableCSVTable : IHasClassMap, IFileReadWritable
    {
        Type ModelType { get; }
        string Name { set; }
        Type TableRowType { get; }

        void InitClassMap(ClassMap classMap);

        void InitCreatableTable(Type tableRowType, Type modelType);
    }

    public interface ICreatableTableRow
    {
        void InitCreatableRow(ITable table, IRowModel model);
    }

    //Handling for agnostic flat file persistance...
    public interface IFileReadWritable
    {
        string DirPath { get; }

        string Filename { get; }

        string FullPath { get; }
        bool ReadWriteReady { get; }

        void InitFileReadWrite(string fullPath);

        void LoadFromFile();

        void OverwriteFile();

        void WriteNewFile(string fullPath);
    }

    //keep broken out because only the table should give config info,
    //but TableRows or Models may also provide a ClassMap access.
    public interface IHasClassMap
    {
        ClassMap ClassMap { get; }
    }

    public interface IHasCSVConfig
    {
        CsvReader CsvReader { get; }

        bool CsvReaderReady { get; }
        CsvWriter CsvWriter { get; }
        bool CsvWriterReady { get; }

        void ConfigureCsvReader(CsvReader csvReader);

        void ConfigureCsvWriter(CsvWriter csvWriter);
    }
}