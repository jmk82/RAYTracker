using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RAYTracker.Helpers
{
    public class BooleanToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value) return new GridLength(0);

            var param = System.Convert.ToDouble(parameter);
            
            return new GridLength(param, GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (GridLength) value;
        }
    }
}
