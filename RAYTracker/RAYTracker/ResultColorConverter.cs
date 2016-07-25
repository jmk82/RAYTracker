using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RAYTracker
{
    public class ResultColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Session session = value as Session;
            if (session.Result < 0)
            {
                return Brushes.LightCoral;
            }
            else
            {
                return Brushes.LightGreen;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
