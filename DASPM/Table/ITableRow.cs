using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    public interface ITableRow<T> where T : IRowModel
    {
        T Fields { get; }
        long ID { get; }
        ITable<T> Table { get; }
    }
}