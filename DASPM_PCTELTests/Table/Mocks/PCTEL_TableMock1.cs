using DASPM.Table;
using DASPM_PCTEL.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM_PCTELTests.Table.Mocks
{
    public class PCTEL_TableMock1 : PCTEL_Table
    {
        public new IList<PCTEL_TableRowMock1> Rows
        {
            get
            {
                return base.GetRows<PCTEL_TableRowMock1>();
            }
        }

        public static PCTEL_TableMock1 Create(string name, string fullPath)
        {
            return (PCTEL_TableMock1)CSVTableBuilder.CreateCSVTable(name, fullPath,
                typeof(PCTEL_TableMock1),
                typeof(PCTEL_TableRowMock1),
                typeof(PCTEL_RowModelMock1),
                typeof(PCTEL_RowModelMock1Map));
        }

        public new PCTEL_TableRowMock1 Row(int id)
        {
            return (PCTEL_TableRowMock1)base.Row(id);
        }
    }
}