using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using DASPM_PCTEL.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM_PCTELTests.Table.Mocks
{
    public class PCTEL_RowModelMock1 : PCTEL_TableRowModel
    {
        public int Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }

        [Ignore]
        public int TestProperty1 => 999;
    }

    public class PCTEL_RowModelMock1Map : PCTEL_TableRowMap<PCTEL_RowModelMock1>
    {
        public PCTEL_RowModelMock1Map()
        {
            Map(m => m.LocType).Index(0);
            Map(m => m.Floor).Index(1);
            Map(m => m.GridID).Index(2);
            Map(m => m.LocID).Index(3);
            Map(m => m.Label).Index(4);

            Map(m => m.Col1).Index(0).Name("Col1").Default(0);
            Map(m => m.Col2).Index(1).Name("Col2").Default("");
            Map(m => m.Col3).Index(2).Name("Col3").Default("");
        }
    }
}