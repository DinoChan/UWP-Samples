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
    public class RectangleStrokeDashArrayConverter:DependencyObject, IValueConverter
    {
        /// <summary>
        /// 获取或设置TargetRectangle的值
        /// </summary>  
        public Rectangle TargetRectangle
        {
            get { return (Rectangle)GetValue(TargetRectangleProperty); }
            set { SetValue(TargetRectangleProperty, value); }
        }

        /// <summary>
        /// 标识 TargetRectangle 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TargetRectangleProperty =
            DependencyProperty.Register("TargetRectangle", typeof(Rectangle), typeof(RectangleStrokeDashArrayConverter), new PropertyMetadata(null));
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double == false)
                return null;

            var progress = (double)value;

            if (TargetRectangle == null)
                return null;

            var totalLength = GetTotalLength();
            totalLength = totalLength / TargetRectangle.StrokeThickness;
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
            if (TargetRectangle == null)
                return 0;

            return (TargetRectangle.ActualHeight + TargetRectangle.ActualWidth) * 2;
        }
    }
}
