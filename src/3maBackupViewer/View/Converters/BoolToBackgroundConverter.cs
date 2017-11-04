using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LateNightStupidities.IIImaBackupViewer.View.Converters
{
    public class BoolToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool outgoing = (bool) value;
            if (outgoing)
            {
                return Brushes.LightGreen;
            }

            return Brushes.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}