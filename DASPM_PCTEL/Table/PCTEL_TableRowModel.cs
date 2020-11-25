using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using DASPM.Table;
using DASPM_PCTEL.DataSet;

namespace DASPM_PCTEL.Table
{
    public class PCTEL_TableRowMap<TModel> : ClassMap<TModel>
        where TModel : PCTEL_TableRowModel
    {
        public PCTEL_TableRowMap()
        {
            Map(m => m.LocType).Index(0);
            Map(m => m.Floor).Index(1);
            Map(m => m.GridID).Index(2);
            Map(m => m.LocID).Index(3);
            Map(m => m.Label).Index(4);
        }
    }

    public class PCTEL_TableRowMap : ClassMap<PCTEL_TableRowModel>
    {
        public PCTEL_TableRowMap()
        {
            Map(m => m.LocType).Index(0);
            Map(m => m.Floor).Index(1);
            Map(m => m.GridID).Index(2);
            Map(m => m.LocID).Index(3);
            Map(m => m.Label).Index(4);
        }
    }

    public class PCTEL_TableRowModel : IRowModel
    {
        public string Floor { get; set; }
        public string GridID { get; set; }
        public string Label { get; set; }
        public string LocID { get; set; }
        public string LocType { get; set; }
    }
}