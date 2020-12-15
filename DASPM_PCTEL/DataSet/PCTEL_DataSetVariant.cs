using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM_PCTEL.DataSet
{
    public enum PCTEL_DataSetVariantIDs

    {
        PCTEL_DST_REF = 100,
        PCTEL_DST_CP,
        PCTEL_DST_AREA
    }

    public class PCTEL_DataSetVariant
    {
        #region ctor

        public PCTEL_DataSetVariant(PCTEL_DataSetVariantIDs variant)
        {
            if (variant == 0) throw new ArgumentException("Bad variant ID");

            ID = variant;
        }

        public PCTEL_DataSetVariant(string initStr)
        {
            if (initStr.Contains(VARIANT_NAME_AREA) || initStr == "AREA")
            {
                ID = PCTEL_DataSetVariantIDs.PCTEL_DST_AREA;
            }
            else if (initStr.Contains(VARIANT_NAME_CP) || initStr == "CP")
            {
                ID = PCTEL_DataSetVariantIDs.PCTEL_DST_CP;
            }
            else if (initStr.Contains(VARIANT_NAME_REF) || initStr == "REF")
            {
                ID = PCTEL_DataSetVariantIDs.PCTEL_DST_REF;
            }
            else
            {
                throw new ArgumentException("Invalid fullPath: used a valid Area, CP, or Ref csv file?");
            }
        }

        #endregion ctor

        public const string VARIANT_NAME_AREA = "AreaTestPoints";

        public const string VARIANT_NAME_CP = "CriticalTestPoints";

        public const string VARIANT_NAME_REF = "*****";

        public PCTEL_DataSetVariantIDs ID { get; protected set; }

        public string Name
        {
            get
            {
                switch (ID)
                {
                    case PCTEL_DataSetVariantIDs.PCTEL_DST_AREA:
                        return VARIANT_NAME_AREA;

                    case PCTEL_DataSetVariantIDs.PCTEL_DST_CP:
                        return VARIANT_NAME_CP;

                    case PCTEL_DataSetVariantIDs.PCTEL_DST_REF:
                        return VARIANT_NAME_REF;

                    default:
                        throw new ArgumentException("Bad VariantID");
                }
            }
        }

        public string LocType
        {
            get
            {
                switch (ID)
                {
                    case PCTEL_DataSetVariantIDs.PCTEL_DST_AREA:
                        return "AREA";

                    case PCTEL_DataSetVariantIDs.PCTEL_DST_CP:
                        return "CP";

                    case PCTEL_DataSetVariantIDs.PCTEL_DST_REF:
                        return "REF";

                    default:
                        throw new ArgumentException("Bad VariantID");
                }
            }
        }

        public PCTEL_DataSetVariant Copy()
        {
            return new PCTEL_DataSetVariant(this.ID);
        }
    }
}