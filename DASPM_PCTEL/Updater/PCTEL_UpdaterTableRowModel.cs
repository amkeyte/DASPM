using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DASPM.Table;
using DASPM_PCTEL.Table;

namespace DASPM_PCTEL.Updater
{
    public class PCTEL_UpdaterTableRowMap : PCTEL_TableRowMap<PCTEL_UpdaterTableRowModel>
    {
        public PCTEL_UpdaterTableRowMap()
        {
            Map(m => m.Comment).Index(5);
        }
    }

    public class PCTEL_UpdaterTableRowModel : PCTEL_TableRowModel

    {
        public string Comment { get; set; }
    }
}