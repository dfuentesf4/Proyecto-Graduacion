using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                // Convierte el valor decimal a cadena con el punto como separador
                return decimalValue.ToString(CultureInfo.InvariantCulture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                // Reemplaza la coma por un punto y convierte a decimal
                stringValue = stringValue.Replace(",", ".");
                if (decimal.TryParse(stringValue, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var result))
                {
                    return result;
                }
            }
            return 0m; // Retorna 0 si no es un decimal válido
        }
    }
}
