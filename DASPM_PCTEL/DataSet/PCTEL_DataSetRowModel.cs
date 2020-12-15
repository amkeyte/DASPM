using CsvHelper.Configuration.Attributes;
using DASPM_PCTEL.Table;
using System;

namespace DASPM_PCTEL.DataSet
{
    public class PCTEL_DataSetModelAreaMap : PCTEL_DataSetRowMap
    {
        public PCTEL_DataSetModelAreaMap()
        {
            DataSetVariant = new PCTEL_DataSetVariant(PCTEL_DataSetVariantIDs.PCTEL_DST_AREA);
            //location
            Map(m => m.GridID).Index(1).Name("Grid Id");
            Map(m => m.LocID).Index(2).Name("Area #");
            Map(m => m.Label).Ignore();

            //type specific
            Map(m => m.SelReference).Ignore();
            Map(m => m.Comment).Index(20).Name("Comment");
            Map(m => m.Latitude).Index(21).Name("Latitude");
            Map(m => m.Longitude).Index(22).Name("Longitude");
        }
    }

    public class PCTEL_DataSetModelCPMap : PCTEL_DataSetRowMap
    {
        public PCTEL_DataSetModelCPMap()
        {
            DataSetVariant = new PCTEL_DataSetVariant(PCTEL_DataSetVariantIDs.PCTEL_DST_CP);

            //location
            Map(m => m.GridID).Ignore();
            Map(m => m.LocID).Index(1).Name("Point Id");
            Map(m => m.Label).Index(2).Name("Label");

            //variant specific
            Map(m => m.SelReference).Ignore();
            Map(m => m.Comment).Index(20).Name("Comment");
            Map(m => m.Latitude).Index(21).Name("Latitude");
            Map(m => m.Longitude).Index(22).Name("Longitude");
        }
    }

    public class PCTEL_DataSetModelRefMap : PCTEL_DataSetRowMap
    {
        public PCTEL_DataSetModelRefMap()
        {
            DataSetVariant = new PCTEL_DataSetVariant(PCTEL_DataSetVariantIDs.PCTEL_DST_REF);

            //location

            Map(m => m.GridID).Ignore();
            Map(m => m.LocID).Index(1).Name("Point Id");
            Map(m => m.Label).Index(2).Name("Label");

            //variant specific
            Map(m => m.SelReference).Index(20).Name("Selected Reference");
            Map(m => m.Comment).Index(21).Name("Comment");
            Map(m => m.Latitude).Index(22).Name("Latitude");
            Map(m => m.Longitude).Index(23).Name("Longitude");
        }
    }

    public class PCTEL_DataSetRowMap : PCTEL_TableRowMap<PCTEL_DataSetRowModel>, IHasDataSetVariant
    {
        #region ClassMembers

        public PCTEL_DataSetVariant DataSetVariant { get; set; }

        public static Type GetClassMapType(PCTEL_DataSetVariant dataSetVariant)
        {
            switch (dataSetVariant.ID)
            {
                case PCTEL_DataSetVariantIDs.PCTEL_DST_AREA:
                    return typeof(PCTEL_DataSetModelAreaMap);

                case PCTEL_DataSetVariantIDs.PCTEL_DST_CP:
                    return typeof(PCTEL_DataSetModelCPMap);

                case PCTEL_DataSetVariantIDs.PCTEL_DST_REF:
                    return typeof(PCTEL_DataSetModelRefMap);

                default:
                    throw new ArgumentException("Bad dataSetVariant");
            }
        }

        #endregion ClassMembers

        #region ctor

        public PCTEL_DataSetRowMap()
        {
            Map(m => m.Floor).Index(0).Name("Floor Plan");
            Map(m => m.Protocol).Index(3).Name("Protocol");
            Map(m => m.Band).Index(4).Name("Band");
            Map(m => m.MeasurementType).Index(5).Name("MeasurementType");
            Map(m => m.ChannelID).Index(6).Name("Channel");
            Map(m => m.Frequency).Index(7).Name("Frequency")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
            Map(m => m.DLPower).Index(8).Name("DL Power(dBm)")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
            Map(m => m.DLSN).Index(9).Name("DL S/N(dB)")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
            Map(m => m.DLFBER).Index(10).Name("DL FBER(%)")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
            Map(m => m.DLDAQ).Index(11).Name("DL DAQ")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
            Map(m => m.DLBER).Index(12).Name("DL BER(%)")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
            Map(m => m.DLSignalPower).Index(13).Name("DL Signal Power(dBm)")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
            Map(m => m.ULPower).Index(14).Name("UL Power(dBm)")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
            Map(m => m.ULSN).Index(15).Name("UL S/N(dB)")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
            Map(m => m.ULFBER).Index(16).Name("UL FBER(%)")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
            Map(m => m.ULDAQ).Index(17).Name("UL DAQ")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
            Map(m => m.ULBER).Index(18).Name("UL BER(%)")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
            Map(m => m.ULSignalPower).Index(19).Name("UL Signal Power(dBm)")
                .TypeConverter<PCTEL_FloatConverter<float?>>();
        }

        #endregion ctor
    }

    public class PCTEL_DataSetRowModel : PCTEL_TableRowModel, IHasDataSetVariant
    {
        [Ignore]
        private PCTEL_DataSetVariant _dataSetVariant;

        [Ignore]
        public PCTEL_DataSetVariant DataSetVariant
        {
            get => _dataSetVariant;
            set
            {
                if (_dataSetVariant is null)
                {
                    _dataSetVariant = value;
                }
                else
                {
                    throw new InvalidOperationException("Cannot alter model DataSetVariant");
                }
            }
        }

        #region Model

        public string Band { get; set; }
        public string ChannelID { get; set; }
        public string Comment { get; set; }
        public float? DLBER { get; set; }
        public float? DLDAQ { get; set; }
        public float? DLFBER { get; set; }
        public float? DLPower { get; set; }
        public float? DLSignalPower { get; set; }
        public float? DLSN { get; set; }
        public float? Frequency { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MeasurementType { get; set; }
        public string Protocol { get; set; }
        public string SelReference { get; set; }
        public float? ULBER { get; set; }
        public float? ULDAQ { get; set; }
        public float? ULFBER { get; set; }
        public float? ULPower { get; set; }
        public float? ULSignalPower { get; set; }
        public float? ULSN { get; set; }

        #endregion Model
    }
}