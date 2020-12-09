using Microsoft.VisualStudio.TestTools.UnitTesting;
using DASPM_PCTEL.DataSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DASPM.Table;
using System.IO;

namespace DASPM_PCTEL.DataSet.Tests
{
    [TestClass()]
    public class PCTEL_DataSetTests
    {
        private class MySetup
        {
            #region general

            public static string FullPath = Path.Combine(UserFolder, TestFilesFolder);
            public static string Name = "DataSetTest1";
            public static string TestFilesFolder { get => @"source\repos\DASPM\DASPM_PCTELTests\TestFiles"; }
            public static string UserFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }

            #endregion general

            #region AreaTest

            public static string AreaFilename = @"MVHS_FAA_PRE_UHF_Fine Arts - Admin 1_AreaTestPoints.csv";
            public static PCTEL_DataSet AreaTable = PCTEL_DataSet.Create(Name, Path.Combine(FullPath, AreaFilename));

            #endregion AreaTest

            #region CPTest

            public static string CPFilename = @"MVHS_FAA_PRE_UHF_Fine Arts - Admin 1_CriticalTestPoints.csv";
            public static PCTEL_DataSet CPTable = PCTEL_DataSet.Create(Name, Path.Combine(FullPath, CPFilename));

            #endregion CPTest
        }

        [TestMethod()]
        public void CreateTest()
        {
            var table = MySetup.AreaTable;

            //types
            Assert.AreEqual(typeof(PCTEL_DataSet), table);
            Assert.AreEqual(typeof(List<PCTEL_DataSetRow>), table.Rows.GetType());
            Assert.AreEqual(typeof(PCTEL_DataSetRow), table.TableRowType);
            Assert.AreEqual(typeof(PCTEL_DataSetRowModel), table.ModelType);
            //class vars
            Assert.AreEqual(PCTEL_DataSet.PCTEL_DATASET_TYPE_NAME_AREA, table.DataSetType);
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
        public void PCTEL_DataSetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RowTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WriteToFileTest_Area()
        {
            //***read file
            string name = "DataSetTest1";
            string path = Path.Combine(UserFolder, TestFiles);
            string testFileFullPath = Path.Combine(path, @"MVHS_FAA_PRE_UHF_Fine Arts - Admin 1_AreaTestPoints.csv");
            var tObj = PCTEL_DataSet.Create(name, Path.Combine(path, testFileFullPath));

            tObj.LoadFromFile();

            //***edit table
            //location
            tObj.Row(1).Fields.Floor = "New Floor";
            tObj.Row(1).Fields.GridID = "2";
            tObj.Row(1).Fields.LocID = "999";
            tObj.Row(1).Fields.Label = "oops!";

            //info
            tObj.Row(2).Fields.Comment = "Changed";

            //nullable float
            tObj.Row(3).Fields.DLPower = null;
            tObj.Row(3).Fields.ULBER = 25.3f;
            tObj.Row(3).Fields.ULDAQ = 0f;

            //nullable float "NT"
            tObj.Row(16).Fields.DLPower = float.MinValue;

            //***write file
            string wfilename = @"WriteToFileTest1 - AreaTestPoints.csv";
            var files = Directory.GetFiles(path, wfilename);
            foreach (string file in files)
            {
                File.Delete(file);
            }
            tObj.WriteToFile(Path.Combine(path, wfilename));

            //*** Unit Test
            var tObj2 = PCTEL_DataSet.Create(name, Path.Combine(path, wfilename));
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
            string name = "DataSetTest1";
            string path = Path.Combine(UserFolder, TestFiles);
            string filename = @"MVHS_FAA_PRE_UHF_Fine Arts - Admin 1_CriticalTestPoints.csv";
            var tObj = PCTEL_DataSet.Create(name, Path.Combine(path, filename));

            tObj.LoadFromFile();

            //***edit table
            //location
            tObj.Row(1).Fields.Floor = "New Floor";
            tObj.Row(1).Fields.GridID = "oops!";
            tObj.Row(1).Fields.Label = "test label";
            tObj.Row(1).Fields.LocID = "777";

            //info
            tObj.Row(0).Fields.Comment = "Changed";
            tObj.Row(5).Fields.DLPower = 777f;

            //***write file
            string wfilename = @"WriteToFileTest1 - CriticalTestPoints.csv";
            var files = Directory.GetFiles(path, wfilename);
            foreach (string file in files)
            {
                File.Delete(file);
            }
            tObj.WriteToFile(Path.Combine(path, wfilename));

            //*** Unit Test
            var tObj2 = PCTEL_DataSet.Create(name, Path.Combine(path, wfilename));
            tObj2.LoadFromFile();

            Assert.AreEqual(6, tObj2.Count);
            //location
            Assert.AreEqual("New Floor", tObj2.Row(1).Fields.Floor);
            Assert.IsNull(tObj2.Row(1).Fields.GridID);
            Assert.AreEqual("777", tObj2.Row(1).Fields.LocID);
            Assert.AreEqual("test label", tObj2.Row(1).Fields.Label);
            //info
            Assert.AreEqual("Changed", tObj2.Row(0).Fields.Comment);
            Assert.AreEqual(777f, tObj2.Row(5).Fields.DLPower);
        }
    }
}