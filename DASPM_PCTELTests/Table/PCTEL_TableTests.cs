using Microsoft.VisualStudio.TestTools.UnitTesting;
using DASPM_PCTEL.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DASPM_PCTELTests.Table.Mocks;

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
            public static PCTEL_Location Location2 => new PCTEL_Location("AREA", "Fine Arts - Admin 1", "1", "", "2");
            public static PCTEL_Location LocationNew => new PCTEL_Location("AREA", "Fine Arts - Admin 1", "1", "TestLocation", "999");

            public static string Name1 => "TableTest1";
            public static string Path1 => Path.Combine(UserFolder, TestFiles, "PCTEL_Table");
            public static string TestFiles { get => @"source\repos\DASPM\DASPM_PCTELTests\TestFiles"; }
            public static string UserFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }
        }

        //[TestMethod()]
        //public void AddRowGenericTest()
        //{
        //    var tObj = PCTEL_Table<PCTEL_TableRowModel>.CreateGeneric(TestSetup.Name1, TestSetup.FullPath1);

        //    var tModel = new PCTEL_TableRowModel();
        //    tModel.Label = "TestLabel";

        //    var tRow = tObj.AddRow(tModel);

        //    Assert.AreEqual("TestLabel", tRow.Fields.Label);
        //}

        [TestMethod()]
        public void AddLocationTest()
        {
            var table = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);
            table.LoadFromFile();
            var newLoc = TestSetup.LocationNew;
            table.AddLocation(newLoc);

            var locs = table.GetRowsByLocation(newLoc);
            Assert.AreEqual("TestLocation", locs[0].Fields.Label);
        }

        [TestMethod()]
        public void AddRowTest()
        {
            var tObj = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);

            var tRow = tObj.AddRow();

            Assert.AreEqual(typeof(PCTEL_TableRowMock1), tRow.GetType());
            Assert.AreEqual(null, tRow.Fields.Label);
        }

        //[TestMethod()]
        //public void CreateGenericTest()
        //{
        //    var tObj = PCTEL_Table<PCTEL_TableRowModel>.CreateGeneric(TestSetup.Name1, TestSetup.FullPath1);

        //    Assert.AreEqual(TestSetup.Name1, tObj.Name);
        //    Assert.AreEqual(TestSetup.FullPath1, tObj.FullPath);
        //}

        [TestMethod()]
        public void AddRowTest_Model()
        {
            var tObj = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);
            var tModel = new PCTEL_RowModelMock1();
            tModel.Label = "TestLabel";

            var tRow = tObj.AddRow(tModel);

            Assert.AreEqual(typeof(PCTEL_TableRowMock1), tRow.GetType());
            Assert.AreEqual("TestLabel", tRow.Fields.Label);
        }

        [TestMethod()]
        public void CalculateTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void ConfigureCsvReaderTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void ConfigureCsvWriterTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var table = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);

            Assert.AreEqual(TestSetup.Name1, table.Name);
            Assert.AreEqual(TestSetup.FullPath1, table.FullPath);

            Assert.AreEqual(typeof(List<PCTEL_TableRowMock1>), table.Rows.GetType());
            Assert.AreEqual(typeof(PCTEL_TableRowMock1), table.TableRowType);
            Assert.AreEqual(typeof(PCTEL_RowModelMock1), table.ModelType);
            Assert.AreEqual(typeof(PCTEL_RowModelMock1Map), table.ClassMap.GetType());
        }

        //[TestMethod()]
        //public void PCTEL_Table_CastFromGenericTest()
        //{
        //    var tObjGeneric = PCTEL_Table<PCTEL_TableRowModel>.CreateGeneric(TestSetup.Name1, TestSetup.FullPath1);

        //    PCTEL_Table tObj = tObjGeneric;

        //    Assert.AreEqual(TestSetup.Name1, tObj.Name);
        //    Assert.AreEqual(TestSetup.FullPath1, tObj.FullPath);
        //}

        [TestMethod()]
        public void GetModelByIDTest()
        {
            var table = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);
            table.LoadFromFile();

            Assert.AreEqual("TestLabel", table.GetModelByID(0).Label);
        }

        [TestMethod()]
        public void GetRowByIDTest()
        {
            var table = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);
            table.LoadFromFile();

            Assert.AreEqual("TestLabel", table.GetRowByID(0).Fields.Label);
        }

        [TestMethod()]
        public void GetRowsByLocationTest()
        {
            var table = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);
            table.LoadFromFile();
            var loc = TestSetup.Location2;

            var rows = table.GetRowsByLocation(loc);

            Assert.AreEqual("2", rows[0].Fields.LocID);
        }

        [TestMethod()]
        public void GetRowsByLocationTest_generic()
        {
            var table = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);
            table.LoadFromFile();
            var loc = TestSetup.Location2;

            var rows = table.GetRowsByLocation<PCTEL_TableRowMock1>(loc);

            Assert.AreEqual("2", rows[0].Fields.LocID);
            Assert.AreEqual("TestValue2", rows[0].Fields.Col3);
        }

        [TestMethod()]
        public void LoadFromFileTest()
        {
            var tObj = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);

            tObj.LoadFromFile();

            Assert.AreEqual(26, tObj.Count);
        }

        [TestMethod()]
        public void PCTEL_TableTest()
        {
            var tObj = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);

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
            Assert.AreEqual("TestLabel", testFields0.Label);
            //mock rows
            Assert.AreEqual(1, tObj.Row(0).Fields.Col1);
            Assert.AreEqual("", tObj.Row(0).Fields.Col2);
            Assert.AreEqual("TestValue", tObj.Row(0).Fields.Col3);
        }

        //[TestMethod()]
        //public void RowGenericTest()
        //{
        //    var tObj = PCTEL_Table<PCTEL_TableRowModel>.CreateGeneric(TestSetup.Name1, TestSetup.FullPath1);

        //    var tModel = new PCTEL_TableRowModel();
        //    tModel.Label = "TestLabel";

        //    var tRow1 = tObj.AddRow(tModel);
        //    var tRow2 = tObj.Row(tRow1.ID);
        //    Assert.AreEqual("TestLabel", tRow2.Fields.Label);
        //}

        [TestMethod()]
        public void RefreshLocationsTest()
        {
            var table = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);
            table.LoadFromFile();

            var loc = TestSetup.Location2;
            table.Locations.RemoveAt(1); //only valid for testing

            Assert.IsFalse(table.Locations.Contains(loc));

            table.RefreshLocations();

            //make sure the location came back...
            Assert.IsTrue(table.Locations.Contains(loc));
        }

        [TestMethod()]
        public void RemoveLocationTest()
        {
            var table = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);
            table.LoadFromFile();

            var loc = TestSetup.Location2;
            table.RemoveLocation(loc);

            Assert.AreEqual(0, table.GetRowsByLocation(loc).Count);
            Assert.AreNotEqual("TestValue2", table.Row(1).Fields.Col3);
        }

        [TestMethod()]
        public void RowTest()
        {
            var tObj = PCTEL_TableMock1.Create(TestSetup.Name1, TestSetup.FullPath1);
            tObj.LoadFromFile();

            Assert.AreEqual(typeof(PCTEL_TableRowMock1), tObj.Row(0).GetType());
            Assert.AreEqual(typeof(PCTEL_RowModelMock1), tObj.Row(0).Fields.GetType());
            Assert.AreEqual(1, tObj.Row(0).Fields.Col1);
        }
    }
}