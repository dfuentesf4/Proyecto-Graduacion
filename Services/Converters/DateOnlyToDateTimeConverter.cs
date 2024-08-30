using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Converters
{
    public class DateOnlyToDateTimeConverter : IValueConverter
    {
        // Convierte de DateOnly a DateTime
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly dateOnly)
            {
                return dateOnly.ToDateTime(TimeOnly.MinValue); // Convertir DateOnly a DateTime
            }
            return DateTime.Now;
        }

        // Convierte de DateTime a DateOnly
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return DateOnly.FromDateTime(dateTime); // Convertir DateTime a DateOnly
            }
            return DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
