using Microsoft.VisualStudio.TestTools.UnitTesting;
using DASPM_PCTEL.DataSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DASPM.Table;
using System.IO;
using DASPM_PCTEL.Table;

namespace DASPM_PCTEL.DataSet.Tests
{
    [TestClass()]
    public class PCTEL_DataSetTests
    {
        private class MySetup
        {
            #region general

            public static string Name = "DataSetTest1";
            public static string TestFilePath = Path.Combine(UserFolder, TestFilesFolder);
            public static string TestFilesFolder { get => @"source\repos\DASPM\DASPM_PCTELTests\TestFiles"; }
            public static string UserFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }

            #endregion general

            #region AreaTest

            public static string AreaFilename = @"MVHS_FAA_PRE_UHF_Fine Arts - Admin 1_AreaTestPoints.csv";
            public static PCTEL_DataSet AreaTable = PCTEL_DataSet.Create(Name, Path.Combine(TestFilePath, AreaFilename));
            public static PCTEL_Location AreaLoc2 => new PCTEL_Location("AREA", "Fine Arts - Admin 1", "1", null, "2");

            #endregion AreaTest

            #region CPTest

            public static string CPFilename = @"MVHS_FAA_PRE_UHF_Fine Arts - Admin 1_CriticalTestPoints.csv";
            public static PCTEL_DataSet CPTable = PCTEL_DataSet.Create(Name, Path.Combine(TestFilePath, CPFilename));
            public static PCTEL_Location CPLoc2 => new PCTEL_Location("CP", "Fine Arts - Admin 1", null, null, "2");

            #endregion CPTest
        }

        [TestMethod()]
        public void CreateTest()
        {
            var table = MySetup.AreaTable;

            //types
            Assert.AreEqual(typeof(PCTEL_DataSet), table.GetType());
            Assert.AreEqual(typeof(List<PCTEL_DataSetRow>), table.Rows.GetType());
            Assert.AreEqual(typeof(PCTEL_DataSetRow), table.TableRowType);
            Assert.AreEqual(typeof(PCTEL_DataSetRowModel), table.ModelType);
            Assert.AreEqual(typeof(PCTEL_DataSetModelAreaMap), table.ClassMap.GetType());

            //class vars
            Assert.AreEqual(PCTEL_DataSetTypes.PCTEL_DST_AREA, table.DataSetType);
        }

        [TestMethod()]
        public void LoadFromFileTest_Area()
        {
            var table = MySetup.AreaTable;
            table.LoadFromFile();

            //all rows
            Assert.AreEqual(20, table.Count);
            //location
            Assert.AreEqual("Fine Arts - Admin 1", table.Row(0).Fields.Floor);
            Assert.AreEqual("1", table.Row(0).Fields.GridID);
            Assert.AreEqual("1", table.Row(0).Fields.LocID);
            Assert.IsNull(table.Row(0).Fields.Label);

            //info and last row
            Assert.AreEqual("460137", table.Row(19).Fields.ChannelID);
            //nullable float
            Assert.IsNull(table.Row(0).Fields.DLDAQ);
            Assert.AreEqual(0f, table.Row(4).Fields.DLBER);
            Assert.AreEqual(22.5f, table.Row(5).Fields.DLBER);
            //nullable float "NT"
            Assert.AreEqual(float.MinValue, table.Row(0).Fields.DLPower);
        }

        [TestMethod()]
        public void LoadFromFileTest_CP()
        {
            var table = MySetup.CPTable;
            table.LoadFromFile();

            //all rows
            Assert.AreEqual(6, table.Count);
            //location
            Assert.AreEqual("Fine Arts - Admin 1", table.Row(0).Fields.Floor);
            Assert.IsNull(table.Row(0).Fields.GridID);
            Assert.AreEqual("1", table.Row(0).Fields.LocID);
            Assert.AreEqual("", table.Row(0).Fields.Label);

            //info and last row
            Assert.AreEqual("460137", table.Row(5).Fields.ChannelID);
            Assert.AreEqual("460137", table[5].ChannelID);
        }

        [TestMethod()]
        public void PCTEL_DataSetAccessorsTest()
        {
            var table = MySetup.AreaTable;
            table.LoadFromFile();

            //Get Row
            Assert.AreEqual("460137", table.Row(0).Fields.ChannelID);
            //Assert.AreEqual(1, table.GetRowByID(0).Fields.ChannelID);

            //get rows
            Assert.AreEqual("460137", table.GetRows<PCTEL_DataSetRow>()[0].Fields.ChannelID);
            Assert.AreEqual("460137", table.Rows[0].Fields.ChannelID);
            //Assert.AreEqual(2, table.GetRowsByLocation(MySetup.Location2)[0].Fields.ChannelID);

            //indexor
            Assert.AreEqual("1", table[0].LocID);
            var row = table[MySetup.AreaLoc2];
            Assert.AreEqual("460137", table[MySetup.AreaLoc2][0].Fields.ChannelID);
        }

