using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    public static class CSVTableRowBuilder
    {
        /// <summary>
        /// Non-Generic creation of the speificied CSVTableRow descendant. Model Type is inferred.
        /// </summary>
        /// <param name="table">The compatible table instance owning this row</param>
        /// <param name="model">An instance of the IRowModel to contain the data for this row</param>
        /// <param name="tableRowType">The CSVTableRow descendant that is compatible with modelType</param>
        /// <param name="modelType">The type of model to be used.</param>
        /// <returns></returns>
        public static ITableRow Create(CSVTable table, IRowModel model, Type tableRowType)
        {
            //probably do some verification on types used

            //if (tableRowType.ContainsGenericParameters)
            //{
            //    //probably need a better specific exception type.
            //    throw new InvalidOperationException("Do not use this factory for tables using the TModel parameter");
            //}

            var tableRow = (CSVTableRow)Activator.CreateInstance(tableRowType);
            tableRow.Initialize(table, model);
            return tableRow;
        }

        ///// <summary>
        ///// Fully qualified creation. Allows use of subtypes of TModel. Model type is inferred from model.
        ///// </summary>
        ///// <param name="table">Instance of table</param>
        ///// <param name="model">Instance of TModel or a subtype</param>
        ///// <param name="tableRowType">Type for the table row compatible with the model.</param>
        ///// <param name="modelType">TModel, or one of its subtypes.</param>
        ///// <returns>The created TableRow</returns>
        //public static ITableRow<TModel> CreateGeneric<TModel>(
        //    CSVTable table,
        //    IRowModel model,
        //    Type tableRowType)
        //    where TModel : IRowModel
        //{
        //    var tableRow = (ITableRow<TModel>)Activator.CreateInstance(tableRowType);
        //    ((CSVTableRow)tableRow).Initialize(table, model);
        //    return tableRow;
        //}
    }
}