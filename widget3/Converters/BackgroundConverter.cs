using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace widget3.Converters
{
    public class BackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Regex colorRegex = new Regex(@"\#.{8}$");
            if (colorRegex.IsMatch((string)value))
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)value));
            }

            var image = new BitmapImage(new Uri((string)value, UriKind.Absolute));
            return new ImageBrush(image);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is SolidColorBrush)
            {
                return ((SolidColorBrush)value).Color.ToString();
            }

            return "path";
        }
    }
}
