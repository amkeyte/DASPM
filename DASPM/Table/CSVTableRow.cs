using CsvHelper.Configuration;
using System;

namespace DASPM.Table
{
    public abstract class CSVTableRow : ICSVHelperTableRow, ICreatableTableRow
    {
        #region ctor

        protected CSVTableRow()
        {
            //do nothing; placeholder.
        }

        #endregion ctor

        public void InitCreatableRow(ITable table, IRowModel model)
        {
            //give an initial model if it's empty TODO make this default values or something
            if (model is null)
            {
                model = (IRowModel)Activator.CreateInstance(model.GetType());
            }

            //probably would be better to create a local TryValidateModelType variant and remove
            //the dependency on CSVTable if possible?
            if (!(table is CSVTable csvTable)) throw new ArgumentException("table must be of type CSVTable");

            if (!csvTable.TryValidateModelType(model, out ArgumentException e)) throw e;

            Table = table;
            Fields = model;
        }

        #region ICSVHelperTableRow

        #region IHasClassMap

        public ClassMap ClassMap => ((IHasClassMap)Table).ClassMap;

        public virtual void InitClassMap(ClassMap classMap)
        {
            throw new NotImplementedException();
        }

        #endregion IHasClassMap

        #region ITableRow

        public IRowModel Fields { get; protected set; }
        public int ID { get; protected set; }
        public Type ModelType { get => Fields.GetType(); }
        public ITable Table { get; protected set; }

        #endregion ITableRow

        #endregion ICSVHelperTableRow
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