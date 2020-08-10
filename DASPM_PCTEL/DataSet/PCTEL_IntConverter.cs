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
    public class PCTEL_IntConverter<T> : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            //null value
            if (text == "") return null;

            //special NT (Not Tested) value
            if (text == "NT") return Int32.MinValue;

            //int value contained
            var numberStyle = memberMapData.TypeConverterOptions.NumberStyle ?? NumberStyles.Integer;

            if (int.TryParse(text, numberStyle, memberMapData.TypeConverterOptions.CultureInfo, out var i))
            {
                return i;
            }

            //default processing
            return base.ConvertFromString(text, row, memberMapData);
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (int.Parse(value.ToString()) == 0) return ""; //broken

            return value.ToString();
        }
    }
}