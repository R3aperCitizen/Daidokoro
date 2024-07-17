using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daidokoro.View.Converters
{
    public class RatingConverter : IValueConverter
    {
        private const string POSITIVE_VOTE = "🍋‍🟩";
        private const string NEGATIVE_VOTE = "🧅";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => (bool)value ? POSITIVE_VOTE : NEGATIVE_VOTE;

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
