﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private string TestFiles { get => @"source\repos\DASPM\DASPMTests\TestFiles"; }
        private string UserFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }

        [TestMethod()]
        public void CSVReportTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var tObj = new CSVTable<MockRowModel1>(name, fullPath);
        }

        [TestMethod()]
        public void LoadFromFileTest()
        {
            string name = "Test1";
            string fullPath = Path.Combine(UserFolder, TestFiles, @"Test1.csv");
            var tObj = new CSVTable<MockRowModel1>(name, fullPath);

            tObj.LoadFromFile();

            Assert.AreEqual(2, tObj.Count);
            Assert.AreEqual(1, tObj.Row(0).Fields.Col1);
            Assert.AreEqual("E", tObj.Row(1).Fields.Col3);
        }

        [TestMethod()]
        public void WriteToFileTest()
        {
            //***read file
            string path = Path.Combine(UserFolder, TestFiles);

            string name = "Test1";
            string test1FullPath = Path.Combine(path,@"Test1.csv");
            var tObj = new CSVTable<MockRowModel1>(name, test1FullPath);
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
            tObj.WriteToFile(Path.Combine(path, wfilename));

            //*** Unit Test
            var tObj2 = new CSVTable<MockRowModel1>(name, Path.Combine(path, wfilename));
            tObj2.LoadFromFile();
            Assert.AreEqual(2, tObj2.Count);
            Assert.AreEqual("Changed", tObj2.Row(0).Fields.Col3);
            Assert.AreEqual(9000, tObj2.Row(1).Fields.Col1);
        }
    }
}