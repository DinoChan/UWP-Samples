using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TemplatedControlSample
{
    public class DateTimeSelector2 : Control
    {
        /// <summary>
        /// 标识 Date 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DateTimeSelector2), new PropertyMetadata(null, OnDateChanged));

        private static void OnDateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DateTimeSelector2 target = obj as DateTimeSelector2;
            DateTime oldValue = (DateTime)args.OldValue;
            DateTime newValue = (DateTime)args.NewValue;
            if (oldValue != newValue)
                target.OnDateChanged(oldValue, newValue);
        }



        /// <summary>
        /// 标识 Time 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(TimeSpan), typeof(DateTimeSelector2), new PropertyMetadata(null, OnTimeChanged));

        private static void OnTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DateTimeSelector2 target = obj as DateTimeSelector2;
            TimeSpan oldValue = (TimeSpan)args.OldValue;
            TimeSpan newValue = (TimeSpan)args.NewValue;
            if (oldValue != newValue)
                target.OnTimeChanged(oldValue, newValue);
        }

        /// <summary>
        /// 标识 DateTime 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DateTimeProperty =
            DependencyProperty.Register("DateTime", typeof(DateTime), typeof(DateTimeSelector2), new PropertyMetadata(null, OnDateTimeChanged));

        private static void OnDateTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DateTimeSelector2 target = obj as DateTimeSelector2;
            DateTime oldValue = (DateTime)args.OldValue;
            DateTime newValue = (DateTime)args.NewValue;
            if (oldValue != newValue)
                target.OnDateTimeChanged(oldValue, newValue);
        }

        public DateTimeSelector2()
        {
            this.DefaultStyleKey = typeof(DateTimeSelector2);
        }

        /// <summary>
        /// 获取或设置Date的值
        /// </summary>  
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        /// <summary>
        /// 获取或设置Time的值
        /// </summary>  
        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        /// <summary>
        /// 获取或设置DateTime的值
        /// </summary>  
        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        private bool _isUpdatingDateTime;

        protected virtual void OnDateChanged(DateTime oldValue, DateTime newValue)
        {
            UpdateDateTime();
        }


        protected virtual void OnTimeChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            UpdateDateTime();
        }

        protected virtual void OnDateTimeChanged(DateTime oldValue, DateTime newValue)
        {
            _isUpdatingDateTime = true;
            try
            {
                    Date = newValue.Date;
                    Time = newValue.TimeOfDay;
            }
            finally
            {
                _isUpdatingDateTime = false;
            }
        }

        private void UpdateDateTime()
        {
            if (_isUpdatingDateTime)
                return;

                DateTime = Date.Date.Add(Time);
        }
    }
}
