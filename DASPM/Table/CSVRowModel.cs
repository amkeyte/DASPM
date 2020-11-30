using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    public abstract class CSVRowModel
    {
        public static object GetFieldByIndex(int index, IRowModel model, ClassMap classMap)
        {
            //validate that model is matched to ClassMap
            if (model.GetType() != classMap.ClassType)
            {
                throw new ArgumentException("model type must be the same as type classMap.ClassType");
            }

            if (index < 0 || index > classMap.GetMaxIndex())
            {
                throw new ArgumentOutOfRangeException("Invalid index given");
            }

            var propName = classMap.MemberMaps[index].Data.Member.Name;
            return classMap.ClassType.GetProperty(propName).GetValue(model);
        }

        public static object GetFieldByModelPropertyName(string propName, IRowModel model, ClassMap classMap)
        {
            //validate that model is matched to ClassMap
            if (model.GetType() != classMap.ClassType)
            {
                throw new ArgumentException("model type must be the same as type classMap.ClassType");
            }

            return classMap.ClassType.GetProperty(propName).GetValue(model);
        }

        public static object GetFieldByName(string name, IRowModel model, ClassMap classMap)
        {
            //validate that model is matched to ClassMap
            if (model.GetType() != classMap.ClassType)
            {
                throw new ArgumentException("model type must be the same as type classMap.ClassType");
            }
            for (var i = 0; i < classMap.GetMaxIndex(); i++)
            {
                if (classMap.MemberMaps[i].Data.Names.Contains(name))
                {
                    var propName = classMap.MemberMaps[i].Data.Member.Name;
                    return classMap.ClassType.GetProperty(propName).GetValue(model);
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a dictionary containing the field name of each header as the key and it's field value.
        /// </summary>
        /// <param name="model">The model object containing information</param>
        /// <param name="classMap">The ClassMap object for the model</param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDict(IRowModel model, ClassMap classMap)
        {
            //validate that model is matched to ClassMap
            if (model.GetType() != classMap.ClassType)
            {
                throw new ArgumentException("model type must be the same as type classMap.ClassType");
            }

            var resDict = new Dictionary<string, object>();

            //for each header, add the header text as key, and the field's contents as value
            foreach (var memberMap in classMap.MemberMaps)
            {
                var fieldName = memberMap.Data.Names[0]; //assumes only one name
                resDict.Add(fieldName, GetFieldByName(fieldName, model, classMap));
            }

            return resDict;
        }

        /// <summary>
        /// returns a list if field values in order by classmap index
        /// </summary>
        /// <param name="model">The model object containing information</param>
        /// <param name="classMap">The ClassMap object for the model</param>
        /// <returns></returns>
        public static List<object> ToList(IRowModel model, ClassMap classMap)
        {
            //validate that model is matched to ClassMap
            if (model.GetType() != classMap.ClassType)
            {
                throw new ArgumentException("model type must be the same as type classMap.ClassType");
            }

            var resList = new List<object>();

            //for each possible index, add the field value to the list
            for (var i = 0; i < classMap.GetMaxIndex(); i++)
            {
                resList.Insert(i, GetFieldByIndex(i, model, classMap));
            }

            return resList;
        }
    }
}