using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Converters
{
    public class DollarCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                // Usar el formato con símbolo de dólar
                return decimalValue.ToString("C", CultureInfo.GetCultureInfo("en-US"));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out var result))
            {
                return result;
            }
            return value;
        }
    }
}
