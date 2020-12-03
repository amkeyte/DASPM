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
        //factory pattern ensures initialize is called.
        public static ICreatableCSVTable CreateCSVTable(
            string name,
            string fullPath,
            Type tableType,
            Type rowType,
            Type modelType,
            Type classMapType)
        {
            var classMap = (ClassMap)Activator.CreateInstance(classMapType);

            if (!TryValidateTypes(tableType, rowType, modelType, classMap, out var e)) throw e;

            var table = (ICreatableCSVTable)Activator.CreateInstance(tableType);
            table.InitCreatableTable(rowType, modelType);
            table.InitClassMap(classMap);
            table.InitFileReadWrite(fullPath);
            table.Name = name;
            return table;
        }

        public static bool TryValidateTypes(
            Type tableType,
            Type rowType,
            Type modelType,
            ClassMap classMap,
            out InvalidOperationException e)
        {
            e = null;

            //this is a CSVTable
            if (!typeof(ICSVHelperTable).IsAssignableFrom(tableType))
            {
                e = new InvalidOperationException("Invalid tableType: " + tableType);
                return false;
            }
            //The row is CSVTableRow
            if (!typeof(ICSVHelperTableRow).IsAssignableFrom(rowType))
            {
                e = new InvalidOperationException("Invalid rowType: " + rowType);
                return false;
            }

            if (!typeof(IRowModel).IsAssignableFrom(modelType))
            {
                e = new InvalidOperationException("Invalid model type: " + modelType);
                return false;
            }

            if (!classMap.ClassType.IsAssignableFrom(modelType))
            {
                e = new InvalidOperationException("Invalid model type: " +
                    modelType + ". must match ClassMap base model type of: " + classMap.ClassType);
                return false;
            }

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