        [TestMethod()]
        public void PCTEL_DataSetTest()
        {
            //see CreateTest
        }

        [TestMethod()]
        public void PCTEL_TableAccessorsTest()
        {
            var tobj = MySetup.AreaTable;
            var table = tobj as PCTEL_Table;
            table.LoadFromFile();

            //Get Row
            Assert.AreEqual(1, table.Row(0).Location.LocID);
            Assert.AreEqual(1, table.GetRowByID(0).Location.LocID);

            //get rows
            Assert.AreEqual(1, table.GetRows<PCTEL_TableRow>()[0].Location.LocID);
            Assert.AreEqual(1, table.Rows[0].Location.LocID);
            Assert.AreEqual(2, table.GetRowsByLocation(MySetup.AreaLoc2)[0].Location.LocID);

            //indexor
            Assert.AreEqual(1, table[0].LocID);
            Assert.AreEqual(2, table[MySetup.AreaLoc2][0].Location.LocID);
        }

        [TestMethod()]
        public void RowTest()
        {
            //see  PCTEL_DataSetAccessorsTest
        }

        [TestMethod()]
        public void WriteToFileTest_Area()
        {
            //***read file
            var table = MySetup.AreaTable;
            table.LoadFromFile();

            //***edit table
            //location
            table.Row(1).Fields.Floor = "New Floor";
            table.Row(1).Fields.GridID = "2";
            table.Row(1).Fields.LocID = "999";
            table.Row(1).Fields.Label = "oops!";

            //info
            table.Row(2).Fields.Comment = "Changed";

            //nullable float
            table.Row(3).Fields.DLPower = null;
            table.Row(3).Fields.ULBER = 25.3f;
            table.Row(3).Fields.ULDAQ = 0f;

            //nullable float "NT"
            table.Row(16).Fields.DLPower = float.MinValue;

            //***write file
            string wfilename = @"WriteToFileTest1 - AreaTestPoints.csv";
            var files = Directory.GetFiles(MySetup.TestFilePath, wfilename);
            foreach (string file in files)
            {
                File.Delete(file);
            }
            table.WriteNewFile(Path.Combine(MySetup.TestFilePath, wfilename));

            //*** Unit Test
            var tObj2 = PCTEL_DataSet.Create("writetestfile", Path.Combine(MySetup.TestFilePath, wfilename));
            tObj2.LoadFromFile();
            Assert.AreEqual(20, tObj2.Count);
            //location
            Assert.AreEqual("New Floor", tObj2.Row(1).Fields.Floor);
            Assert.AreEqual("2", tObj2.Row(1).Fields.GridID);
            Assert.AreEqual("999", tObj2.Row(1).Fields.LocID);
            Assert.IsNull(tObj2.Row(1).Fields.Label);
            //info
            Assert.AreEqual("Changed", tObj2.Row(2).Fields.Comment);
            //nullable float
            Assert.IsNull(tObj2.Row(3).Fields.DLPower);
            Assert.AreEqual(25.3f, tObj2.Row(3).Fields.ULBER);
            Assert.AreEqual(0f, tObj2.Row(3).Fields.ULDAQ);
            //nullable float "NT"
            Assert.AreEqual(float.MinValue, tObj2.Row(16).Fields.DLPower);
        }

        [TestMethod()]
        public void WriteToFileTest_CP()
        {
            //***read file
            var table = MySetup.CPTable;
            table.LoadFromFile();

            //***edit table
            //location
            table.Row(1).Fields.Floor = "New Floor";
            table.Row(1).Fields.GridID = "oops!";
            table.Row(1).Fields.Label = "test label";
            table.Row(1).Fields.LocID = "777";

            //info
            table.Row(0).Fields.Comment = "Changed";
            table.Row(5).Fields.DLPower = 777f;

            //***write file
            string wfilename = @"WriteToFileTest1 - CriticalTestPoints.csv";
            var files = Directory.GetFiles(MySetup.TestFilePath, wfilename);
            foreach (string file in files)
            {
                File.Delete(file);
            }
            table.WriteNewFile(Path.Combine(MySetup.TestFilePath, wfilename));

            //*** Unit Test
            var table2 = PCTEL_DataSet.Create("testCPFile", Path.Combine(MySetup.TestFilePath, wfilename));
            table2.LoadFromFile();

            Assert.AreEqual(6, table2.Count);
            //location
            Assert.AreEqual("New Floor", table2.Row(1).Fields.Floor);
            Assert.IsNull(table2.Row(1).Fields.GridID);
            Assert.AreEqual("777", table2.Row(1).Fields.LocID);
            Assert.AreEqual("test label", table2.Row(1).Fields.Label);
            //info
            Assert.AreEqual("Changed", table2.Row(0).Fields.Comment);
            Assert.AreEqual(777f, table2.Row(5).Fields.DLPower);
        }
    }
}