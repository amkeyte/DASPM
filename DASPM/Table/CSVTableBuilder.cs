using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    public static class CSVTableBuilder
    {
        private static bool TryValidateAssignable(Type tableType, Type rowType, Type modelType, out InvalidOperationException e)
        {
            e = null;

            //this is a CSVTable
            if (!typeof(ICSVHelperTable).IsAssignableFrom(tableType))
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

        ///// <summary>
        ///// Instanciates an empty model and then uses its ClassMapType to create an empty classmap of the correct type.
        ///// </summary>
        ///// <param name="modelType"></param>
        ///// <returns></returns>
        //public static ClassMap CreateClassMap(Type modelType)
        //{
        //    var model = (CSVRowModel)Activator.CreateInstance(modelType);
        //    var classMap = (ClassMap)Activator.CreateInstance(model.ClassMap);
        //    return classMap;
        //}

        //factory pattern ensures initialize is called.
        public static CSVTable CreateCSVTable(
            string name,
            string fullPath,
            Type tableType,
            Type rowType,
            Type modelType,
            Type classMapType)
        {
            if (!TryValidateAssignable(tableType, rowType, modelType, out var e)) throw e;

            //maybe not desired... it could be possible to have concrete table with generic rows?
            //if (rowType.IsGenericTypeDefinition)
            //{
            //    throw new InvalidOperationException("[this is in test] Use 'CreateGeneric' variant to build table with rows of generic type: " + rowType);
            //}

            var table = (CSVTable)Activator.CreateInstance(tableType);
            var classMap = (ClassMap)Activator.CreateInstance(classMapType);
            table.InitCreatableTable(rowType, modelType);
            table.InitClassMap(classMap);
            table.InitFileReadWrite(fullPath);
            table.Name = name;
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