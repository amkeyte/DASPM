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
        private string TestFiles { get => @"source\repos\DASPM\DASPM_PCTELTests\TestFiles"; }
        private string UserFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }

        [TestMethod()]
        public void LoadFromFileTest_Area()
        {
            string name = "DataSetTest1";
            string path = Path.Combine(UserFolder, TestFiles);
            string filename = @"MVHS_FAA_PRE_UHF_Fine Arts - Admin 1_AreaTestPoints.csv";
            var tObj = PCTEL_DataSet.Create(name, Path.Combine(path, filename));

            tObj.LoadFromFile();

            //all rows
            Assert.AreEqual(20, tObj.Count);
            //location
            Assert.AreEqual("Fine Arts - Admin 1", tObj.Row(0).Fields.Floor);
            Assert.AreEqual("1", tObj.Row(0).Fields.GridID);
            Assert.AreEqual("1", tObj.Row(0).Fields.LocID);
            Assert.IsNull(tObj.Row(0).Fields.Label);

            //info and last row
            Assert.AreEqual("460137", tObj.Row(19).Fields.ChannelID);
            //nullable float
            Assert.IsNull(tObj.Row(0).Fields.DLDAQ);
            Assert.AreEqual(0f, tObj.Row(4).Fields.DLBER);
            Assert.AreEqual(22.5f, tObj.Row(5).Fields.DLBER);
            //nullable float "NT"
            Assert.AreEqual(float.MinValue, tObj.Row(0).Fields.DLPower);
        }

        [TestMethod()]
        public void LoadFromFileTest_CP()
        {
            string name = "DataSetTest1";
            string path = Path.Combine(UserFolder, TestFiles);
            string filename = @"MVHS_FAA_PRE_UHF_Fine Arts - Admin 1_CriticalTestPoints.csv";
            var tObj = PCTEL_DataSet.Create(name, Path.Combine(path, filename));

            tObj.LoadFromFile();

            //all rows
            Assert.AreEqual(6, tObj.Count);
            //location
            Assert.AreEqual("Fine Arts - Admin 1", tObj.Row(0).Fields.Floor);
            Assert.IsNull(tObj.Row(0).Fields.GridID);
            Assert.AreEqual("1", tObj.Row(0).Fields.LocID);
            Assert.AreEqual("", tObj.Row(0).Fields.Label);

            //info and last row
            Assert.AreEqual("460137", tObj.Row(5).Fields.ChannelID);
            Assert.AreEqual("460137", tObj[5].ChannelID);
        }

        [TestMethod()]
        public void WriteToFileTest_Area()
        {
            //***read file
            string name = "DataSetTest1";
            string path = Path.Combine(UserFolder, TestFiles);
            string filename = @"MVHS_FAA_PRE_UHF_Fine Arts - Admin 1_AreaTestPoints.csv";
            var tObj = PCTEL_DataSet.Create(name, Path.Combine(path, filename));

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
            tObj.WriteToFile(path, wfilename);

            //*** Unit Test
            var tObj2 = PCTEL_DataSet.Create(name, Path.Combine(path, filename));
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
            tObj.WriteToFile(path, wfilename);

            //*** Unit Test
            var tObj2 = PCTEL_DataSet.Create(name, Path.Combine(path, filename));
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