using DASPM.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPMTests.Table.Mocks
{
    //internal class MockTableRowCore
    //{
    //    private CSVTableRow BaseTableRow { get; set; }

    //    private MockTableRow1 TableRow { get; set; }

    //    public MockTableRowCore(MockTableRow1 tableRow)
    //    {
    //        TableRow = tableRow;
    //        BaseTableRow = tableRow;
    //        Table = (MockTable1)tableRow.Table;
    //    }

    //    public MockRowModel1 Fields => (MockRowModel1)BaseTableRow.Fields;
    //    public MockTable1 Table { get; protected set; }
    //}

    public class MockTableRow1 : CSVTableRow
    {
        public new MockRowModel1 Fields { get => (MockRowModel1)base.Fields; }

        public static MockTableRow1 Create(MockTable1 table, MockRowModel1 model)
        {
            return (MockTableRow1)CSVTableRowBuilder.Create(table, model, typeof(MockTableRow1));
        }

        #region ClassMembers

        public string TestProperty1 { get => "TestProperty1"; }

        #endregion ClassMembers
    }

    //public class MockTableRow1<TModel> : MockTableRow1, ITableRow<TModel>
    //    where TModel : MockRowModel1
    //{
    //    private MockTableRowCore Core { get; set; }

    //    public MockTableRow1()
    //    {
    //        Core = new MockTableRowCore(this);
    //    }

    //    public new MockRowModel1 Fields => Core.Fields;

    //    TModel ITableRow<TModel>.Fields => (TModel)Fields;

    //    public new MockTable1 Table => Core.Table;

    //    ITable<TModel> ITableRow<TModel>.Table => (ITable<TModel>)Table;

    //    public static MockTableRow1<TModel> CreateGeneric(
    //        MockTable1<TModel> table,
    //        TModel model)
    //    {
    //        return (MockTableRow1<TModel>)CSVTableRowBuilder.CreateGeneric<TModel>(
    //            table,
    //            model,
    //            typeof(MockTableRow1<TModel>));
    //    }
    //}
}