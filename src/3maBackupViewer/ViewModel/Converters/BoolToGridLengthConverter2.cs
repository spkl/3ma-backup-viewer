using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Converters
{
    public class BoolToGridLengthConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool outgoing = (bool)value;
            bool invert = (bool) (parameter ?? false);

            if (invert)
            {
                return new GridLength(1, outgoing ? GridUnitType.Auto : GridUnitType.Star);
            }

            return new GridLength(1, outgoing ? GridUnitType.Star : GridUnitType.Auto);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}