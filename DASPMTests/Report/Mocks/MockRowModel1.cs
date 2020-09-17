using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using DASPM.Table;

namespace DASPMTests.Report.Mocks
{
    public class MockRowModel1 : CSVRowModel
    {
        public int Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }

        [Ignore]
        public string ModelName => "Mock1";
    }
}