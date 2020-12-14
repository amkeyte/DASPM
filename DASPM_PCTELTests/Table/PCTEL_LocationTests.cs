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
    public class PCTEL_LocationTests
    {
        [TestMethod()]
        public void ApplyLocationTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CompareToTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CompareToTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EqualsTest()
        {
            var loc1 = new PCTEL_Location("AREA", "TestFloor", "999", "TestLabel", "998");
            var loc2 = new PCTEL_Location("AREA", "TestFloor", "999", "TestLabel", "998");

            Assert.IsTrue(loc1.Equals(loc2));
            Assert.IsTrue(loc1 == loc2);
        }

        [TestMethod()]
        public void EqualsTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PCTEL_LocationTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PCTEL_LocationTest1()
        {
            Assert.Fail();
        }
    }
}