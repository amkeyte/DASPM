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

        /// <summary>
        /// Non-Generic creation of the speificied CSVTableRow descendant. Model Type is inferred.
        /// </summary>
        /// <param name="table">The compatible table instance owning this row</param>
        /// <param name="model">An instance of the IRowModel to contain the data for this row</param>
        /// <param name="tableRowType">The CSVTableRow descendant that is compatible with modelType</param>
        /// <param name="modelType">The type of model to be used.</param>
        /// <returns></returns>
        public static CSVTableRow Create(CSVTable table, IRowModel model, Type tableRowType)
        {
            //probably do some verification on types used

            if (tableRowType.ContainsGenericParameters)
            {
                //probably need a better specific exception type.
                throw new InvalidOperationException("Do not use this factory for tables using the TModel parameter");
            }

            var tableRow = (CSVTableRow)Activator.CreateInstance(tableRowType);
            tableRow.Initialize(table, model);
            return tableRow;
        }

        protected void Initialize(CSVTable table, IRowModel model)
        {
            //give an initial model if it's empty TODO make this default values or something
            if (model is null)
            {
                model = (IRowModel)Activator.CreateInstance(model.GetType());
            }

            if (!table.TryValidateModelType(model, out ArgumentException e)) throw e;

            Table = table;
            Fields = model;
            ModelType = model.GetType();
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

        /// <summary>
        /// Fully qualified creation. Allows use of subtypes of TModel. Model type is inferred from model.
        /// </summary>
        /// <param name="table">Instance of table</param>
        /// <param name="model">Instance of TModel or a subtype</param>
        /// <param name="tableRowType">Type for the table row compatible with the model.</param>
        /// <param name="modelType">TModel, or one of its subtypes.</param>
        /// <returns>The created TableRow</returns>
        public static CSVTableRow<TModel> CreateGeneric(CSVTable<TModel> table, TModel model, Type tableRowType)
        {
            //if these end up changing, cascade this down.

            var tableRow = (CSVTableRow<TModel>)Activator.CreateInstance(tableRowType);
            tableRow.Initialize(table, model);
            return tableRow;
        }

        //accepts base CSVTable to support CSVTable.CreateRow(Addrow?)
        public static CSVTableRow<TModel> CreateGeneric(CSVTable table, TModel model, Type tableRowType)
        {
            var tableRow = (CSVTableRow<TModel>)Activator.CreateInstance(tableRowType);
            tableRow.Initialize(table, model);
            return tableRow;
        }

        /// <summary>
        /// Shorthand creator for when the model type is TModel.
        /// </summary>
        /// <param name="table">Instance of compatible table</param>
        /// <param name="model">instance of TModel</param>
        /// <param name="tableRowType">Type of table row compatible with TModel</param>
        /// <returns>The created TableRow</returns>
        //public static CSVTableRow<TModel> Create(CSVTable<TModel> table, TModel model, Type tableRowType)
        //{
        //    return Create(table, model, tableRowType, typeof(TModel));
        //}

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