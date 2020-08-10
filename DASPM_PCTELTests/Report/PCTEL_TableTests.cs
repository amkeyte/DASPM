using Microsoft.VisualStudio.TestTools.UnitTesting;
using DASPM_PCTEL.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM_PCTEL.Table.Tests
{
    [TestClass()]
    public class PCTEL_TableTests
    {
        [TestMethod()]
        public void LocationsTest()
        {
            string name = "TableTest1";
            string path = @"C:\Users\Aaron\source\repos\DASPM\DASPM_PCTELTests\TestFiles\PCTEL_Table\";
            string filename = @"TableTest1.csv";
            var tObj = PCTEL_Table.Create(name, path, filename);

            tObj.LoadFromFile();
            var listRow = tObj.Row(0);
            var loc = listRow.Location;
            var dictRow = tObj.Locations[loc];

            //location fields
            Assert.AreEqual("AREA", dictRow.Location.LocType);
            Assert.AreEqual("Fine Arts - Admin 1", dictRow.Location.Floor);
            Assert.AreEqual("1", dictRow.Location.GridID);
            Assert.AreEqual("1", dictRow.Location.LocID);
            Assert.AreEqual("", dictRow.Location.Label);
            Assert.AreEqual("AREAFine Arts - Admin 111", dictRow.Location.Key);

            //model fields access
            Assert.AreEqual("Fine Arts - Admin 1", dictRow.Fields.Floor);
            Assert.AreEqual("1", tObj[loc].GridID);
        }

        [TestMethod()]
        public void PCTEL_TableTest()
        {
            string name = "TableTest1";
            string path = @"C:\Users\Aaron\source\repos\DASPM\DASPM_PCTELTests\TestFiles\PCTEL_Table\";
            string filename = @"TableTest1.csv";
            var tObj = PCTEL_Table.Create(name, path, filename);

            tObj.LoadFromFile();

            //all rows
            Assert.AreEqual(26, tObj.Count);
            //location
            Assert.AreEqual("Fine Arts - Admin 1", tObj.Row(0).Fields.Floor);
            Assert.AreEqual("1", tObj.Row(0).Fields.GridID);
            Assert.AreEqual("1", tObj.Row(0).Fields.LocID);
            Assert.AreEqual("", tObj.Row(0).Fields.Label);

            //info and last row
            Assert.AreEqual("6", tObj[25].LocID);
        }
    }
}