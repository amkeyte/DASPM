using DASPM.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPMTests.Report.Mocks
{
    public class MockTable1 : CSVTable
    {
        public static MockTable1 Create(string name, string fullPath)
        {
            return (MockTable1)Create(name, fullPath,
                typeof(MockTable1),
                typeof(MockTableRow1),
                typeof(MockRowModel1));
        }
    }

    public class MockTable1<TModel> : CSVTable<TModel>
        where TModel : IRowModel
    {
        public static MockTable1<TModel> Create(string name, string fullPath)
        {
            return (MockTable1<TModel>)Create(name, fullPath,
                typeof(MockTable1<TModel>),
                typeof(MockTableRow1));
        }
    }
}