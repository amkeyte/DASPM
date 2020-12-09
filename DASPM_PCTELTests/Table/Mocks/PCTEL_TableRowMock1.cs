using DASPM.Table;
using DASPM_PCTEL.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM_PCTELTests.Table.Mocks
{
    public class PCTEL_TableRowMock1 : PCTEL_TableRow
    {
        public new PCTEL_RowModelMock1 Fields { get => (PCTEL_RowModelMock1)base.Fields; }

        public static PCTEL_TableRowMock1 Create(PCTEL_TableMock1 table, PCTEL_RowModelMock1 model)
        {
            return (PCTEL_TableRowMock1)CSVTableRowBuilder.Create(
                table, model,
                typeof(PCTEL_TableRowMock1));
        }
    }
}