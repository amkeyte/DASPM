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

        protected CSVTableRow()
        {
            //do nothing; placeholder.
        }

        public void Initialize(CSVTable table, IRowModel model)
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

        #endregion ctor

        #region ClassMembers

        public Type ModelType { get; protected set; }

        #endregion ClassMembers

        #region ImplimentITableRow

        public IRowModel Fields { get; protected set; }
        public int ID { get; protected set; }
        public ITable Table { get; protected set; }

        #endregion ImplimentITableRow
    }

    //public class CSVTableRow<TModel> : CSVTableRow, ITableRow<TModel>
    //    where TModel : IRowModel
    //{
    //    #region ctor

    //    ////accepts base CSVTable to support CSVTable.CreateRow(Addrow?)
    //    //public static CSVTableRow<TModel> CreateGeneric(CSVTable table, TModel model, Type tableRowType)
    //    //{
    //    //    var tableRow = (CSVTableRow<TModel>)Activator.CreateInstance(tableRowType);
    //    //    tableRow.Initialize(table, model);
    //    //    return tableRow;
    //    //}

    //    /// <summary>
    //    /// Shorthand creator for when the model type is TModel.
    //    /// </summary>
    //    /// <param name="table">Instance of compatible table</param>
    //    /// <param name="model">instance of TModel</param>
    //    /// <param name="tableRowType">Type of table row compatible with TModel</param>
    //    /// <returns>The created TableRow</returns>
    //    //public static CSVTableRow<TModel> Create(CSVTable<TModel> table, TModel model, Type tableRowType)
    //    //{
    //    //    return Create(table, model, tableRowType, typeof(TModel));
    //    //}

    //    #endregion ctor

    //    #region ImplimentITableRow

    //    new public TModel Fields
    //    {
    //        get
    //        {
    //            return (TModel)base.Fields;
    //        }
    //        protected set
    //        {
    //            base.Fields = value;
    //        }
    //    }

    //    new public ITable<TModel> Table
    //    {
    //        get
    //        {
    //            return (ITable<TModel>)base.Table;
    //        }
    //        protected set
    //        {
    //            base.Table = Table;
    //        }
    //    }

    //    #endregion ImplimentITableRow
    //}
}