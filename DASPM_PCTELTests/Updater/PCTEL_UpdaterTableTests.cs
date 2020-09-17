using Microsoft.VisualStudio.TestTools.UnitTesting;
using DASPM_PCTEL.Updater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DASPM_PCTEL.DataSet;

namespace DASPM_PCTEL.Updater.Tests
{
    [TestClass()]
    public class PCTEL_UpdaterTableTests
    {
        private string TestFiles { get => @"source\repos\DASPM\DASPM_PCTELTests\TestFiles"; }
        private string UserFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }

        [TestMethod()]
        public void PCTEL_UpdaterTableTest()
        {
            string name = "TableTest1";
            string fullPath = Path.Combine(UserFolder, TestFiles, "PCTEL_Updater", @"UpdaterTest1.csv");
            var tObj = PCTEL_UpdaterTable.Create(name, fullPath);

            tObj.LoadFromFile();

            //all rows
            Assert.AreEqual(26, tObj.Count);
            //location
            Assert.AreEqual("Fine Arts - Admin 1", tObj.Row(0).Fields.Floor);
            Assert.AreEqual("1", tObj.Row(0).Fields.GridID);
            Assert.AreEqual("1", tObj.Row(0).Fields.LocID);
            Assert.AreEqual("", tObj.Row(0).Fields.Label);

            //info and last row
            Assert.AreEqual("Test", tObj[0].Comment);
            Assert.AreEqual("6", tObj[25].LocID);
            Assert.AreEqual("AnotherTest", tObj[25].Comment);
        }

        [TestMethod()]
        public void PCTEL_UpdaterUpdateTest()
        {
            string name = @"TableTest1";
            string path = Path.Combine(UserFolder, TestFiles, "PCTEL_Updater");
            string filename = "UpdaterTest2Source.csv";
            var tObj = PCTEL_UpdaterTable.Create(name, Path.Combine(path, filename));
            tObj.LoadFromFile();
            tObj.DataSetUpdater.UpdaterRules = new PCTEL_UpdaterRules<PCTEL_UpdaterTableRowModel>();

            //*** setup
            //reset the test files
            string sourcePath = Path.Combine(path, @"DataSetsSource");
            string targetPath = Path.Combine(path, @"DataSetsResult");

            var files = Directory.GetFiles(sourcePath);

            foreach (string fSource in files)
            {
                string fTarget = fSource.Substring(sourcePath.Length + 1);
                File.Copy(Path.Combine(sourcePath, fTarget), Path.Combine(targetPath, fTarget), true);
            }

            string wFilename = "UpdaterTest2.csv";
            File.Delete(Path.Combine(path, wFilename));

            //*** edit table
            tObj[0].Comment = "Area Comment Added";
            tObj[20].Comment = "CP Comment Added";
            //*** write edited table to file
            tObj.WriteToFile(path, wFilename);

            //*** update target dataset
            tObj.DataSetPath = targetPath;
            tObj.DataSetUpdater.Update();

            //*** Test

            //read in written files
            var DSList = new List<PCTEL_DataSet<PCTEL_DataSetRowModel>>();
            files = Directory.GetFiles(targetPath);
            foreach (string f in files)
            {
                string fTarget = Path.GetFileNameWithoutExtension(f);
                DSList.Add(PCTEL_DataSet.Create(fTarget, f));
                DSList[DSList.Count - 1].LoadFromFile();
            }

            Assert.AreEqual("MVHS_FAA_PRE_UHF_Fine Arts - Admin 1_AreaTestPoints.csv", DSList[0].Filename);
            Assert.AreEqual(tObj[0].Floor, DSList[0][0].Floor);
            Assert.AreEqual(tObj[0].Comment, DSList[0][0].Comment);
            Assert.AreEqual("MVHS_FAA_PRE_UHF_Fine Arts - Admin 1_CriticalTestPoints.csv", DSList[1].Filename);
            Assert.AreEqual(tObj[20].Floor, DSList[1][0].Floor);
            Assert.AreEqual("Main Entry", DSList[1][0].Comment);
        }
    }
}