using Microsoft.VisualStudio.TestTools.UnitTesting;
using DASPM.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DASPMTests.Table.Mocks;
using CsvHelper.Configuration;
using System.Diagnostics;

namespace DASPM.Table.Tests
{
    [TestClass()]
    public class CSVTableBuilderTests
    {
        private class TestSetup
        {
            #region general

            public static string TestFiles { get => @"source\repos\DASPM\DASPMTests\TestFiles"; }

            public static string UserFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }

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
        }

        [TestMethod()]
        public void CreateCSVTableTest()
        {
            var table = (MockTable1)CSVTableBuilder.CreateCSVTable(
                TestSetup.Name1,
                TestSetup.FullPath1,
                typeof(MockTable1),
                typeof(MockTableRow1),
                typeof(MockRowModel1),
                typeof(MockRowModel1Map));

            //properties
            Assert.AreEqual("Test1", table.Name);
            Assert.AreEqual(typeof(MockRowModel1), table.ClassMap.ClassType);
            Assert.AreEqual(TestSetup.Count1, table.Count);
            Assert.AreEqual(TestSetup.Filename1, table.Filename);
            Assert.AreEqual("Col1", table.Headers[0]);
            Assert.AreEqual(TestSetup.RowsCount1, table.Rows.Count);
            //methods
            Assert.AreEqual(typeof(MockTable1), table.GetType());
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => table.Row(0));
            Assert.AreEqual("TestProperty", table.TestProperty1);
        }

        [TestMethod()]
        public void TryValidateTypesTest()
        {
            var classMap = new MockRowModel1Map();
            InvalidOperationException e;

            //passing
            Assert.IsTrue(
                CSVTableBuilder.TryValidateTypes(
                    typeof(MockTable1),
                    typeof(MockTableRow1),
                    typeof(MockRowModel1),
                    classMap,
                    out e));
            Debug.Print("Test passed. No Exception Occured.");

            //bad table
            Assert.IsFalse(
                CSVTableBuilder.TryValidateTypes(
                    typeof(string),
                    typeof(MockTableRow1),
                    typeof(MockRowModel1),
                    classMap,
                    out e));
            Debug.Print(e.Message);

            //bad tableRow
            Assert.IsFalse(
                CSVTableBuilder.TryValidateTypes(
                    typeof(MockTable1),
                    typeof(string),
                    typeof(MockRowModel1),
                    classMap,
                    out e));
            Debug.Print(e.Message);

            //bad model for IRowModel
            Assert.IsFalse(
                CSVTableBuilder.TryValidateTypes(
                    typeof(MockTable1),
                    typeof(MockTableRow1),
                    typeof(string),
                    classMap,
                    out e));
            Debug.Print(e.Message);

            //classmap / model mismatch
            Assert.IsFalse(
                CSVTableBuilder.TryValidateTypes(
                    typeof(MockTable1),
                    typeof(MockTableRow1),
                    typeof(MockRowModel2),
                    classMap,
                    out e));
            Debug.Print(e.Message);
        }
    }
}