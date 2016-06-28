using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using widget3.Code;

namespace widget3.Converters
{
    class SmallTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? value : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            Regex smallTimeStingRegEx = new Regex(@"[0-9]{1,2}:[0-9]{1,2}$");
            if (smallTimeStingRegEx.IsMatch((string)value))
            {
                return new SmallTime((string)value);
            }

            return value;
        }
    }
}
