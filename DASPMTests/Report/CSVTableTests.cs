using DASPM.Table;
using DASPMTests.Table.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DASPM.Table.Tests
{
    [TestClass()]
    public class CSVTableTests
    {
        private class TestSetup
        {
            #region general

            public static string TestFiles { get => @"source\repos\DASPM\DASPMTests\TestFiles"; }

            public static string UserFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }

            public static void Reset()
            {
                _Table2 = null;
                _Table3A = null;
                _Table3B = null;

                //clear the old writetofile results
                var files = Directory.GetFiles(Path.Combine(UserFolder, TestFiles), Filename3B);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }

            #endregion general

            //before LoadFromFile

            #region test1

            public static int Count1 { get => 0; }
            public static string Filename1 { get => @"Test1.csv"; }
            public static string FullPath1 { get => Path.Combine(UserFolder, TestFiles, Filename1); }
            public static IList<string> Headers1 { get => new List<string>() { "Col1", "Col2", "Col3" }; }
            public static string Name1 { get => "Test1"; }
            public static int RowsCount1 { get => 0; }
            public static MockTable1 Table1 { get => MockTable1.Create(Name1, FullPath1); }

            #endregion test1

            //after loadFromFile

            #region test2

            private static MockTable1 _Table2;

            public static int Count2 = 2;
            public static int Row0Col1_2 = 1;
            public static string Row1Col3_2 = "E";
            public static string Filename2 { get => @"Test1.csv"; }
            public static string FullPath2 { get => Path.Combine(UserFolder, TestFiles, Filename2); }
            public static IList<string> Headers2 { get => new List<string>() { "Col1", "Col2", "Col3" }; }
            public static string Name2 { get => "Test1"; }

            public static IList<MockTableRow1> Rows2
            {
                get => Table2.Rows;
            }

            public static MockTable1 Table2
            {
                get
                {
                    var table = _Table2;
                    if (table is null)
                    {
                        table = MockTable1.Create(Name2, FullPath2);
                    }
                    table.LoadFromFile();
                    return table;
                }
            }

            #endregion test2

            // A after loadfromfile, B  after WriteToFile - before LoadFromFile.

            #region test3

            private static MockTable1 _Table3A;
            private static MockTable1 _Table3B;

            public static int Count3 = 2;
            public static int Row0Col1_3A = 1;
            public static string Row0Col3_3B = "Changed";
            public static string Row1Col3_3A = "E";
            public static int Row1Col3_3B = 9000;
            public static string Filename3A { get => @"Test1.csv"; }
            public static string Filename3B { get => @"WriteToFileTest1.csv"; }
            public static string FullPath3A { get => Path.Combine(UserFolder, TestFiles, Filename3A); }
            public static string FullPath3B { get => Path.Combine(UserFolder, TestFiles, Filename3B); }

            public static IList<string> Headers3 { get => new List<string>() { "Col1", "Col2", "Col3" }; }
            public static string Name3 { get => "Test1"; }

            public static IList<MockTableRow1> Rows3A
            {
                get => Table3A.Rows;
            }

            public static IList<MockTableRow1> Rows3B
            {
                get => Table3B.Rows;
            }

            public static MockTable1 Table3A
            {
                get
                {
                    var table = _Table3A;
                    if (table is null)
                    {
                        table = MockTable1.Create(Name3, FullPath3A);
                    }
                    table.LoadFromFile();
                    return table;
                }
            }

            public static MockTable1 Table3B
            {
                get
                {
                    var table = _Table3B;
                    if (table is null)
                    {
                        table = MockTable1.Create(Name3, FullPath3B);
                    }
                    return table;
                }
            }

            #endregion test3
        }

        [TestMethod()]
        public void AddRowTest()
        {
            var table = TestSetup.Table1;

            table.AddRow();

            Assert.AreEqual(typeof(MockTableRow1), table.Rows[0].GetType());
            Assert.AreEqual(typeof(MockRowModel1), table.Rows[0].Fields.GetType());
            Assert.AreEqual(0, table.Rows[0].Fields.Col1);
        }

        [TestMethod()]
        public void AddRowTest_model()
        {
            var table = TestSetup.Table1;
            var model = new MockRowModel1();

            model.Col1 = 1;
            table.AddRow(model);
            var rowsTest = table.Rows;

            Assert.AreEqual(typeof(MockTableRow1), rowsTest[0].GetType());
            Assert.AreEqual(typeof(MockRowModel1), rowsTest[0].Fields.GetType());
            Assert.AreEqual(1, model.Col1);
        }

        //    Assert.AreEqual("Test1", nameTest);
        //    Assert.AreEqual(typeof(MockTable1<MockRowModel1>), tableTypeTest);
        //    Assert.AreEqual(typeof(MockRowModel1), rowTypeTest);
        //}
        [TestMethod()]
        public void ConfigureCsvReaderTest()
        {
            //            Assert.Fail();
        }

        //    var nameTest = tObj.Name;
        [TestMethod()]
        public void ConfigureCsvWriterTest()
        {
            //Assert.Fail();
        }

        //    var tableTypeTest = tObj.GetType();
        //    var rowTypeTest = tObj.ModelType;
        [TestMethod()]
        public void CreateMockTableTest()
        {
            var table = TestSetup.Table1;

            var tableTypeTest = table.GetType();
            var rowTypeTest = table.ModelType;

            var nameTest = table.Name;

            Assert.AreEqual("Test1", nameTest);
            Assert.AreEqual(typeof(MockTable1), tableTypeTest);
            Assert.AreEqual(typeof(MockRowModel1), rowTypeTest);
        }

        //[TestMethod()]
        //public void CreateMockTableGenericTest()
        //{
        //    string name = "Test1";
        //    string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
        //    var tObj = MockTable1<MockRowModel1>.CreateGeneric(name, fullPath);
        [TestMethod()]
        public void CreateRowTest()
        {
            var table = TestSetup.Table1;

            var model = new MockRowModel1();
            model.Col1 = 1;
            var row = table.CreateRow(model);

            Assert.AreEqual(typeof(MockTableRow1), row.GetType());
            Assert.AreEqual(typeof(MockRowModel1), row.Fields.GetType());
            Assert.AreEqual(1, model.Col1);
        }

        [TestMethod()]
        public void CSVTableAccessorsTest()
        {
            TestSetup.Reset();
            var table = TestSetup.Table2;
            table.LoadFromFile();

            //Get Row

            Assert.AreEqual(0, table.Row(0).ID);

            //get rows
            Assert.AreEqual(0, table.GetRows<CSVTableRow>()[0].ID);
            Assert.AreEqual(0, table.Rows[0].ID);

            //indexor
            Assert.IsTrue(table[0].GetType().GetInterfaces().Contains(typeof(ITableRow)));
        }

        //[TestMethod()]
        //public void ConvertFromGenericTest()
        //{
        //    string name = "Test1";
        //    string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
        //    var genericTable = MockTable1<MockRowModel1>.CreateGeneric(name, fullPath);

        //    ITable interfaceTable = genericTable;
        //    CSVTable csvTable = genericTable;

        //    csvTable.LoadFromFile();

        //    var row0Test = (MockRowModel1)interfaceTable.Row(0).Fields;
        //    var row1Test = (MockRowModel1)interfaceTable.Row(1).Fields;

        //    Assert.AreEqual(2, interfaceTable.Count);
        //    Assert.AreEqual(1, row0Test.Col1);
        //    Assert.AreEqual("E", row1Test.Col3);
        //}

        //[TestMethod()]
        //public void CreateGenericTest()
        //{
        //    string name = "Test1";
        //    string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
        //    var tObj = CSVTableBuilder.CreateGeneric<MockRowModel1>(name, fullPath,
        //        typeof(MockTable1<MockRowModel1>),
        //        typeof(MockTableRow1<MockRowModel1>),
        //        typeof(MockRowModel1));

        //    var nameTest = tObj.Name;

        //    Assert.AreEqual("Test1", nameTest);
        //}
        //[TestMethod()]
        //public void LoadFromFileGenericTest()
        //{
        //    string name = "Test1";
        //    string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
        //    var table = MockTable1<MockRowModel1>.CreateGeneric(name, fullPath);

        //    table.LoadFromFile();
        //    var row0Test = table.Row(0).Fields;
        //    var row1Test = table.Row(1).Fields;

        //    Assert.AreEqual(2, table.Count);
        //    Assert.AreEqual(1, row0Test.Col1);
        //    Assert.AreEqual("E", row1Test.Col3);
        //}

        [TestMethod()]
        public void InitClassMapTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void InitCreatableTableTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void InitFileReadWriteTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void LoadFromFileTest()
        {
            TestSetup.Reset();
            var table = TestSetup.Table2;

            table.LoadFromFile();

            Assert.AreEqual(TestSetup.Count2, table.Count);
            Assert.AreEqual(TestSetup.Row0Col1_2, table.Row(0).Fields.Col1);
            Assert.AreEqual(TestSetup.Row1Col3_2, table.Row(1).Fields.Col3);
        }

        //[TestMethod()]
        //public void RowsGetGenericTest()
        //{
        //    string name = "Test1";
        //    string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
        //    var table = MockTable1<MockRowModel1>.CreateGeneric(name, fullPath);
        //    var model = new MockRowModel1();
        //    var row = MockTableRow1<MockRowModel1>.CreateGeneric(table, model);

        //    //requires _rows public hack
        //    table._rows.Add(row);

        //    Assert.AreEqual(typeof(List<MockTableRow1<MockRowModel1>>), table.Rows.GetType());
        //    Assert.AreEqual(typeof(MockTableRow1<MockRowModel1>), table.Rows[0].GetType());
        //    Assert.AreEqual(typeof(MockRowModel1), table.Rows[0].Fields.GetType());
        //}

        [TestMethod()]
        public void OverwriteFileTest()
        {
            //TODO
            //Assert.Fail();
        }

        [TestMethod()]
        public void Rows_GetTest()
        {
            var table = TestSetup.Table2;
            table.LoadFromFile();

            Assert.AreEqual(typeof(List<MockTableRow1>), table.Rows.GetType());
            Assert.AreEqual(typeof(MockTableRow1), table.Rows[0].GetType());
            Assert.AreEqual(typeof(MockRowModel1), table.Rows[0].Fields.GetType());
        }

        //[TestMethod()]
        //public void WriteToFileGenericTest()
        //{
        //    //***read file
        //    string path = Path.Combine(UserFolder, TestFiles);

        //    string name = "Test1";
        //    string test1FullPath = Path.Combine(path, @"Test1.csv");
        //    var tObj = MockTable1<MockRowModel1>.CreateGeneric(name, test1FullPath);
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
        //    string wfile = Path.Combine(path, wfilename);
        //    var files = Directory.GetFiles(path, wfilename);
        //    foreach (string file in files)
        //    {
        //        File.Delete(file);
        //    }
        //    tObj.WriteToFile(Path.Combine(path, wfile));

        //    //*** Unit Test
        //    var tObj2 = MockTable1<MockRowModel1>.CreateGeneric(name, wfile);
        //    tObj2.LoadFromFile();
        //    Assert.AreEqual(2, tObj2.Count);
        //    Assert.AreEqual("Changed", tObj2.Row(0).Fields.Col3);
        //    Assert.AreEqual(9000, tObj2.Row(1).Fields.Col1);
        //}

        [TestMethod()]
        public void RowTest()
        {
            TestSetup.Reset();
            var table = TestSetup.Table2;

            table.LoadFromFile();

            Assert.AreEqual(typeof(MockTableRow1), table.Row(0).GetType());
            Assert.AreEqual(typeof(MockRowModel1), table.Row(0).ModelType);
            Assert.AreEqual(0, table.Row(0).ID);
        }

        [TestMethod()]
        public void TryValidateModelTypeTest()
        {
            var table = TestSetup.Table1;
            ArgumentException e;
            var model1 = new MockRowModel1();
            var model2 = new MockRowModel2();

            Assert.IsTrue(table.TryValidateModelType(model1, out e));
            Debug.Print("Validation passed.");
            Assert.IsFalse(table.TryValidateModelType(model2, out e));
            Debug.Print(e.Message);
        }

        [TestMethod()]
        public void WriteNewFileTest()
        {
            //***read file
            TestSetup.Reset();
            var table1 = TestSetup.Table3A;

            //integrity check
            Assert.AreEqual(TestSetup.Count3, table1.Count);
            Assert.AreEqual(TestSetup.Row0Col1_3A, table1.Row(0).Fields.Col1);
            Assert.AreEqual(TestSetup.Row1Col3_3A, table1.Row(1).Fields.Col3);

            //***edit table
            table1.Row(0).Fields.Col3 = "Changed";
            table1.Row(1).Fields.Col1 = 9000;

            //integrity check
            Assert.AreEqual(2, table1.Count);
            Assert.AreEqual(TestSetup.Row0Col1_3A, table1.Row(0).Fields.Col1);
            Assert.AreEqual(TestSetup.Row1Col3_3A, table1.Row(1).Fields.Col3);

            //***write file

            table1.WriteNewFile(TestSetup.FullPath3B);
            //*** Unit Test

            var table2 = TestSetup.Table3B;
            table2.LoadFromFile();

            Assert.AreEqual(TestSetup.Count3, table2.Count);
            Assert.AreEqual(TestSetup.Row0Col3_3B, table2.Row(0).Fields.Col3);
            Assert.AreEqual(TestSetup.Row1Col3_3B, table2.Row(1).Fields.Col1);
        }
    }
}