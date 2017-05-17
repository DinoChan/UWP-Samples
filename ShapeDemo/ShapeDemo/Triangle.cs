using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ShapeDemo
{
    public class Triangle : Path
    {
        /// <summary>
        ///     标识 Direction 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(Direction), typeof(Triangle), new PropertyMetadata(Direction.Up, OnDirectionChanged));

        private bool _realizeGeometryScheduled;
        private Size _orginalSize;

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
            var target = obj as Triangle;
            var oldValue = (Direction)args.OldValue;
            var newValue = (Direction)args.NewValue;
            if (oldValue != newValue)
                target.OnDirectionChanged(oldValue, newValue);
        }

        protected virtual void OnDirectionChanged(Direction oldValue, Direction newValue)
        {
            InvalidateGeometry();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_realizeGeometryScheduled == false && _orginalSize != finalSize)
            {
                _realizeGeometryScheduled = true;
                LayoutUpdated += OnTriangleLayoutUpdated;
                _orginalSize = finalSize;
            }
            base.ArrangeOverride(finalSize);
            return finalSize;
        }

        /// <summary>提供 Silverlight 布局传递的“度量值”部分的行为。类可以替代此方法以定义其自己的“度量值”传递行为。</summary>
        /// <returns>此对象根据其子对象分配大小的计算或者可能根据其他注意事项（如固定容器大小）确定的在布局过程中所需的大小。</returns>
        /// <param name="availableSize">此对象可以提供给子对象的可用大小。可以将值指定为无穷大 (<see cref="F:System.Double.PositiveInfinity" />)，以指明对象大小将调整为可用的任何内容的大小。</param>
        /// <remarks>
        /// 在 WPF 中，测量值替代利用 Shape.DefiningGeometry 进行工作，Shape.DefiningGeometry 并不始终跟预计的一样。有关详细信息，请参阅错误 99497，其中 WPF 默认情况下没有正确的测量值。
        /// 
        /// 在 Silverlight 中，路径上的测量值替代的工作方式与原始形状的工作方式不相同。
        /// 
        /// 返回的应该是此形状无需剪辑便可正确呈现的最小尺寸。默认情况下，呈现的形状可以小到一个点，因此会返回笔划粗细。
        ///             </remarks>
        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(base.StrokeThickness, base.StrokeThickness);
        }

        public void InvalidateGeometry()
        {
            InvalidateArrange();
            if (_realizeGeometryScheduled == false)
            {
                _realizeGeometryScheduled = true;
                LayoutUpdated += OnTriangleLayoutUpdated;
            }
        }

        private void OnTriangleLayoutUpdated(object sender, object e)
        {
            _realizeGeometryScheduled = false;
            LayoutUpdated -= OnTriangleLayoutUpdated;
            RealizeGeometry();
        }

        private void RealizeGeometry()
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
                    figure.StartPoint = new Point(ActualWidth / 2, 0);
                    //segment = new LineSegment { Point = new Point(ActualWidth / 2, 0) };
                    //figure.Segments.Add(segment);
                    segment = new LineSegment { Point = new Point(ActualWidth, ActualHeight) };
                    figure.Segments.Add(segment);
                    segment = new LineSegment { Point = new Point(0, ActualHeight) };
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
            Data = geometry;
        }

    }
}
