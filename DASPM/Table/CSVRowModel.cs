using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    public class CSVRowModel : IRowModel
    {
        public List<string> Headers {
            get
            {
                //use the classmap to get the names of headers(?)
                var result = new List<String>();

                foreach (var i in GetClassMap<CSVRowModel>().MemberMaps)
                {
                    result.Add(i.Data.Names[0]);
                }
                //just accessing properties will not give actual text values if they aren't usable as valid property names
                throw new NotImplementedException();
            }
        }

        public Dictionary<string, object> ToDict()
        {
            //for each header, add the header text as key, and the field's contents as value
            throw new NotImplementedException();
        }

        public List<object> ToList()
        {
            //use the ClassMap to get the index and name of each property, then add it to the list
            throw new NotImplementedException();
        }
    }
}
