using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM.Table
{
    public interface ITable<T> where T : IRowModel
    {
        long Count { get; }
        string Filename { get; }
        IList<string> Header { get; }
        string Name { get; }
        string FilePath { get; }
        IList<ITableRow<T>> Rows { get; }

        //ITableRow<T> AddRow(ITableRow<T> row);

        void Refresh();

        ITableRow<T> Row(int id);
    }
}