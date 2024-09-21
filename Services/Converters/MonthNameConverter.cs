using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Converters
{
    public class MonthNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int month && month >= 1 && month <= 12)
            {
                DateTimeFormatInfo dtInfo = CultureInfo.CurrentCulture.DateTimeFormat;
                return dtInfo.GetMonthName(month);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ConvertBack is not used in this case, so return the original value
            return value;
        }
    }
}
