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
        #region Accessors

        public new IList<PCTEL_TableRowMock1> Rows => base.GetRows<PCTEL_TableRowMock1>();
        public new PCTEL_RowModelMock1 this[int index] => Rows[index].Fields;
        public new IList<PCTEL_TableRowMock1> this[PCTEL_Location loc] => GetRowsByLocation<PCTEL_TableRowMock1>(loc);

        public new PCTEL_TableRowMock1 Row(int id) => (PCTEL_TableRowMock1)base.Row(id);

        #endregion Accessors

        public static PCTEL_TableMock1 Create(string name, string fullPath)
        {
            return (PCTEL_TableMock1)CSVTableBuilder.CreateCSVTable(name, fullPath,
                typeof(PCTEL_TableMock1),
                typeof(PCTEL_TableRowMock1),
                typeof(PCTEL_RowModelMock1),
                typeof(PCTEL_RowModelMock1Map));
        }
    }
}