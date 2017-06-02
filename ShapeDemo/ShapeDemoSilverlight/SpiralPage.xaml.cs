using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;

namespace ShapeDemoSilverlight
{
    public partial class SpiralPage : UserControl
    {


        public SpiralPage()
        {
            InitializeComponent();
            var points = GetPoints();
            Polyline.Points = GetPoints();
            PolylineBack.Points = GetPoints();
            Loaded += SpiralPage_Loaded;
        }

        private void SpiralPage_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard1.Begin();
        }

        private PointCollection GetPoints()
        {
            var width = 200;
            var height = 200;
            var graphToCanvasX = width / 50;
            var graphToCanvasY = height / 50;

            // distance from origin of graph to origin of canvas
            var offsetX = 30;
            var offsetY = 30;

            var points = new PointCollection();
            for (var t = 0d; t <= 31.4; t += Math.PI / 16)
            {
                var xGraph = Math.Sin(t) * t;
                var yGraph = Math.Cos(t) * t;

                // Translate the origin based on the max/min parameters (y axis is flipped), then scale to canvas.
                var x = (xGraph + offsetX) * graphToCanvasX;
                var y = (offsetY - yGraph) * graphToCanvasY;

                points.Add(new Point(x, y));
            }
            return points;
        }


    }

    public class PolylineProgressBehavior : Behavior<Polyline>
    {

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
            DependencyProperty.Register("Progress", typeof(double), typeof(PolylineProgressBehavior), new PropertyMetadata(0d, OnProgressChanged));

        private static void OnProgressChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PolylineProgressBehavior target = obj as PolylineProgressBehavior;
            double oldValue = (double)args.OldValue;
            double newValue = (double)args.NewValue;
            if (oldValue != newValue)
                target.OnProgressChanged(oldValue, newValue);
        }


        protected virtual void OnProgressChanged(double oldValue, double newValue)
        {
            UpdateStrokeDashArray();
        }

        protected virtual double GetTotalLength()
        {
            var totalLength = 0d;
            var point = AssociatedObject.Points[0];
            foreach (var item in AssociatedObject.Points.Skip(1))
            {
                totalLength += Math.Sqrt(Math.Pow(point.X - item.X, 2) + Math.Pow(point.Y - item.Y, 2));
                point = item;
            }

            return totalLength;
        }


        private void UpdateStrokeDashArray()
        {
            if (AssociatedObject == null || AssociatedObject.Points == null || AssociatedObject.StrokeThickness == 0)
                return;


            var totalLength = GetTotalLength();
            var result = new DoubleCollection { Progress * totalLength / 100 / AssociatedObject.StrokeThickness, double.MaxValue };
            AssociatedObject.StrokeDashArray = result;
        }


    }
}