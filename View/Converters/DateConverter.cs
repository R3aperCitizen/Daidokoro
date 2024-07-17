using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daidokoro.View.Converters
{
    public class DateConverter : IValueConverter
    {
        private const string DATE_FORMAT = "dd/MM/yyyy";
        private const string FAILURE_STRING = "Non ottenuto";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is DateTime ? ((DateTime)value).ToString(DATE_FORMAT) : FAILURE_STRING;

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
