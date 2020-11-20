using CsvHelper.Configuration;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    public class CSVTableRow : ITableRow
    {
        #region ctor

        ////factory pattern - default row type
        //public static CSVTableRow Create(CSVTable table, IRowModel model, Type modelType)
        //{
        //    CSVTableRow tableRow = new CSVTableRow();
        //    tableRow.Initialize(table, model, modelType);
        //    return tableRow;
        //}

        //factory pattern - specified row type
        public static CSVTableRow Create(CSVTable table, IRowModel model, Type tableRowType, Type modelType)
        {
            //probably do some verification on types used

            if (tableRowType.GenericTypeArguments.Length > 0)
            {
                //probably need a better specific exception type.
                throw new InvalidOperationException("Do not use this factory for tables using the TModel parameter");
            }

            var tableRow = (CSVTableRow)Activator.CreateInstance(tableRowType);
            tableRow.Initialize(table, model, modelType);
            return tableRow;
        }

        protected void Initialize(CSVTable table, IRowModel model, Type modelType)
        {
            //give an initial model if it's empty TODO make this default values or something
            if (model is null)
            {
                model = (IRowModel)Activator.CreateInstance(modelType);
            }

            if (!table.TryValidateModelType(model, out ArgumentException e)) throw e;

            Table = table;
            Fields = model;
            ModelType = modelType;
        }

        protected CSVTableRow()
        {
            //do nothing; placeholder.
        }

        #endregion ctor

        #region ClassMembers

        public Type ModelType { get; protected set; }

        #endregion ClassMembers

        #region ImplimentITableRow

        public IRowModel Fields { get; protected set; }
        public long ID { get; protected set; }
        public ITable Table { get; protected set; }

        #endregion ImplimentITableRow
    }

    public class CSVTableRow<TModel> : CSVTableRow, ITableRow<TModel>
        where TModel : IRowModel
    {
        #region ctor

        ////create with default type
        //public static CSVTableRow<TModel> Create(CSVTable<TModel> table, TModel model)
        //{
        //    var tableRow = new CSVTableRow<TModel>();
        //    tableRow.Initialize(table, model, typeof(TModel));
        //    return tableRow;
        //}

        //create with table row type
        public static CSVTableRow<TModel> Create(CSVTable<TModel> table, TModel model, Type tableRowType)
        {
            var tableRow = (CSVTableRow<TModel>)Activator.CreateInstance(tableRowType);
            tableRow.Initialize(table, model, typeof(TModel));
            return tableRow;
        }

        #endregion ctor

        #region ImplimentITableRow

        new public TModel Fields
        {
            get
            {
                return (TModel)base.Fields;
            }
            protected set
            {
                base.Fields = value;
            }
        }

        new public ITable<TModel> Table
        {
            get
            {
                return (ITable<TModel>)base.Table;
            }
            protected set
            {
                base.Table = Table;
            }
        }

        #endregion ImplimentITableRow
    }
}