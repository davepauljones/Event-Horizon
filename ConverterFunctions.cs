using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

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
    
    public class RectConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double width && values[1] is double height)
            {
                return new Rect(0, 0, width, height);
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}