using System;
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
    public class CSVTable
    {
        public static CSVTable<IRowModel> Create(string name, string fullPath)
        {
            return new CSVTable<IRowModel>(name, fullPath);
        }
    }

    public class CSVTable<TModel> : ITable<TModel> where TModel : IRowModel
    {
        #region ctor

        public CSVTable(string name, string fullPath)
        {
            this.Name = name;
            this.FullPath = fullPath;
            this.Path = System.IO.Path.GetDirectoryName(fullPath);
            this.Filename = System.IO.Path.GetFileName(fullPath);
            this.Rows = new List<ITableRow<TModel>>();
        }

        #endregion ctor

        #region ImplementITable

        public ClassMap ClassMap { get; protected set; }

        public long Count
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
        public IList<ITableRow<TModel>> Rows { get; protected set; }
        //public ITableRow<T> AddRow(ITableRow<T> row)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public ITableRow<TModel> Row(int id)
        {
            return Rows.ElementAt(id);
        }

        #endregion ImplementITable

        #region ClassMembers

        public string Filename { get; protected set; }

        public string FullPath { get; protected set; }

        public string Path { get; protected set; }

        public virtual void AddRow(TModel model)
        {
            //default. override for specific use case
            var row = CreateRow(model);
            this.Rows.Add(row);
        }

        public virtual ITableRow<TModel> CreateRow(TModel model)
        {
            //defualt implementation. override for specific TableRow constructor
            return new CSVTableRow<TModel>(this, model);
        }

        //short form access
        public virtual TModel this[int index]
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

        #endregion ClassMembers

        #region CSVHelper

        public void LoadFromFile()
        {
            using (var reader = new StreamReader(FullPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                ConfigureCsvReader(csv);
                ClassMap = csv.Configuration.Maps[typeof(TModel)];
                var records = csv.GetRecords<TModel>();
                Rows.Clear();
                foreach (var r in records)
                {
                    this.AddRow(r);
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
                ClassMap = csv.Configuration.Maps[typeof(TModel)];
                var rowModels = new List<TModel>();
                foreach (var r in Rows)
                {
                    rowModels.Add(r.Fields);
                }

                csv.WriteRecords(rowModels);
            }
        }

        protected virtual void ConfigureCsvReader(CsvReader csv)
        {
            //stub. Override to add a classmap, etc...
        }

        protected virtual void ConfigureCsvWriter(CsvWriter csv)
        {
            //stub. Override to add a classmap, etc...
        }

        #endregion CSVHelper
    }
}