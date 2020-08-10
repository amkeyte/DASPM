using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace DASPM_PCTEL.DataSet
{
    internal class PCTEL_FloatConverter<T> : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            //null value
            if (text == "") return null;

            //special NT (Not Tested) value
            if (text == "NT") return float.MinValue;

            //int value contained
            var numberStyle = memberMapData.TypeConverterOptions.NumberStyle ?? NumberStyles.Float;

            if (float.TryParse(text, numberStyle, memberMapData.TypeConverterOptions.CultureInfo, out var f))
            {
                return f;
            }

            //default processing
            return base.ConvertFromString(text, row, memberMapData);
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null) return "";
            if ((float)value == float.MinValue) return "NT";
            return value.ToString();
        }
    }
}