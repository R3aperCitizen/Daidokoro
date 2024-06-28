using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daidokoro.View.Converters
{
    public class LikesConverter : IValueConverter
    {
        private const string LIKES_SYMBOL = "❤️";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => $"{(int)value} {LIKES_SYMBOL}";

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
