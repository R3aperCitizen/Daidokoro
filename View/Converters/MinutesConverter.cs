using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daidokoro.View.Converters
{
    public class MinutesConverter : IValueConverter
    {
        private const string TIME_UNIT = "min";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => $"{(int)value} {TIME_UNIT}";

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
