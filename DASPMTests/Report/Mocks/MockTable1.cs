using DASPM.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPMTests.Report.Mocks
{
    internal class MockTableCore
    {
        private CSVTable BaseTable { get; set; }

        private MockTable1 Table { get; set; }

        internal ITableRow Row(int id)
        {
            return Table.Row(id);
        }

        public MockTableCore(MockTable1 table)
        {
            Table = table;
            BaseTable = table;
        }

        public IList<ITableRow> Rows => BaseTable.Rows;

        //public IList<ITableRow<TModel>> GetRowsGeneric<TModel>() where TModel : MockRowModel1
        //{
        //    var result = new List<MockTableRow1<TModel>>();
        //    foreach (var row in BaseTable.Rows)
        //    {
        //        result.Add((MockTableRow1<TModel>)row);
        //    }

        //    return (IList<ITableRow<TModel>>)result;
        //}
    }

    public class MockTable1 : CSVTable
    {
        public static MockTable1 Create(string name, string fullPath)
        {
            return (MockTable1)CSVTableBuilder.Create(name, fullPath,
                typeof(MockTable1),
                typeof(MockTableRow1),
                typeof(MockRowModel1));
        }
    }

    //public class MockTable1<TModel> : MockTable1, ITable<TModel>
    //    where TModel : MockRowModel1
    //{
    //    private MockTableCore Core { get; set; }

    //    public MockTable1()
    //    {
    //        Core = new MockTableCore(this);
    //    }

    //    public new IList<MockTableRow1<MockRowModel1>> Rows => (IList<MockTableRow1<MockRowModel1>>)Core.GetRowsGeneric<MockRowModel1>();

    //    IList<ITableRow<TModel>> ITable<TModel>.Rows => (IList<ITableRow<TModel>>)Rows;

    //    public static MockTable1<TModel> CreateGeneric(
    //        string name,
    //        string fullPath)
    //    {
    //        return (MockTable1<TModel>)CSVTableBuilder.CreateGeneric<TModel>(
    //            name,
    //            fullPath,
    //            typeof(MockTable1<TModel>),
    //            typeof(MockTableRow1<TModel>),
    //            typeof(MockRowModel1));
    //    }

    //    ITableRow<TModel> ITable<TModel>.Row(int id) => (ITableRow<TModel>)Core.Row(id);

    //    public new MockTableRow1<MockRowModel1> Row(int id) => (MockTableRow1<MockRowModel1>)Core.Row(id);
    //}
}