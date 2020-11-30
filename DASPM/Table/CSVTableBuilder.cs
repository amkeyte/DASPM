using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    public class CSVTableBuilder
    {
        private static bool TryValidateAssignable(Type tableType, Type rowType, Type modelType, out InvalidOperationException e)
        {
            e = null;

            //this is a CSVTable
            if (!typeof(CSVTable).IsAssignableFrom(tableType))
            {
                e = new InvalidOperationException("Invalid tableType: " + tableType);
                return false;
            }
            //The row is CSVTableRow
            if (!typeof(CSVTableRow).IsAssignableFrom(rowType))
            {
                e = new InvalidOperationException("Invalid rowType: " + rowType);
                return false;
            }

            //if (tableType.IsGenericTypeDefinition)
            //{
            //    e= new InvalidOperationException("Use 'CreateGeneric' variant to build table of generic type: " + tableType);
            //    return false;
            //}

            return true;
        }

        //factory pattern ensures initialize is called.
        public static ITable Create(string name, string fullPath, Type tableType, Type rowType, Type modelType)
        {
            if (!TryValidateAssignable(tableType, rowType, modelType, out var e)) throw e;

            //maybe not desired... it could be possible to have concrete table with generic rows?
            //if (rowType.IsGenericTypeDefinition)
            //{
            //    throw new InvalidOperationException("[this is in test] Use 'CreateGeneric' variant to build table with rows of generic type: " + rowType);
            //}

            var table = (CSVTable)Activator.CreateInstance(tableType);
            table.Initialize(name, fullPath, rowType, modelType);
            return table;
        }

        //    //create with table type
        //    public static ITable<TModel> CreateGeneric<TModel>(
        //        string name, string fullPath,
        //        Type tableType,
        //        Type rowType,
        //        Type modelType)
        //        where TModel : IRowModel
        //    {
        //        if (!TryValidateAssignable(tableType, rowType, modelType, out var e)) throw e;

        //        var table = (ITable<TModel>)Activator.CreateInstance(tableType);
        //        ((CSVTable)table).Initialize(name, fullPath, rowType, modelType);
        //        return table;
        //    }
    }
}