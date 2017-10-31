using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Converters
{
    public class BoolToHorizontalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
            {
                return HorizontalAlignment.Right;
            }

            return HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}