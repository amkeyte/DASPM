using Microsoft.VisualStudio.TestTools.UnitTesting;
using DASPM.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DASPMTests.Report.Mocks;

namespace DASPM.Table.Tests
{
    [TestClass()]
    public class CSVTableRowTests
    {
        private string TestFiles { get => @"source\repos\DASPM\DASPMTests\TestFiles"; }
        private string UserFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }

        [TestMethod()]
        public void CreateTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var table = CSVTable.Create(name, fullPath,
                typeof(MockTable1),
                typeof(MockTableRow1),
                typeof(MockRowModel1));

            var model = new MockRowModel1();
            var row = CSVTableRow.Create(table, model, typeof(MockTableRow1), typeof(MockRowModel1));

            Assert.AreEqual(typeof(MockTableRow1), row.GetType());
            Assert.AreEqual(typeof(MockRowModel1), row.ModelType);
            Assert.AreEqual(typeof(MockRowModel1), row.Fields.GetType());
        }

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
        public void CreateMockTableRow1GenericTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var table = MockTable1<MockRowModel1>.Create(name, fullPath);
            var model = new MockRowModel1();
            var row = MockTableRow1<MockRowModel1>.Create(table, model);

            Assert.AreEqual(typeof(MockTableRow1<MockRowModel1>), row.GetType());
            Assert.AreEqual(typeof(MockRowModel1), row.ModelType);
            Assert.AreEqual(typeof(MockRowModel1), row.Fields.GetType());
        }

        //[TestMethod()]
        //public void CreateSpecifiedTest()
        //{
        //    string name = "Test1";
        //    string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
        //    var table = CSVTable.Create(name, fullPath, typeof(MockRowModel1));
        //    var model = new MockRowModel1();
        //    var row = CSVTableRow.Create(table, model, typeof(MockTableRow1), typeof(MockRowModel1));

        //    Assert.AreEqual(typeof(MockTableRow1), row.GetType());
        //    Assert.AreEqual(typeof(MockRowModel1), row.ModelType);
        //    Assert.AreEqual(typeof(MockRowModel1), row.Fields.GetType());
        //}

        [TestMethod()]
        public void CreateGenericTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");

            var table = CSVTable<MockRowModel1>.Create(name, fullPath,
                typeof(MockTable1<MockRowModel1>),
                typeof(MockTableRow1));

            var model = new MockRowModel1();

            var row = CSVTableRow<MockRowModel1>.Create(table, model,
                typeof(MockTableRow1<MockRowModel1>));

            Assert.AreEqual(typeof(MockTableRow1<MockRowModel1>), row.GetType());
            Assert.AreEqual(typeof(MockRowModel1), row.ModelType);
            Assert.AreEqual(typeof(MockRowModel1), row.Fields.GetType());
        }
    }
}