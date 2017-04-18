using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PathDemoSilverlight
{
    public class Triangle2 : Shape
    {
        /// <summary>
        ///     标识 Direction 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(Direction), typeof(Triangle2), new PropertyMetadata(Direction.Up, OnDirectionChanged));

        private bool _realizeGeometryScheduled;
        private Size _orginalSize;
        private Direction _orginalDirection;

        /// <summary>
        ///     获取或设置Direction的值
        /// </summary>
        public Direction Direction
        {
            get { return (Direction)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        private static void OnDirectionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as Triangle2;
            var oldValue = (Direction)args.OldValue;
            var newValue = (Direction)args.NewValue;
            if (oldValue != newValue)
                target.OnDirectionChanged(oldValue, newValue);
        }

        protected virtual void OnDirectionChanged(Direction oldValue, Direction newValue)
        {
            InvalidateArrange();
        }

        public override Transform GeometryTransform
        {
            get
            {
                var geometry = new PathGeometry();
                var figure = new PathFigure { IsClosed = true };
                geometry.Figures.Add(figure);
                switch (Direction)
                {
                    case Direction.Left:
                        figure.StartPoint = new Point(ActualWidth, 0);
                        var segment = new LineSegment { Point = new Point(ActualWidth, ActualHeight) };
                        figure.Segments.Add(segment);
                        segment = new LineSegment { Point = new Point(0, ActualHeight / 2) };
                        figure.Segments.Add(segment);
                        break;
                    case Direction.Up:
                        figure.StartPoint = new Point(0, ActualHeight);
                        segment = new LineSegment { Point = new Point(ActualWidth / 2, 0) };
                        figure.Segments.Add(segment);
                        segment = new LineSegment { Point = new Point(ActualWidth, ActualHeight) };
                        figure.Segments.Add(segment);
                        break;
                    case Direction.Right:
                        figure.StartPoint = new Point(0, 0);
                        segment = new LineSegment { Point = new Point(ActualWidth, ActualHeight / 2) };
                        figure.Segments.Add(segment);
                        segment = new LineSegment { Point = new Point(0, ActualHeight) };
                        figure.Segments.Add(segment);
                        break;
                    case Direction.Down:
                        figure.StartPoint = new Point(0, 0);
                        segment = new LineSegment { Point = new Point(ActualWidth, 0) };
                        figure.Segments.Add(segment);
                        segment = new LineSegment { Point = new Point(ActualWidth / 2, ActualHeight) };
                        figure.Segments.Add(segment);
                        break;
                }
                return geometry.Transform;
            }
        }

    }
}
