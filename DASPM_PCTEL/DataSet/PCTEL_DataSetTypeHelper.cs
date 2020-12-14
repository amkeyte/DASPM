using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM_PCTEL.DataSet
{
    public class PCTEL_DataSetTypeHelper
    {
        public const string PCTEL_DATASET_TYPE_NAME_AREA = "AreaTestPoints";
        public const string PCTEL_DATASET_TYPE_NAME_CP = "CriticalTestPoints";
        public const string PCTEL_DATASET_TYPE_NAME_REF = "*****";

        public static void AssignDataSetType(IHas)


public static Type GetClassMapType(string fullPath)
        {
            var dataSetType = GetDataSetType(fullPath);
            return GetClassMapType(dataSetType);
        }

        public static Type GetClassMapType(PCTEL_DataSetTypes dataSetType)
        {
            switch (dataSetType)
            {
                case PCTEL_DataSetTypes.PCTEL_DST_AREA:
                    return typeof(PCTEL_DataSetModelAreaMap);

                case PCTEL_DataSetTypes.PCTEL_DST_CP:
                    return typeof(PCTEL_DataSetModelCPMap);

                case PCTEL_DataSetTypes.PCTEL_DST_REF:
                    return typeof(PCTEL_DataSetModelRefMap);

                default:
                    throw new ArgumentException("Bad dataSetType");
            }
        }

        public static PCTEL_DataSetTypes GetDataSetType(string fullPath)
        {
            if (fullPath.Contains(PCTEL_DATASET_TYPE_NAME_AREA))
            {
                return PCTEL_DataSetTypes.PCTEL_DST_AREA;
            }
            else if (fullPath.Contains(PCTEL_DATASET_TYPE_NAME_CP))
            {
                return PCTEL_DataSetTypes.PCTEL_DST_CP;
            }
            else
            {
                throw new ArgumentException("Invalid fullPath: used a valid Area, CP, or Ref csv file?");
            }
        }

        public static PCTEL_DataSetTypes GetDataSetTypeFromLocType(string locType)
        {
            if (locType == "AREA")
            {
                return PCTEL_DataSetTypes.PCTEL_DST_AREA;
            }
            else if (locType == "CP")
            {
                return PCTEL_DataSetTypes.PCTEL_DST_CP;
            }
            else if (locType == "REF")
            {
                return PCTEL_DataSetTypes.PCTEL_DST_REF;
            }
            else
            {
                throw new ArgumentException("locType is not a valid DataSetType text identifier");
            }
        }

        public static string GetDataSetTypeName(PCTEL_DataSetTypes dataSetType)
        {
            switch (dataSetType)
            {
                case PCTEL_DataSetTypes.PCTEL_DST_AREA:
                    return PCTEL_DATASET_TYPE_NAME_AREA;

                case PCTEL_DataSetTypes.PCTEL_DST_CP:
                    return PCTEL_DATASET_TYPE_NAME_CP;

                case PCTEL_DataSetTypes.PCTEL_DST_REF:
                    return PCTEL_DATASET_TYPE_NAME_REF;

                default:
                    throw new ArgumentException("Bad dataSetType");
            }
        }

        public static string GetDataSetTypeText(PCTEL_DataSetTypes dataSetType)
        {
            switch (dataSetType)
            {
                case PCTEL_DataSetTypes.PCTEL_DST_AREA:
                    return "AREA";

                case PCTEL_DataSetTypes.PCTEL_DST_CP:
                    return "CP";

                case PCTEL_DataSetTypes.PCTEL_DST_REF:
                    return "REF";

                default:
                    throw new ArgumentException("Bad dataSetType");
            }
        }
    }
}