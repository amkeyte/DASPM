﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using DASPM_PCTEL.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DASPM_PCTEL.Table.Tests
{
    [TestClass()]
    public class PCTEL_TableTests
    {
        //[TestMethod()]
        //public void LocationsTest()
        //{
        //    string name = "TableTest1";
        //    string path = Path.Combine(UserFolder, TestFiles, "PCTEL_Table");
        //    string filename = @"TableTest1.csv";
        //    var tObj = PCTEL_Table.Create(name, Path.Combine(path, filename));

        //    tObj.LoadFromFile();
        //    var listRow = tObj.Row(0);
        //    var loc = listRow.Location;
        //    var dictRow = tObj.Locations[loc];

        //    //location fields
        //    Assert.AreEqual("AREA", dictRow.Location.LocType);
        //    Assert.AreEqual("Fine Arts - Admin 1", dictRow.Location.Floor);
        //    Assert.AreEqual("1", dictRow.Location.GridID);
        //    Assert.AreEqual("1", dictRow.Location.LocID);
        //    Assert.AreEqual("", dictRow.Location.Label);
        //    Assert.AreEqual("AREAFine Arts - Admin 111", dictRow.Location.Key);

        //    //model fields access
        //    Assert.AreEqual("Fine Arts - Admin 1", dictRow.Fields.Floor);
        //    Assert.AreEqual("1", tObj[loc].GridID);
        //}

        private class TestSetup
        {
            public static string Filename1 => @"TableTest1.csv";
            public static string FullPath1 => Path.Combine(Path1, Filename1);
            public static string Name1 => "TableTest1";
            public static string Path1 => Path.Combine(UserFolder, TestFiles, "PCTEL_Table");
            public static string TestFiles { get => @"source\repos\DASPM\DASPM_PCTELTests\TestFiles"; }
            public static string UserFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }
        }

        [TestMethod()]
        public void AddRowGenericTest()
        {
            var tObj = PCTEL_Table<PCTEL_TableRowModel>.CreateGeneric(TestSetup.Name1, TestSetup.FullPath1);

            var tModel = new PCTEL_TableRowModel();
            tModel.Label = "TestLabel";

            var tRow = tObj.AddRow(tModel);

            Assert.AreEqual("TestLabel", tRow.Fields.Label);
        }

        [TestMethod()]
        public void AddRowTest()
        {
            var tObj = PCTEL_Table.Create(TestSetup.Name1, TestSetup.FullPath1);
            var tModel = new PCTEL_TableRowModel();
            tModel.Label = "TestLabel";

            var tRow = tObj.AddRow(tModel);

            Assert.AreEqual("TestLabel", tRow.Fields.Label);
        }

        [TestMethod()]
        public void CreateGenericTest()
        {
            var tObj = PCTEL_Table<PCTEL_TableRowModel>.CreateGeneric(TestSetup.Name1, TestSetup.FullPath1);

            Assert.AreEqual(TestSetup.Name1, tObj.Name);
            Assert.AreEqual(TestSetup.FullPath1, tObj.FullPath);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var tObj = PCTEL_Table.Create(TestSetup.Name1, TestSetup.FullPath1);

            Assert.AreEqual(TestSetup.Name1, tObj.Name);
            Assert.AreEqual(TestSetup.FullPath1, tObj.FullPath);
        }

        [TestMethod()]
        public void PCTEL_Table_CastFromGenericTest()
        {
            var tObjGeneric = PCTEL_Table<PCTEL_TableRowModel>.CreateGeneric(TestSetup.Name1, TestSetup.FullPath1);

            PCTEL_Table tObj = tObjGeneric;

            Assert.AreEqual(TestSetup.Name1, tObj.Name);
            Assert.AreEqual(TestSetup.FullPath1, tObj.FullPath);
        }

        [TestMethod()]
        public void PCTEL_TableTest()
        {
            var tObj = PCTEL_Table.Create(TestSetup.Name1, TestSetup.FullPath1);

            tObj.LoadFromFile();

            //keep these to verify casting
            var testRow0 = tObj.Row(0);
            var testFields0 = testRow0.Fields;

            //all rows
            Assert.AreEqual(26, tObj.Count);
            //location
            Assert.AreEqual("Fine Arts - Admin 1", testFields0.Floor);
            Assert.AreEqual("1", testFields0.GridID);
            Assert.AreEqual("1", testFields0.LocID);
            Assert.AreEqual("", testFields0.Label);

            //info and last row
            //Assert.AreEqual("6", tObj[25]);
        }

        [TestMethod()]
        public void RowGenericTest()
        {
            var tObj = PCTEL_Table<PCTEL_TableRowModel>.CreateGeneric(TestSetup.Name1, TestSetup.FullPath1);

            var tModel = new PCTEL_TableRowModel();
            tModel.Label = "TestLabel";

            var tRow1 = tObj.AddRow(tModel);
            var tRow2 = tObj.Row(tRow1.ID);
            Assert.AreEqual("TestLabel", tRow2.Fields.Label);
        }

        [TestMethod()]
        public void RowTest()
        {
            var tObj = PCTEL_Table.Create(TestSetup.Name1, TestSetup.FullPath1);
            var tModel = new PCTEL_TableRowModel();
            tModel.Label = "TestLabel";

            var tRow1 = tObj.AddRow(tModel);
            var tRow2 = tObj.Row(tRow1.ID);
            Assert.AreEqual("TestLabel", tRow2.Fields.Label);
        }
    }
}