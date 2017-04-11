using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TemplatedControlSample
{
    public class DateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((value is DateTime) == false)
                return new DateTimeOffset(DateTime.Now);

            DateTime from = (DateTime)value ;
            return new DateTimeOffset(from);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset?)
            {
                DateTimeOffset? from = value as DateTimeOffset?;
                return from.Value.DateTime;
            }
            else
            {
                DateTimeOffset from = (DateTimeOffset)value;
                return from.DateTime;
            }
        }
    }
}
