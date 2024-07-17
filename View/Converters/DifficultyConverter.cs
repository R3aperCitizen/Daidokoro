using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daidokoro.View.Converters
{
    public class DifficultyConverter : IValueConverter
    {
        private const string DIFFICULTY_SYMBOL = "🌶️";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => Enumerable.Repeat(DIFFICULTY_SYMBOL, (int)value).Aggregate((s1, s2) => s1 + s2);

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
