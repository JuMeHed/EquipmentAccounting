using System;
using System.Globalization;
using System.Windows.Data;

namespace EquipmentAccounting.Converters
{
    internal class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "Применен" : "Не применен";
            }
            return "Не применен";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (string.Equals(stringValue, "Применен", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else if (string.Equals(stringValue, "Не применен", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            throw new InvalidOperationException("Неправильное значение для преобразования.");
        }
    }
}
