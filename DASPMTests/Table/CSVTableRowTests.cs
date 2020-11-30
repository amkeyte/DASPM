using DASPMTests.Table.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace DASPM.Table.Tests
{
    [TestClass()]
    public class CSVTableRowTests
    {
        private string TestFiles { get => @"source\repos\DASPM\DASPMTests\TestFiles"; }
        private string UserFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }

        //[TestMethod()]
        //public void CreateGenericTest()
        //{
        //    string name = "Test1";
        //    string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");

        //    var table = (MockTable1<MockRowModel1>)CSVTableBuilder.CreateGeneric<MockRowModel1>(
        //        name, fullPath,
        //        typeof(MockTable1<MockRowModel1>),
        //        typeof(MockTableRow1<MockRowModel1>),
        //        typeof(MockRowModel1));

        //    var model = new MockRowModel1();

        //    var row = (MockTableRow1<MockRowModel1>)CSVTableRowBuilder.CreateGeneric<MockRowModel1>(
        //        table,
        //        model,
        //        typeof(MockTableRow1<MockRowModel1>));

        //    Assert.AreEqual(typeof(MockTableRow1<MockRowModel1>), row.GetType());
        //    Assert.AreEqual(typeof(MockRowModel1), row.ModelType);
        //    Assert.AreEqual(typeof(MockRowModel1), row.Fields.GetType());
        //}

        //[TestMethod()]
        //public void CreateMockTableRow1GenericTest()
        //{
        //    string name = "Test1";
        //    string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");

        //    var table = MockTable1<MockRowModel1>.CreateGeneric(name, fullPath);
        //    var model = new MockRowModel1();
        //    var row = MockTableRow1<MockRowModel1>.CreateGeneric(table, model);

        //    Assert.AreEqual(typeof(MockTableRow1<MockRowModel1>), row.GetType());
        //    Assert.AreEqual(typeof(MockRowModel1), row.ModelType);
        //    Assert.AreEqual(typeof(MockRowModel1), row.Fields.GetType());
        //}

        [TestMethod()]
        public void CreateMockTableRow1Test()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var table = MockTable1.Create(name, fullPath);
            var model = new MockRowModel1();

            var row = MockTableRow1.Create(table, model);

            Assert.AreEqual(typeof(MockTableRow1), row.GetType());
            Assert.AreEqual(typeof(MockRowModel1), row.ModelType);
            Assert.AreEqual(typeof(MockRowModel1), row.Fields.GetType());
        }

        [TestMethod()]
        public void CreateTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var table = (CSVTable)CSVTableBuilder.Create(name, fullPath,
                typeof(MockTable1),
                typeof(MockTableRow1),
                typeof(MockRowModel1));
            var model = new MockRowModel1();

            var row = (CSVTableRow)CSVTableRowBuilder.Create(table, model, typeof(MockTableRow1));

            Assert.AreEqual(typeof(MockTableRow1), row.GetType());
            Assert.AreEqual(typeof(MockRowModel1), row.ModelType);
            Assert.AreEqual(typeof(MockRowModel1), row.Fields.GetType());
        }
    }
}