using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace DASPM.Table
{
    public abstract class CSVTable : ITable
    {
        //provide default constructor so creation of the correct descendant type is posible
        public CSVTable()
        {
        }

        public string Filename { get; protected set; }

        public string FullPath { get; protected set; }

        public Type ModelType { get; protected set; }

        public string Path { get; protected set; }

        public Type RowType { get; protected set; }

        public void Initialize(string name, string fullPath, Type rowType, Type modelType)
        {
            this.Path = System.IO.Path.GetDirectoryName(fullPath);
            this.Filename = System.IO.Path.GetFileName(fullPath);

            //initialized to prevent possible null access errors down the line
            this._rows = new List<object>();

            this.ModelType = modelType;
            this.RowType = rowType;

            this.Name = name;
            this.FullPath = fullPath;

            //add an initialized state field to ensure correct creation.
        }

        #region ImplementITable

        //public while testing!! TODO should be protected... fix this hack
        public List<object> _rows;

        public ClassMap ClassMap { get; protected set; }

        public int Count
        {
            get
            {
                return Rows.Count;
            }
        }

        public IList<string> Headers
        {
            get
            {
                //use the classmap to get the names of headers(?)
                if (ClassMap is null) return null;
                var result = new List<String>();

                foreach (var i in ClassMap.MemberMaps)
                {
                    result.Add(i.Data.Names[0]); //assumes only one name has been added to classmap.
                }
                //just accessing properties will not give actual text values if they aren't usable as valid property names
                return result;
            }
        }

        public string Name { get; protected set; }

        //the table is backed by a List<object> so that casting can be achieved
        //all accesses to the various Rows properties will need to cast in and out
        //of type object to present the correct interface to the client code.

        //this is used to make sure there is only one backing rows list, even when the
        //Rows property is called from hiding accessors of descendant classes.

        //because of casting difficulties and also to promote the integrity of the list,
        //copies are used. If this is a performance bottleneck look into caching.
        public IList<ITableRow> Rows
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
            //probably don't need this.
            protected set
            {
                throw new NotImplementedException("this got called!!");
#pragma warning disable CS0162 // Unreachable code detected
                _rows = new List<object>();
                foreach (var row in value)
                {
                    _rows.Add(row);
                }
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public ITableRow Row(int id)
        {
            return Rows.ElementAt(id);
        }

        #endregion ImplementITable

        #region ClassMembers

        //short form access
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
            return CSVTableRowBuilder.Create(this, model, RowType);
        }

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

        #region CSVHelper

        protected virtual void ConfigureCsvReader(CsvReader csv)
        {
            //stub. Override to add a classmap, etc...
        }

        protected virtual void ConfigureCsvWriter(CsvWriter csv)
        {
            //stub. Override to add a classmap, etc...
        }

        public void LoadFromFile()
        {
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

        public void WriteToFile(string newFileFullPath)
        {
            using (var writer = new StreamWriter(newFileFullPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                //register the map to ignore some properties in IRowModel, etc...

                ConfigureCsvWriter(csv);
                ClassMap = csv.Configuration.Maps[ModelType];
                var rowModels = new List<object>();
                foreach (var r in Rows)
                {
                    rowModels.Add(r.Fields);
                }

                csv.WriteRecords(rowModels);
            }
        }

        #endregion CSVHelper
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