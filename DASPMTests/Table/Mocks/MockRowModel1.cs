using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using DASPM.Table;

namespace DASPMTests.Table.Mocks
{
    public class MockRowModel1 : IRowModel
    {
        public int Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }

        [Ignore]
        public string ModelName => "Mock1";
    }

    public sealed class MockRowModel1Map : ClassMap<MockRowModel1>
    {
        public MockRowModel1Map()
        {
            Map(m => m.Col1).Index(0).Name("Col1");
            Map(m => m.Col2).Index(1).Name("Col2");
            Map(m => m.Col3).Index(2).Name("Col3");
        }
    }
}