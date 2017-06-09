using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace ColorWheelDemoSilverlight
{

    public class HsvWheelColorPicker : ColorPicker
    {


        public HsvWheelColorPicker()
        {
            this.DefaultStyleKey = typeof(HsvWheelColorPicker);
        }

        private Point _dragOrginalPosition;

        protected override void OnColorPointVisualDragStarted(ColorPointVisual colorPointVisual, Point position)
        {
            base.OnColorPointVisualDragStarted(colorPointVisual, position);
            var transform = colorPointVisual.TransformToVisual(this);
            var bounds = colorPointVisual.GetBoundsRelativeTo(this);
            if (bounds == null)
                return;

            _dragOrginalPosition = new Point(bounds.Value.X + bounds.Value.Width / 2, bounds.Value.Y + bounds.Value.Height / 2);
        }

        protected override void OnColorPointVisualDragDelta(ColorPointVisual colorPointVisual, Point position)
        {
            base.OnColorPointVisualDragDelta(colorPointVisual, position);
            _dragOrginalPosition = new Point(_dragOrginalPosition.X + position.X, _dragOrginalPosition.Y + position.Y);
            colorPointVisual.ColorPoint.Color = GetColor(_dragOrginalPosition);
        }


        private Color GetColor(Point point)
        {
            var centerPoint = new Point(ActualWidth / 2, ActualHeight / 2);
            var diameter = ActualWidth < ActualHeight ? ActualWidth : ActualHeight;
            var radius = diameter / 2;

            var x = point.X;
            var y = point.Y;
            double distance = Math.Sqrt(Math.Pow(centerPoint.X - x, 2) + Math.Pow(centerPoint.Y - y, 2));
            var saturation = distance / radius;
            saturation = Math.Min(1, saturation);


            //var deltaX = x * x - radius * radius;
            //var deltaY = y * y - radius * radius;
            var cx = x - centerPoint.X;
            var cy = y - centerPoint.Y;

            double theta = Math.Atan2(cy, cx);

            if (theta < 0)
            {
                theta += 2 * Math.PI;
            }

            //double alpha = Math.Sqrt((cx * cx) + (cy * cy));

            var hue = (int)((theta / (Math.PI * 2)) * 360.0);
            var color = ColorHelper.FromHsv(hue, saturation, 1);


            return color;


        }
    }
}
