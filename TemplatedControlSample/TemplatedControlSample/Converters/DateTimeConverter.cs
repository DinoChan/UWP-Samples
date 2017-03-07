using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TemplatedControlSample
{
   public class DateTimeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime? from = value as DateTime?;
            if (from == null)
                return DateTime.Now;

            return from.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
