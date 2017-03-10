using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace TemplatedControlSample
{
    [TemplatePart(Name = DateElementPartName, Type = typeof(CalendarDatePicker))]
    [TemplatePart(Name = TimeElementPartName, Type = typeof(TimePicker))]
    [StyleTypedProperty(Property = "TimePickerStyle", StyleTargetType = typeof(TimePicker))]
    public class DateTimeSelector : Control
    {
        public const string DateElementPartName = "DateElement";
        public const string TimeElementPartName = "TimeElement";

        /// <summary>
        /// 标识 SelectedDateTime 依赖属性。
        /// </summary>
        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime", typeof(DateTime), typeof(DateTimeSelector), new PropertyMetadata(DateTime.Now, OnSelectedDateTimeChanged));

        private static void OnSelectedDateTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DateTimeSelector target = obj as DateTimeSelector;
            DateTime oldValue = (DateTime)args.OldValue;
            DateTime newValue = (DateTime)args.NewValue;
            if (oldValue != newValue)
                target.OnSelectedDateTimeChanged(oldValue, newValue);
        }




        /// <summary>
        /// 标识 TimePickerStyle 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TimePickerStyleProperty =
            DependencyProperty.Register("TimePickerStyle", typeof(Style), typeof(DateTimeSelector), new PropertyMetadata(null));



        public DateTimeSelector()
        {
            this.DefaultStyleKey = typeof(DateTimeSelector);
        }

        /// <summary>
        /// 获取或设置SelectedDateTime的值
        /// </summary>  
        public DateTime SelectedDateTime
        {
            get { return (DateTime)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        /// <summary>
        /// 获取或设置TimePickerStyle的值
        /// </summary>  
        public Style TimePickerStyle
        {
            get { return (Style)GetValue(TimePickerStyleProperty); }
            set { SetValue(TimePickerStyleProperty, value); }
        }

        private CalendarDatePicker _dateElement;
        private TimePicker _timeElement;
        private bool _isUpdatingDateTime;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_dateElement != null)
                _dateElement.DateChanged -= OnDateElementDateChanged;

            _dateElement = GetTemplateChild(DateElementPartName) as CalendarDatePicker;
            if (_dateElement != null)
                _dateElement.DateChanged += OnDateElementDateChanged;

            if (_timeElement != null)
                _timeElement.TimeChanged -= OnTimeElementTimeChanged;

            _timeElement = GetTemplateChild(TimeElementPartName) as TimePicker;
            if (_timeElement != null)
                _timeElement.TimeChanged += OnTimeElementTimeChanged;

            UpdateElement();
        }

        protected virtual void OnSelectedDateTimeChanged(DateTime oldValue, DateTime newValue)
        {
            UpdateElement();
        }

        private void OnDateElementDateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            UpdateSelectDateTime();
        }

        private void OnTimeElementTimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            UpdateSelectDateTime();
        }


        private void UpdateElement()
        {
            _isUpdatingDateTime = true;
            try
            {
                if (_dateElement != null)
                    _dateElement.Date = SelectedDateTime.Date;

                if (_timeElement != null)
                    _timeElement.Time = SelectedDateTime.TimeOfDay;
            }
            finally
            {
                _isUpdatingDateTime = false;
            }
        }

        private void UpdateSelectDateTime()
        {
            if (_isUpdatingDateTime)
                return;

            DateTime dateTime = DateTime.Now;
            if (_dateElement != null && _dateElement.Date.HasValue)
                dateTime = _dateElement.Date.Value.Date;

            if (_timeElement != null)
                dateTime = dateTime.Add(_timeElement.Time);

            SelectedDateTime = dateTime;
        }
    }
}
