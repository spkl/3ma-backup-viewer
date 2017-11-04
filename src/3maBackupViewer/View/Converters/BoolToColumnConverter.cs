using System;
using System.Globalization;
using System.Windows.Data;

namespace LateNightStupidities.IIImaBackupViewer.View.Converters
{
    public class BoolToColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool outgoing = (bool) value;
            bool invert = (bool) (parameter ?? false);

            if (invert)
            {
                return outgoing ? 0 : 1;
            }

            return outgoing ? 1 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}