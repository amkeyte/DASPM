using DASPMTests.Table.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DASPM.Table.Tests
{
    [TestClass()]
    public class CSVRowModelTests
    {
        [TestMethod()]
        public void GetFieldByIndexTest()
        {
            var classMap = new MockRowModel1Map();
            var model = new MockRowModel1();
            model.Col1 = 100;
            model.Col2 = "200";
            model.Col3 = "300";

            var test1Result = CSVRowModel.GetFieldByIndex(0, model, classMap);
            var test2Result = CSVRowModel.GetFieldByIndex(1, model, classMap);

            Assert.AreEqual(100, test1Result);
            Assert.AreEqual("200", test2Result);
        }

        [TestMethod()]
        public void GetFieldByModelPropertyNameTest()
        {
            var classMap = new MockRowModel1Map();
            var model = new MockRowModel1();
            model.Col1 = 100;
            model.Col2 = "200";
            model.Col3 = "300";

            var test1Result = CSVRowModel.GetFieldByModelPropertyName("Col1", model, classMap);
            var test2Result = CSVRowModel.GetFieldByModelPropertyName("Col2", model, classMap);

            Assert.AreEqual(100, test1Result);
            Assert.AreEqual("200", test2Result);
        }

        [TestMethod()]
        public void GetFieldByNameTest()
        {
            var classMap = new MockRowModel1Map();
            var model = new MockRowModel1();
            model.Col1 = 100;
            model.Col2 = "200";
            model.Col3 = "300";

            var test1Result = CSVRowModel.GetFieldByName("Col1", model, classMap);
            var test2Result = CSVRowModel.GetFieldByName("Col2", model, classMap);

            Assert.AreEqual(100, test1Result);
            Assert.AreEqual("200", test2Result);
        }

        [TestMethod()]
        public void ToDictTest()
        {
            var classMap = new MockRowModel1Map();
            var model = new MockRowModel1();
            model.Col1 = 100;
            model.Col2 = "200";
            model.Col3 = "300";

            var test1Result = CSVRowModel.ToDict(model, classMap);

            Assert.AreEqual(100, test1Result["Col1"]);
            Assert.AreEqual("200", test1Result["Col2"]);
        }

        [TestMethod()]
        public void ToListTest()
        {
            {
                var classMap = new MockRowModel1Map();
                var model = new MockRowModel1();
                model.Col1 = 100;
                model.Col2 = "200";
                model.Col3 = "300";

                var test1Result = CSVRowModel.ToList(model, classMap);

                Assert.AreEqual(100, test1Result[0]);
                Assert.AreEqual("200", test1Result[1]);
            }
        }
    }
}