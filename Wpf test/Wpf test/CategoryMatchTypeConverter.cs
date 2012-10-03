using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Wpf_test
{
    [ValueConversion(typeof(CategoryMatchType), typeof(Visibility))]  
    public class CategoryMatchTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is CategoryMatchType))
                throw new ArgumentException("Value must be of type CategoryMatchType");

            CategoryMatchType cmt = (CategoryMatchType)value;
            if (cmt == CategoryMatchType.Manual)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
