﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace PathDemoUWwp
{
    [TemplatePart(Name = PathElementName, Type = typeof(Path))]
    [StyleTypedProperty(Property = nameof(PathElementStyle), StyleTargetType = typeof(Path))]
    public class TriangleControl : Control
    {
        private const string PathElementName = "PathElement";


        public TriangleControl()
        {
            this.DefaultStyleKey = typeof(TriangleControl);
            this.SizeChanged += OnTriangleControlSizeChanged;
        }


        /// <summary>
        ///     标识 Direction 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(Direction), typeof(TriangleControl), new PropertyMetadata(Direction.Up, OnDirectionChanged));

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
            var target = obj as TriangleControl;
            var oldValue = (Direction)args.OldValue;
            var newValue = (Direction)args.NewValue;
            if (oldValue != newValue)
                target.OnDirectionChanged(oldValue, newValue);
        }

        protected virtual void OnDirectionChanged(Direction oldValue, Direction newValue)
        {
            UpdateShape();
        }

        /// <summary>
        /// 获取或设置PathElementStyle的值
        /// </summary>  
        public Style PathElementStyle
        {
            get { return (Style)GetValue(PathElementStyleProperty); }
            set { SetValue(PathElementStyleProperty, value); }
        }

        /// <summary>
        /// 标识 PathElementStyle 依赖属性。
        /// </summary>
        public static readonly DependencyProperty PathElementStyleProperty =
            DependencyProperty.Register("PathElementStyle", typeof(Style), typeof(TriangleControl), new PropertyMetadata(null));


        private Path _pathElement;

        protected  override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _pathElement = GetTemplateChild("PathElement") as Path;
        }


        private void OnTriangleControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateShape();
        }

        private void UpdateShape()
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
            _pathElement.Data = geometry;
        }
    }
}
