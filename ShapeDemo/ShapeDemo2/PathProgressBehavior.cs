using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ShapeDemo2
{
    public class PathProgressBehavior : Behavior<UIElement>
    {

        protected override void OnAttached()
        {
            base.OnAttached();

        }

        /// <summary>
        /// 获取或设置Progress的值
        /// </summary>  
        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        /// <summary>
        /// 标识 Progress 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(double), typeof(PathProgressBehavior), new PropertyMetadata(0d, OnProgressChanged));

        private static void OnProgressChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PathProgressBehavior target = obj as PathProgressBehavior;
            double oldValue = (double)args.OldValue;
            double newValue = (double)args.NewValue;
            if (oldValue != newValue)
                target.OnProgressChanged(oldValue, newValue);
        }


        protected virtual void OnProgressChanged(double oldValue, double newValue)
        {
            UpdateStrokeDashArray();
        }

        protected virtual double GetTotalLength(Path path)
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


        private void UpdateStrokeDashArray()
        {
            var target = AssociatedObject as Path;
            if (target == null)
                return;

            double progress = Progress;
            //if (target.ActualHeight == 0 || target.ActualWidth == 0)
            //    return;

            if (target.StrokeThickness == 0)
                return;

            var totalLength = GetTotalLength(target);
            var result = new DoubleCollection { progress * totalLength / 100 / target.StrokeThickness, double.MaxValue };
            target.StrokeDashArray = result;
        }


    }
}
