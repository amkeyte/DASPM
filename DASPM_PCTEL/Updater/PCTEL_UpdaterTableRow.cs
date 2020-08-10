using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DASPM.Table;
using DASPM_PCTEL.Table;

namespace DASPM_PCTEL.Updater
{
    public class PCTEL_UpdaterTableRow<TModel> : PCTEL_TableRow<TModel>
        where TModel : PCTEL_UpdaterTableRowModel
    {
        public PCTEL_UpdaterTableRow(PCTEL_UpdaterTable<TModel> table, TModel row) : this((CSVTable<TModel>)table, row)
        {
        }

        private PCTEL_UpdaterTableRow(CSVTable<TModel> table, TModel row) : base(table, row)
        {
        }
    }
}