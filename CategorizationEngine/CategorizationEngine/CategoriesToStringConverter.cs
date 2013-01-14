using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CategorizationEngine
{
    public class CategoriesToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            var categories = value as IEnumerable<Category>;
            if (categories == null)
                return DependencyProperty.UnsetValue;

            var sb = new StringBuilder();
            foreach (var category in categories)
                sb.Append(string.Format("[{0}] ", category.Name));
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
