using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TemplatedControlSample
{

    public sealed partial class DateTimeSelector3 : UserControl
    {
        /// <summary>
        /// 标识 DateTime 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DateTimeProperty =
            DependencyProperty.Register("DateTime", typeof(DateTime?), typeof(DateTimeSelector3), new PropertyMetadata(null, OnDateTimeChanged));

        private static void OnDateTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DateTimeSelector3 target = obj as DateTimeSelector3;
            DateTime? oldValue = (DateTime?)args.OldValue;
            DateTime? newValue = (DateTime?)args.NewValue;
            if (oldValue != newValue)
                target.OnDateTimeChanged(oldValue, newValue);
        }

        public DateTimeSelector3()
        {
            this.InitializeComponent();
            DateTime = System.DateTime.Now;
            TimeElement.TimeChanged += OnTimeChanged;
            DateElement.DateChanged += OnDateChanged;
        }


        /// <summary>
        /// 获取或设置DateTime的值
        /// </summary>  
        public DateTime? DateTime
        {
            get { return (DateTime?)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        private bool _isUpdatingDateTime;

        private void OnDateTimeChanged(DateTime? oldValue, DateTime? newValue)
        {
            _isUpdatingDateTime = true;
            try
            {
                if (DateElement != null && DateTime != null)
                    DateElement.Date = DateTime.Value;

                if (TimeElement != null && DateTime != null)
                    TimeElement.Time = DateTime.Value.TimeOfDay;
            }
            finally
            {
                _isUpdatingDateTime = false;
            }
        }

        private void OnDateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            UpdateDateTime();
        }

        private void OnTimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            if (_isUpdatingDateTime)
                return;

            DateTime = DateElement.Date.Date.Add(TimeElement.Time);
        }
    }
}
