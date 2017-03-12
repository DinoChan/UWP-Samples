using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TemplatedControlSample
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TimeSpan? from = value as TimeSpan?;
            if (from == null)
                return TimeSpan.Zero;

            return from.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            TimeSpan from = (TimeSpan)value;
            return from as TimeSpan?;
        }
    }
}
