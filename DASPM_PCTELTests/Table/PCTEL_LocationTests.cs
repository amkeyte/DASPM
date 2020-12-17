using Microsoft.VisualStudio.TestTools.UnitTesting;
using DASPM_PCTEL.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DASPM_PCTELTests.Table.Mocks;

namespace DASPM_PCTEL.Table.Tests
{
    [TestClass()]
    public class PCTEL_LocationTests
    {
        [TestMethod()]
        public void ApplyLocationTest()
        {
            var model = new PCTEL_RowModelMock1();
            model.Col1 = 1;
            model.Col2 = "2";
            model.Col3 = "3";
            var loc1 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 998);

            PCTEL_Location.ApplyLocation(model, loc1);

            Assert.AreEqual("AREA", model.LocType);
            Assert.AreEqual("TestFloor", model.Floor);
            Assert.AreEqual(999, model.GridID);
            Assert.AreEqual("TestLabel", model.Label);
            Assert.AreEqual(998, model.LocID);
        }

        [TestMethod()]
        public void CompareToTest()
        {
            var loc1 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 998);
            var loc2 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 998);
            var loc3 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 997);
            var loc4 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 999);

            Assert.AreEqual(0, loc2.CompareTo(loc1));
            Assert.AreEqual(-1, loc3.CompareTo(loc1));
            Assert.AreEqual(1, loc4.CompareTo(loc1));
        }

        [TestMethod()]
        public void CompareToTest_obj()
        {
            object loc1 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 998);
            var loc2 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 998);
            var loc3 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 997);
            var loc4 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 999);

            Assert.AreEqual(0, loc2.CompareTo(loc1));
            Assert.AreEqual(-1, loc3.CompareTo(loc1));
            Assert.AreEqual(1, loc4.CompareTo(loc1));
        }

        [TestMethod()]
        public void EqualsTest()
        {
            var loc1 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 998);
            var loc2 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 998);

            Assert.IsTrue(loc1.Equals(loc2));
            Assert.IsTrue(loc1 == loc2);
        }

        [TestMethod()]
        public void EqualsTest_obj()
        {
            var loc1 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 998);
            object loc2 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 998);

            Assert.IsTrue(loc1.Equals(loc2));
            Assert.IsTrue(loc1 == loc2);
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            var loc1 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 998);
            var loc2 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 998);
            var loc3 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 997);

            Assert.AreEqual(loc1 == loc2, loc1.GetHashCode() == loc2.GetHashCode());
            Assert.AreEqual(loc1 == loc3, loc1.GetHashCode() == loc3.GetHashCode());
        }

        [TestMethod()]
        public void PCTEL_LocationTest()
        {
            var loc1 = new PCTEL_Location("AREA", "TestFloor", 999, "TestLabel", 998);

            Assert.AreEqual("AREA", loc1.LocType);
            Assert.AreEqual("TestFloor", loc1.Floor);
            Assert.AreEqual(999, loc1.GridID);
            Assert.AreEqual("TestLabel", loc1.Label);
            Assert.AreEqual(998, loc1.LocID);
        }

        [TestMethod()]
        public void PCTEL_LocationTest1()
        {
            var model = new PCTEL_RowModelMock1();
            model.Col1 = 1;
            model.Col2 = "2";
            model.Col3 = "3";
            model.LocType = "AREA";
            model.Floor = "TestFloor";
            model.GridID = 999;
            model.Label = "TestLabel";
            model.LocID = 998;

            var loc1 = new PCTEL_Location(model);

            Assert.AreEqual("AREA", loc1.LocType);
            Assert.AreEqual("TestFloor", loc1.Floor);
            Assert.AreEqual(999, loc1.GridID);
            Assert.AreEqual("TestLabel", loc1.Label);
            Assert.AreEqual(998, loc1.LocID);
        }
    }
}