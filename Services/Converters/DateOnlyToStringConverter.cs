using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Converters
{
    public class DateOnlyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly date)
            {
                // Aquí formateamos la fecha en el formato deseado
                return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            // Si no hay fecha, devolvemos un valor vacío o un texto por defecto
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DateOnly.TryParse(value.ToString(), out var date))
            {
                return date;
            }

            return null;
        }
    }
}
