using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    public static class CSVRowModel
    {
        public static Dictionary<string, object> ToDict(IRowModel model, ClassMap   classMap)
        {
            //for each header, add the header text as key, and the field's contents as value
            throw new NotImplementedException();
        }

        public static List<object> ToList(IRowModel model, ClassMap classMap)
        {
            //use the ClassMap to get the index and name of each property, then add it to the list
            throw new NotImplementedException();


        }
}
