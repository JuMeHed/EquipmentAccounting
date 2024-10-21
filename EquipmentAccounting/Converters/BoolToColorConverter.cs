using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EquipmentAccounting.Converters
{
    internal class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#39ed9f")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f71441"));
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
