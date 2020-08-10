using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace DASPM.Table
{
    public class CSVTable<T> : ITable<T> where T : IRowModel
    {
        #region ctor

        public CSVTable(string name, string path, string filename)
        {
            this.Name = name;
            this.FilePath = path;
            this.Filename = filename;
            this.Rows = new List<ITableRow<T>>();
        }

        #endregion ctor

        #region ImplementITable

        public long Count
        {
            get
            {
                return Rows.Count;
            }
        }

        public string Filename { get; protected set; }

        public string FilePath { get; protected set; }
        public IList<string> Header { get; protected set; }

        public string Name { get; protected set; }
        public IList<ITableRow<T>> Rows { get; protected set; }

        //public ITableRow<T> AddRow(ITableRow<T> row)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public ITableRow<T> Row(int id)
        {
            return Rows.ElementAt(id);
        }

        #endregion ImplementITable

        #region ClassMembers

        public virtual void AddRow(T model)
        {
            //default. override for specific use case
            var row = CreateRow(model);
            this.Rows.Add(row);
        }

        public virtual ITableRow<T> CreateRow(T model)
        {
            //defualt implementation. override for specific TableRow constructor
            return new CSVTableRow<T>(this, model);
        }

        //short form access
        public virtual T this[int index]
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
            using (var reader = new StreamReader(Path.Combine(FilePath, Filename)))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                ConfigureCsvReader(csv);
                var records = csv.GetRecords<T>();
                Rows.Clear();
                foreach (var r in records)
                {
                    this.AddRow(r);
                }
            }
        }

        public void WriteToFile(string path, string filename)
        {
            using (var writer = new StreamWriter(Path.Combine(path, filename)))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                //register the map to ignore some properties in IRowModel, etc...

                ConfigureCsvWriter(csv);
                var rowModels = new List<T>();
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