using Microsoft.VisualStudio.TestTools.UnitTesting;
using DASPM.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DASPMTests.Report.Mocks;
using System.IO;

namespace DASPM.Table.Tests
{
    [TestClass()]
    public class CSVTableTests
    {
        private string TestFiles { get => @"source\repos\DASPM\DASPMTests\TestFiles"; }
        private string UserFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }

        //[TestMethod()]
        //public void CreateWithDefaultTableTypeTest()
        //{
        //    string name = "Test1";
        //    string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
        //    var tObj = CSVTable.Create(name, fullPath, typeof(MockRowModel1));

        //    var nameTest = tObj.Name;

        //    Assert.AreEqual("Test1", nameTest);
        //}

        [TestMethod()]
        public void CreateTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var tObj = CSVTable.Create(name, fullPath, typeof(CSVTable), typeof(CSVTableRow), typeof(MockRowModel1));

            var nameTest = tObj.Name;

            Assert.AreEqual("Test1", nameTest);
        }

        [TestMethod()]
        public void CreateWithExendedTableTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var tObj = CSVTable.Create(name, fullPath,
                typeof(MockTable1),
                typeof(MockTableRow1),
                typeof(MockRowModel1));

            var tableTypeTest = tObj.GetType();
            var rowTypeTest = tObj.ModelType;

            var nameTest = tObj.Name;

            Assert.AreEqual("Test1", nameTest);
            Assert.AreEqual(typeof(MockTable1), tableTypeTest);
            Assert.AreEqual(typeof(MockRowModel1), rowTypeTest);
        }

        [TestMethod()]
        public void CreateMockTableTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var tObj = MockTable1.Create(name, fullPath);

            var tableTypeTest = tObj.GetType();
            var rowTypeTest = tObj.ModelType;

            var nameTest = tObj.Name;

            Assert.AreEqual("Test1", nameTest);
            Assert.AreEqual(typeof(MockTable1), tableTypeTest);
            Assert.AreEqual(typeof(MockRowModel1), rowTypeTest);
        }

        [TestMethod()]
        public void CreateMockTableGenericTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var tObj = MockTable1<MockRowModel1>.Create(name, fullPath);

            var tableTypeTest = tObj.GetType();
            var rowTypeTest = tObj.ModelType;

            var nameTest = tObj.Name;

            Assert.AreEqual("Test1", nameTest);
            Assert.AreEqual(typeof(MockTable1<MockRowModel1>), tableTypeTest);
            Assert.AreEqual(typeof(MockRowModel1), rowTypeTest);
        }

        [TestMethod()]
        public void CreateGenericTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var tObj = CSVTable<MockRowModel1>.Create(name, fullPath,
                typeof(MockTable1<MockRowModel1>),
                typeof(MockTableRow1<MockRowModel1>));

            var nameTest = tObj.Name;

            Assert.AreEqual("Test1", nameTest);
        }

        //[TestMethod()]
        //public void CreateWithDefaultTableTypeGenericTest()
        //{
        //    string name = "Test1";
        //    string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
        //    var tObj = CSVTable<MockRowModel1>.Create(name, fullPath);

        //    var nameTest = tObj.Name;

        //    Assert.AreEqual("Test1", nameTest);
        //}

        [TestMethod()]
        public void RowsGetTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var table = MockTable1.Create(name, fullPath);
            var model = new MockRowModel1();
            var row = MockTableRow1.Create(table, model);

            //requires _rows public hack
            table._rows.Add(row);

            Assert.AreEqual(typeof(List<ITableRow>), table.Rows.GetType());
            Assert.AreEqual(typeof(MockTableRow1), table.Rows[0].GetType());
            Assert.AreEqual(typeof(MockRowModel1), table.Rows[0].Fields.GetType());
        }

        [TestMethod()]
        public void RowsGetGenericTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var table = MockTable1<MockRowModel1>.Create(name, fullPath);
            var model = new MockRowModel1();
            var row = MockTableRow1<MockRowModel1>.Create(table, model);

            //requires _rows public hack
            table._rows.Add(row);

            Assert.AreEqual(typeof(List<ITableRow<MockRowModel1>>), table.Rows.GetType());
            Assert.AreEqual(typeof(MockTableRow1<MockRowModel1>), table.Rows[0].GetType());
            Assert.AreEqual(typeof(MockRowModel1), table.Rows[0].Fields.GetType());
        }

        [TestMethod()]
        public void CreateRowTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var table = MockTable1.Create(name, fullPath);

            var model = new MockRowModel1();
            model.Col1 = 1;
            var row = table.CreateRow(model);

            Assert.AreEqual(typeof(MockTableRow1), row.GetType());
            Assert.AreEqual(typeof(MockRowModel1), row.Fields.GetType());
            Assert.AreEqual(1, model.Col1);
        }

        [TestMethod()]
        public void AddRowTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var table = MockTable1.Create(name, fullPath);
            var model = new MockRowModel1();

            model.Col1 = 1;
            table.AddRow(model);
            var rowsTest = table.Rows;

            Assert.AreEqual(typeof(MockTableRow1), rowsTest[0].GetType());
            Assert.AreEqual(typeof(MockRowModel1), rowsTest[0].Fields.GetType());
            Assert.AreEqual(1, model.Col1);
        }

        [TestMethod()]
        public void LoadFromFileTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var table = MockTable1.Create(name, fullPath);

            table.LoadFromFile();
            var row0Test = (MockRowModel1)table.Row(0).Fields;
            var row1Test = (MockRowModel1)table.Row(1).Fields;

            Assert.AreEqual(2, table.Count);
            Assert.AreEqual(1, row0Test.Col1);
            Assert.AreEqual("E", row1Test.Col3);
        }

        [TestMethod()]
        public void LoadFromFileGenericTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var table = MockTable1<MockRowModel1>.Create(name, fullPath);

            table.LoadFromFile();
            var row0Test = table.Row(0).Fields;
            var row1Test = table.Row(1).Fields;

            Assert.AreEqual(2, table.Count);
            Assert.AreEqual(1, row0Test.Col1);
            Assert.AreEqual("E", row1Test.Col3);
        }

        //[TestMethod()]
        //public void WriteToFileTest()
        //{
        //    //***read file
        //    string path = Path.Combine(UserFolder, TestFiles);

        //    string name = "Test1";
        //    string test1FullPath = Path.Combine(path,@"Test1.csv");
        //    var tObj = new CSVTable(name, test1FullPath,typeof(MockRowModel1));
        //    tObj.LoadFromFile();

        //    var row0Fields = (MockRowModel1)tObj.Row(0).Fields;
        //    var row1Fields = (MockRowModel1)tObj.Row(1).Fields;

        //    //integrity check
        //    Assert.AreEqual(2, tObj.Count);
        //    Assert.AreEqual(1, row0Fields.Col1);
        //    Assert.AreEqual("E", row1Fields.Col3);

        //    //***edit table
        //    row0Fields.Col3 = "Changed";
        //    row1Fields.Col1 = 9000;

        //    //integrity check
        //    Assert.AreEqual(2, tObj.Count);
        //    Assert.AreEqual("Changed", row0Fields.Col3);
        //    Assert.AreEqual(9000, row1Fields.Col1);

        //    //***write file
        //    string wfilename = @"WriteToFileTest1.csv";
        //    var files = Directory.GetFiles(path, wfilename);
        //    foreach (string file in files)
        //    {
        //        File.Delete(file);
        //    }
        //    tObj.WriteToFile(Path.Combine(path, wfilename));

        //    //*** Unit Test

        //    var tObj2 = new CSVTable(name, Path.Combine(path, wfilename),typeof(MockRowModel1));
        //    tObj2.LoadFromFile();

        //    var row0Fields2 = (MockRowModel1)tObj2.Row(0).Fields;
        //    var row1Fields2 = (MockRowModel1)tObj2.Row(1).Fields;

        //    Assert.AreEqual(2, tObj2.Count);
        //    Assert.AreEqual("Changed", row0Fields2.Col3);
        //    Assert.AreEqual(9000, row1Fields2.Col1);
        //}

        //[TestMethod()]
        //public void WriteToFileGenericTest()
        //{
        //    //***read file
        //    string path = Path.Combine(UserFolder, TestFiles);

        //    string name = "Test1";
        //    string test1FullPath = Path.Combine(path, @"Test1.csv");
        //    var tObj = new CSVTable<MockRowModel1>(name, test1FullPath);
        //    tObj.LoadFromFile();

        //    //integrity check
        //    Assert.AreEqual(2, tObj.Count);
        //    Assert.AreEqual(1, tObj.Row(0).Fields.Col1);
        //    Assert.AreEqual("E", tObj.Row(1).Fields.Col3);

        //    //***edit file
        //    tObj.Row(0).Fields.Col3 = "Changed";
        //    tObj.Row(1).Fields.Col1 = 9000;

        //    //integrity check
        //    Assert.AreEqual(2, tObj.Count);
        //    Assert.AreEqual("Changed", tObj.Row(0).Fields.Col3);
        //    Assert.AreEqual(9000, tObj.Row(1).Fields.Col1);

        //    //***write file
        //    string wfilename = @"WriteToFileTest1.csv";
        //    var files = Directory.GetFiles(path, wfilename);
        //    foreach (string file in files)
        //    {
        //        File.Delete(file);
        //    }
        //    tObj.WriteToFile(Path.Combine(path, wfilename));

        //    //*** Unit Test
        //    var tObj2 = new CSVTable<MockRowModel1>(name, Path.Combine(path, wfilename));
        //    tObj2.LoadFromFile();
        //    Assert.AreEqual(2, tObj2.Count);
        //    Assert.AreEqual("Changed", tObj2.Row(0).Fields.Col3);
        //    Assert.AreEqual(9000, tObj2.Row(1).Fields.Col1);
        //}
    }
}