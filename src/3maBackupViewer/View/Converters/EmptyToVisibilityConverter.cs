using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LateNightStupidities.IIImaBackupViewer.View.Converters
{
    public class EmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool empty;
            if (value is string str)
            {
                empty = string.IsNullOrEmpty(str);
            }
            else
            {
                empty = value == null;
            }

            return empty ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}