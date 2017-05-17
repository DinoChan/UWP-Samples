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
using System.Linq;

namespace ShapeDemoSilverlight
{
    public class ProgressToStrokeDashArrayConverter : DependencyObject, IValueConverter
    {

        /// <summary>
        /// 获取或设置TargetTriangle的值
        /// </summary>  
        public Triangle TargetTriangle
        {
            get { return (Triangle)GetValue(TargetTriangleProperty); }
            set { SetValue(TargetTriangleProperty, value); }
        }

        /// <summary>
        /// 标识 TargetTriangle 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TargetTriangleProperty =
            DependencyProperty.Register("TargetTriangle", typeof(Triangle), typeof(ProgressToStrokeDashArrayConverter), new PropertyMetadata(null));


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value is double) == false)
                return null;

            double progress = (double)value;
        

            if (TargetTriangle == null)
                return null;


            var geometry = TargetTriangle.Data as PathGeometry;
            if (geometry == null)
                return null;

            if (geometry.Figures.Any() == false)
                return null;

            var figure = geometry.Figures.FirstOrDefault();
            if (figure == null)
                return null;

            var totalLength = 0d;
            var point = figure.StartPoint;
            foreach (var item in figure.Segments)
            {
                var segment = item as LineSegment;
                if (segment == null)
                    return null;

                totalLength += Math.Sqrt(Math.Pow(point.X - segment.Point.X, 2) + Math.Pow(point.Y - segment.Point.Y, 2));
                point = segment.Point;
            }

            totalLength += Math.Sqrt(Math.Pow(point.X - figure.StartPoint.X, 2) + Math.Pow(point.Y - figure.StartPoint.Y, 2));

            var result = new DoubleCollection { progress * totalLength / 100/TargetTriangle.StrokeThickness, double.MaxValue };
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
