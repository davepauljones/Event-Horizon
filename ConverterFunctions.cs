using System;
using System.Windows.Data;

namespace The_Oracle
{
    public class HeightToParentConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            double height = (double)value;
            double adjustment = Convert.ToDouble(parameter);

            if (height - adjustment > 0)
                return height - adjustment;
            else
                return height;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            double height = (double)value;
            double adjustment = Convert.ToDouble(parameter);

            return height - adjustment;
        }
    }
}