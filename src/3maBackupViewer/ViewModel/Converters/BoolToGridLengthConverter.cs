using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Converters
{
    public class BoolToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool outgoing = (bool)value;
            bool invert = (bool) (parameter ?? false);

            if (invert)
            {
                return new GridLength(outgoing ? 2 : 8, GridUnitType.Star);
            }

            return new GridLength(outgoing ? 8 : 2, GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}