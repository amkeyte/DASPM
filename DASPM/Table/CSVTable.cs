using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace DASPM.Table
{
    /// <summary>
    /// Abstract class that should hold most of the generic code for creation of table
    /// and interacting with the CSVHelper Library.
    /// Factory methods create instances using the correct types of ITableRow, IRowModel, and ClassMap for the
    /// client code.
    /// Descendant tables may hide accessor methods as required to provide the correct
    /// return types.
    /// </summary>
    public abstract class CSVTable : ICSVHelperTable, ICreatableTable
    {
        #region ctor

        //provide default constructor so creation of the correct descendant type is posible
        public CSVTable()
        {
            this._rows = new List<object>();
        }

        #endregion ctor

        #region ICSVHelperTable

        #region ICreatableTable

        public virtual Type ModelType { get; protected set; }
        public virtual Type TableRowType { get; protected set; }

        public virtual void InitCreatableTable(Type tableRowType, Type modelType)
        {
            TableRowType = tableRowType;
            ModelType = modelType;
        }

        #endregion ICreatableTable

        #region IHasClassMap

        /// <summary>
        /// ClassMap is used because it provides most of the reflection logic used to learn about the model
        /// </summary>
        public virtual ClassMap ClassMap { get; protected set; }

        public virtual void InitClassMap(ClassMap classMap)
        {
            ClassMap = classMap;
        }

        #endregion IHasClassMap

        #region IHasCSVConfig

        #region CsvReader

        private CsvReader _CsvReader;

        public CsvReader CsvReader
        {
            get
            {
                if (!CsvReaderReady) throw new InvalidOperationException("CsvReader is not ready.");
                return _CsvReader;
            }
            protected set
            {
                _CsvReader = value;
            }
        }

        public bool CsvReaderReady { get; protected set; }

        public virtual void ConfigureCsvReader(CsvReader csvReader)
        {
        }

        #endregion CsvReader

        #region CsvWriter

        private CsvWriter _CsvWriter;

        public CsvWriter CsvWriter
        {
            get
            {
                if (!CsvWriterReady) throw new InvalidOperationException("CsvWriter is not ready.");
                return _CsvWriter;
            }
            protected set
            {
                _CsvWriter = value;
            }
        }

        public bool CsvWriterReady { get; protected set; }

        public virtual void ConfigureCsvWriter(CsvWriter csvWriter)
        {
        }

        #endregion CsvWriter

        #endregion IHasCSVConfig

        #region IFileReadWritable

        public string DirPath { get => Path.GetDirectoryName(FullPath); }

        public string Filename { get => Path.GetFileName(FullPath); }

        public string FullPath { get; protected set; }

        public bool ReadWriteReady { get; protected set; }

        public void InitFileReadWrite(string fullPath)
        {
            FullPath = fullPath;
            ReadWriteReady = true;
        }

        public void LoadFromFile()
        {
            if (!ReadWriteReady) throw new InvalidOperationException("Read / Write is not ready.");

            using (var reader = new StreamReader(FullPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                ConfigureCsvReader(csv);
                ClassMap = csv.Configuration.Maps[ModelType];
                var records = csv.GetRecords(ModelType);
                //use internal list or else it will only clear the copy.
                _rows.Clear();
                foreach (var r in records)
                {
                    //for generics, this is calling the non-generic AddRow... need to fix.
                    this.AddRow((IRowModel)r);
                }
            }
        }

        #region Write

        private void WriteFile()
        {
            if (!ReadWriteReady) throw new InvalidOperationException("Read / Write is not ready.");

            using (var writer = new StreamWriter(FullPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                CsvWriter = csv;
                ConfigureCsvWriter(csv);//calls to descendant class
                CsvWriterReady = true; //might find some way to automate this?

                ClassMap = csv.Configuration.Maps[ModelType];//needed?

                var rowModels = new List<object>();
                foreach (var r in Rows)
                {
                    rowModels.Add(r.Fields);
                }

                csv.WriteRecords(rowModels);
                CsvWriterReady = false;
                CsvWriter = null;
            }
        }

        public void OverwriteFile()

        {
            WriteFile();
        }

        public void WriteNewFile(string fullPath)
        {
            FullPath = fullPath;
            WriteFile();
        }

        #endregion Write

        #endregion IFileReadWritable

        #region ITable

        //public while testing!! TODO should be protected... fix this hack
        public List<object> _rows;

        public int Count
        {
            get
            {
                return _rows.Count;
            }
        }

        public IList<string> Headers
        {
            get
            {
                //use the classmap to get the names of headers(?)
                //if (ClassMap is null)
                //{
                //    throw new InvalidOperationException("Headers is not initialized because no file has been loaded.");
                //}
                var result = new List<string>();

                foreach (var i in ClassMap.MemberMaps)
                {
                    //just accessing properties will not give actual text values if they aren't usable as valid property names
                    result.Add(i.Data.Names[0]); //assumes only one name has been added to classmap.
                }
                return result;
            }
        }

        public string Name { get; set; }
        //the table is backed by a List<object> so that casting can be achieved
        //all accesses to the various Rows properties will need to cast in and out
        //of type object to present the correct interface to the client code.

        //this is used to make sure there is only one backing rows list, even when the
        //Rows property is called from hiding accessors of descendant classes.

        //because of casting difficulties and also to promote the integrity of the list,
        //copies are used. If this is a performance bottleneck look into caching.

        //note: since the TableRows are passed by reference, it is still possible to update
        //the table even though the list is a copy.
        public virtual IList<ITableRow> Rows
        {
            get
            {
                var result = new List<ITableRow>();
                foreach (var row in _rows)
                {
                    result.Add((ITableRow)row);
                }

                return result;
            }
        }

        public virtual IRowModel this[int index]
        {
            get
            {
                try
                {
                    return Rows[index].Fields;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    e.Data.Add("index", index);
                    e.Data.Add("Rows.Count", Rows.Count);
                    throw e;
                }
            }
        }

        /// <summary>
        /// Uses the model to create a new TableRow using CreateRow. Then adds it to the table
        /// and returns it.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The newly created row</returns>
        public virtual ITableRow AddRow(IRowModel model)
        {
            if (!TryValidateModelType(model, out ArgumentException e)) throw e;

            //default. override for specific use case
            var row = CreateRow(model);
            //using internal because writing to the Rows copy is not desired.
            this._rows.Add(row);
            return row;
        }

        public virtual ITableRow CreateRow(IRowModel model)
        {
            if (!TryValidateModelType(model, out ArgumentException e)) throw e;
            //if (RowType.ContainsGenericParameters)
            //{
            //    return CSVTableRowBuilder.CreateGeneric<IRowModel>(this, model, this.RowType);
            //}
            return CSVTableRowBuilder.Create(this, model, TableRowType);
        }

        public ITableRow Row(int id)
        {
            return (ITableRow)_rows.ElementAt(id);
        }

        #endregion ITable

        #endregion ICSVHelperTable

        #region ClassMembers

        //short form access

        //verify that model is of ModelType, since different branches of IRowModel could still be assigned.
        public bool TryValidateModelType(IRowModel model, out ArgumentException exception)
        {
            exception = null;
            if (!ModelType.IsAssignableFrom(model.GetType()))
            {
                exception = new ArgumentException("The model must be of type " + ModelType.FullName);
                return false;
            }
            return true;
        }

        #endregion ClassMembers
    }

    //    public class CSVTable<TModel> : CSVTable, ITable<TModel> where TModel : IRowModel
    //    {
    //        /// <summary>
    //        /// Returns a correctly typed copy of the internal rows list
    //        /// </summary>
    //        public new IList<ITableRow<TModel>> Rows
    //        {
    //            get
    //            {
    //                var result = new List<ITableRow<TModel>>();
    //                foreach (var row in _rows)
    //                {
    //                    result.Add((ITableRow<TModel>)row);
    //                }

    //                return result;
    //            }

    //            //probably dont need this
    //            protected set
    //            {
    //                throw new NotImplementedException("set got called!!");
    //#pragma warning disable CS0162 // Unreachable code detected
    //                _rows = (List<object>)value;
    //#pragma warning restore CS0162 // Unreachable code detected
    //            }
    //        }

    //        public virtual void AddRow(TModel model)
    //        {
    //            if (!TryValidateModelType(model, out ArgumentException e)) throw e;
    //            var row = CreateRow(model);
    //            //use internal list because writing to the Rows copy is not desired.
    //            _rows.Add(row);
    //        }

    //        public new ITableRow<TModel> Row(int id)
    //        {
    //            return Rows[id];
    //        }

    //        #region ClassMembers

    //        //short form access
    //        public virtual new TModel this[int index]
    //        {
    //            get
    //            {
    //                return (TModel)base[index];
    //            }
    //        }

    //        public virtual ITableRow<TModel> CreateRow(TModel model)
    //        {
    //            return (ITableRow<TModel>)base.CreateRow(model);
    //        }

    //        #endregion ClassMembers
    //    }
}