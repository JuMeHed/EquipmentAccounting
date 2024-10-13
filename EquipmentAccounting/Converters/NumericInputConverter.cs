using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace EquipmentAccounting.Converters
{
    public class NumericInputConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string input)
            {
                return Regex.Replace(input, "[^0-9]", "");
            }
            return "";
        }
    }
}
