using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Converters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                // Usar cultura de Guatemala 'es-GT'
                return decimalValue.ToString("C", new CultureInfo("es-GT"));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Currency, new CultureInfo("es-GT"), out var result))
            {
                return result;
            }
            return value;
        }
    }
}
