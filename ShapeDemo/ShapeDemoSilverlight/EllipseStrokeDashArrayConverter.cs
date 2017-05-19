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

namespace ShapeDemoSilverlight
{
    public class EllipseStrokeDashArrayConverter : DependencyObject, IValueConverter
    {

        /// <summary>
        /// 获取或设置TargetEllipse的值
        /// </summary>  
        public Ellipse TargetEllipse
        {
            get { return (Ellipse)GetValue(TargetEllipseProperty); }
            set { SetValue(TargetEllipseProperty, value); }
        }

        /// <summary>
        /// 标识 TargetEllipse 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TargetEllipseProperty =
            DependencyProperty.Register("TargetEllipse", typeof(Ellipse), typeof(EllipseStrokeDashArrayConverter), new PropertyMetadata(null));


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double == false)
                return new DoubleCollection { 0, double.MaxValue }; ;

            var progress = (double)value;

            if (TargetEllipse == null)
                return new DoubleCollection { 0, double.MaxValue }; ;

            var totalLength = GetTotalLength();
            totalLength = totalLength / TargetEllipse.StrokeThickness;
            var thirdSection = progress * totalLength / 100;
            var secondSection = (totalLength - thirdSection) / 2;
            var result = new DoubleCollection { 0, secondSection, thirdSection, double.MaxValue };
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private double GetTotalLength()
        {
            if (TargetEllipse == null)
                return 0;

            return (TargetEllipse.ActualHeight - TargetEllipse.StrokeThickness) * Math.PI;
        }
    }
}
