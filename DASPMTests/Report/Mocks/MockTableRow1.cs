using DASPM.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPMTests.Report.Mocks
{
    public class MockTableRow1 : CSVTableRow
    {
        public static MockTableRow1 Create(MockTable1 table, MockRowModel1 model)
        {
            return (MockTableRow1)Create(table, model, typeof(MockTableRow1));
        }
    }

    public class MockTableRow1<TModel> : CSVTableRow<TModel>
        where TModel : IRowModel
    {
        public static MockTableRow1<TModel> CreateGeneric(MockTable1<TModel> table, TModel model)
        {
            return (MockTableRow1<TModel>)CreateGeneric(table, model,
                typeof(MockTableRow1<TModel>));
        }
    }
}