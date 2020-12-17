﻿using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using DASPM.Table;

#if BUILD_DATASET
using DASPM_PCTEL.DataSet;
#endif

namespace DASPM_PCTEL.Table
{
    public abstract class PCTEL_TableRowMap<TModel> : ClassMap<TModel>
        where TModel : PCTEL_TableRowModel
    {
        public PCTEL_TableRowMap()
        {
            //Map(m => m.LocType).Index(0);
            //Map(m => m.Floor).Index(1);
            //Map(m => m.GridID).Index(2);
            //Map(m => m.LocID).Index(3);
            //Map(m => m.Label).Index(4);
        }
    }

    //public abstract class PCTEL_TableRowMap : ClassMap<PCTEL_TableRowModel>
    //{
    //    public PCTEL_TableRowMap()
    //    {
    //        Map(m => m.LocType).Index(0);
    //        Map(m => m.Floor).Index(1);
    //        Map(m => m.GridID).Index(2);
    //        Map(m => m.LocID).Index(3);
    //        Map(m => m.Label).Index(4);
    //    }
    //}

    public abstract class PCTEL_TableRowModel : IRowModel
    {
        public string Floor { get; set; }
        public int? GridID { get; set; }
        public string Label { get; set; }
        public int LocID { get; set; }
        public string LocType { get; set; }
    }
}