﻿using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ColorWheelDemoSilverlight
{
    public class HsvWheelColorPalette : ColorPalette
    {
        private Point _dragOrginalPosition;


        public HsvWheelColorPalette()
        {
            DefaultStyleKey = typeof(HsvWheelColorPalette);
        }

        protected override void OnColorPointVisualDragStarted(ColorPointVisual colorPointVisual, Point position)
        {
            base.OnColorPointVisualDragStarted(colorPointVisual, position);
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
            var distance = Math.Sqrt(Math.Pow(centerPoint.X - x, 2) + Math.Pow(centerPoint.Y - y, 2));
            var saturation = distance / radius;
            saturation = Math.Min(1, saturation);


            var distanceOfX = x - centerPoint.X;
            var distanceOfY = y - centerPoint.Y;

            var theta = Math.Atan2(distanceOfY, distanceOfX);

            if (theta < 0)
                theta += 2 * Math.PI;

            var hue = (int) (theta / (Math.PI * 2) * 360.0);
            var color = ColorHelper.FromHsv(hue, saturation, 1);
            return color;
        }
    }
}