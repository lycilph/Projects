using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Wp_filter_controls_test
{
    [ValueConversion(typeof(bool), typeof(Brush))]
    public class ValidToBorderBrushConverter : IValueConverter
    {
        public Brush InvalidBrush { get; set; }

        public ValidToBorderBrushConverter()
        {
            InvalidBrush = Brushes.Red;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool valid = (bool)value;
            return (valid ? Brushes.Transparent : InvalidBrush);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
