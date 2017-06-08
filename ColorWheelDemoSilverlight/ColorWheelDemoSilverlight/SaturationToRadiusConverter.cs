using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ColorWheelDemoSilverlight
{
    public class SaturationToRadiusConverter : DependencyObject, IValueConverter
    {



        /// <summary>
        /// 获取或设置Radius的值
        /// </summary>  
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        /// 标识 Radius 依赖属性。
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(SaturationToRadiusConverter), new PropertyMetadata(0d, OnRadiusChanged));

        private static void OnRadiusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SaturationToRadiusConverter target = obj as SaturationToRadiusConverter;
            double oldValue = (double)args.OldValue;
            double newValue = (double)args.NewValue;
            if (oldValue != newValue)
                target.OnRadiusChanged(oldValue, newValue);
        }

        protected virtual void OnRadiusChanged(double oldValue, double newValue)
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * this.Radius / 100d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
