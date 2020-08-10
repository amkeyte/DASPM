using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DASPM.Table;
using DASPM_PCTEL.Table;

namespace DASPM_PCTEL.DataSet
{
    public class PCTEL_DataSetRow<T> : CSVTableRow<T>
        where T : PCTEL_DataSetRowModel
    {
        #region ctor

        public PCTEL_DataSetRow(PCTEL_DataSet<T> table, T row) : this((CSVTable<T>)table, row)
        {
        }

        private PCTEL_DataSetRow(CSVTable<T> table, T row) : base(table, row)
        {
        }

        #endregion ctor

        #region ClassMembers

        public PCTEL_DataSetTypes DataSetType { get => this.Table.DataSetType; }

        public PCTEL_Location Location
        {
            get
            {
                return new PCTEL_Location(Fields);
            }
        }

        public new PCTEL_DataSet<T> Table { get; protected set; }

        #endregion ClassMembers
    }
}