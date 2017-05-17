using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ShapeDemo
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


        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double == false)
                return null;

            var progress = (double)value;

            if (TargetTriangle == null)
                return null;

            var totalLength = GetTriangleTotalLength();
            var result = new DoubleCollection { progress * totalLength / 100 / TargetTriangle.StrokeThickness, double.MaxValue };
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        protected double GetTriangleTotalLength()
        {
            var geometry = TargetTriangle.Data as PathGeometry;
            if (geometry == null)
                return 0;

            if (geometry.Figures.Any() == false)
                return 0;

            var figure = geometry.Figures.FirstOrDefault();
            if (figure == null)
                return 0;

            var totalLength = 0d;
            var point = figure.StartPoint;
            foreach (var item in figure.Segments)
            {
                var segment = item as LineSegment;
                if (segment == null)
                    return 0;

                totalLength += Math.Sqrt(Math.Pow(point.X - segment.Point.X, 2) + Math.Pow(point.Y - segment.Point.Y, 2));
                point = segment.Point;
            }

            totalLength += Math.Sqrt(Math.Pow(point.X - figure.StartPoint.X, 2) + Math.Pow(point.Y - figure.StartPoint.Y, 2));
            return totalLength;
        }
    }
}
