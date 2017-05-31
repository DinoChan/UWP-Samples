using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ShapeDemo2
{

 
    public  class PathExtention:DependencyObject
    {

        /// <summary>
        //  从指定元素获取 Progress 依赖项属性的值。
        /// </summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>Progress 依赖项属性的值</returns>
        public static double GetProgress(DependencyObject obj)
        {
            return (double)obj.GetValue(ProgressProperty);
        }

        /// <summary>
        /// 将 Progress 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">The element on which to set the property value.</param>
        /// <param name="value">The property value to set.</param>
        public static void SetProgress(DependencyObject obj, double value)
        {
            obj.SetValue(ProgressProperty, value);
        }

        /// <summary>
        /// 标识 Progress 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.RegisterAttached("Progress", typeof(double), typeof(PathExtention), new PropertyMetadata(0d, OnProgressChanged));


        private static void OnProgressChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Path target = obj as Path;
            double progress = (double)args.NewValue;
            if (target.ActualHeight == 0 || target.ActualWidth == 0)
                return;

            if (target.StrokeThickness == 0)
                return;

            var totalLength = GetTotalLength(target);
            var result = new DoubleCollection { progress * totalLength / 100 / target.StrokeThickness, double.MaxValue };
            target.StrokeDashArray = result;
        }

        private static double GetTotalLength(Path path)
        {
            var geometry = path.Data as PathGeometry;
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