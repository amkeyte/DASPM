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
    public class CSVReportTests
    {
        [TestMethod()]
        public void AddRowTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CSVReportTest()
        {
            string name = "Test1";
            string path = @"C:\Users\Aaron\source\repos\DASPM\DASPMTests\TestFiles\";
            string filename = @"Test1.csv";
            var tObj = new CSVTable<MockRowModel1>(name, path, filename);
        }

        [TestMethod()]
        public void LoadFromFileTest()
        {
            string name = "Test1";
            string path = @"C:\Users\Aaron\source\repos\DASPM\DASPMTests\TestFiles\";
            string filename = @"Test1.csv";
            var tObj = new CSVTable<MockRowModel1>(name, path, filename);

            tObj.LoadFromFile();

            Assert.AreEqual(2, tObj.Count);
            Assert.AreEqual(1, tObj.Row(0).Fields.Col1);
            Assert.AreEqual("E", tObj.Row(1).Fields.Col3);
        }

        [TestMethod()]
        public void RefreshTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RowTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WriteToFileTest()
        {
            //***read file
            string name = "Test1";
            string path = @"C:\Users\Aaron\source\repos\DASPM\DASPMTests\TestFiles\";
            string filename = @"Test1.csv";
            var tObj = new CSVTable<MockRowModel1>(name, path, filename);
            tObj.LoadFromFile();
            //integrity check
            Assert.AreEqual(2, tObj.Count);
            Assert.AreEqual(1, tObj.Row(0).Fields.Col1);
            Assert.AreEqual("E", tObj.Row(1).Fields.Col3);

            //***edit file
            tObj.Row(0).Fields.Col3 = "Changed";
            tObj.Row(1).Fields.Col1 = 9000;

            //integrity check
            Assert.AreEqual(2, tObj.Count);
            Assert.AreEqual("Changed", tObj.Row(0).Fields.Col3);
            Assert.AreEqual(9000, tObj.Row(1).Fields.Col1);

            //***write file
            string wfilename = @"WriteToFileTest1.csv";
            var files = Directory.GetFiles(path, wfilename);
            foreach (string file in files)
            {
                File.Delete(file);
            }
            tObj.WriteToFile(path, wfilename);

            //*** Unit Test
            var tObj2 = new CSVTable<MockRowModel1>(name, path, wfilename);
            tObj2.LoadFromFile();
            Assert.AreEqual(2, tObj2.Count);
            Assert.AreEqual("Changed", tObj2.Row(0).Fields.Col3);
            Assert.AreEqual(9000, tObj2.Row(1).Fields.Col1);
        }
    }
}