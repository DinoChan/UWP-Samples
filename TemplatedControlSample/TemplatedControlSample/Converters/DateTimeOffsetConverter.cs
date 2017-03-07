﻿using System;
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
            DateTime? from = value as DateTime?;
            if (from == null)
                return new DateTimeOffset(DateTime.Now);

            return new DateTimeOffset(from.Value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset?)
            {
                DateTimeOffset? from = value as DateTimeOffset?;
                if (from == null)
                    return null;

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
