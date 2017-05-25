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

namespace PointerDemo
{
    [TemplatePart(Name = LineElementName, Type = typeof(Line))]
    [TemplatePart(Name = ArrowElementName, Type = typeof(Path))]
    public class LineArrow : Control
    {
        private const string LineElementName = "LineElement";
        private const string ArrowElementName = "ArrowElement";

        /// <summary>
        /// 获取或设置StrokeThickness的值
        /// </summary>  
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// 标识 StrokeThickness 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(LineArrow), new PropertyMetadata(0d, OnStrokeThicknessChanged));

        private static void OnStrokeThicknessChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            LineArrow target = obj as LineArrow;
            double oldValue = (double)args.OldValue;
            double newValue = (double)args.NewValue;
            if (oldValue != newValue)
                target.OnStrokeThicknessChanged(oldValue, newValue);
        }

        protected virtual void OnStrokeThicknessChanged(double oldValue, double newValue)
        {
            UpdateShape();
        }


        /// <summary>
        /// 获取或设置StartCorner的值
        /// </summary>  
        public CornerType StartCorner
        {
            get { return (CornerType)GetValue(StartCornerProperty); }
            set { SetValue(StartCornerProperty, value); }
        }

        /// <summary>
        /// 标识 StartCorner 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StartCornerProperty =
            DependencyProperty.Register("StartCorner", typeof(CornerType), typeof(LineArrow), new PropertyMetadata(CornerType.TopLeft, OnStartCornerChanged));

        private static void OnStartCornerChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            LineArrow target = obj as LineArrow;
            CornerType oldValue = (CornerType)args.OldValue;
            CornerType newValue = (CornerType)args.NewValue;
            if (oldValue != newValue)
                target.OnStartCornerChanged(oldValue, newValue);
        }

        protected virtual void OnStartCornerChanged(CornerType oldValue, CornerType newValue)
        {
            UpdateShape();
        }

        private Line _lineElement;
        private Path _arrowElement;

        public LineArrow()
        {
            this.DefaultStyleKey = typeof(LineArrow);
            SizeChanged += OnLineArrowSizeChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _lineElement = GetTemplateChild(LineElementName) as Line;
            _arrowElement = GetTemplateChild(ArrowElementName) as Path;
        }

        private void OnLineArrowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateShape();
        }

        private void UpdateShape()
        {
            if (ActualWidth == 0 || ActualHeight == 0)
                return;

            UpdateLine();
            UpdateArrow();
        }

        //protected override Size MeasureOverride(Size availableSize)
        //{
        //    return base.MeasureOverride(new Size(this.StrokeThickness, this.StrokeThickness));
        //}

        private void UpdateLine()
        {
            if (_lineElement == null)
                return;
            
            double x1 = 0;
            double x2 = 0;
            double y1 = 0;
            double y2 = 0;
            double startOffset = StrokeThickness / 2;
            double endOffset = StrokeThickness;
            switch (StartCorner)
            {
                case CornerType.TopLeft:
                    x1 = startOffset;
                    y1 = startOffset;
                    x2 = ActualWidth - endOffset;
                    y2 = ActualHeight - endOffset;
                    break;
                case CornerType.TopRight:
                    x1 = ActualWidth - startOffset;
                    y1 = startOffset;
                    x2 = endOffset;
                    y2 = ActualHeight - endOffset;
                    break;
                case CornerType.BottomRight:
                    x1 = ActualWidth - startOffset;
                    y1 = ActualHeight - startOffset;
                    x2 = endOffset;
                    y2 = endOffset;
                    break;
                case CornerType.BottomLeft:
                    x1 = startOffset;
                    y1 = ActualHeight - startOffset;
                    x2 = ActualWidth - endOffset;
                    y2 = endOffset;
                    break;
            }
            _lineElement.X1 = x1;
            _lineElement.Y1 = y1;
            _lineElement.X2 = x2;
            _lineElement.Y2 = y2;
        }

        private void UpdateArrow()
        {
            if (_arrowElement == null)
                return;


            var rotation = 0d;
            var horizontalAlignment = HorizontalAlignment.Left;
            var verticalAlignment = VerticalAlignment.Top;
            switch (StartCorner)
            {
                case CornerType.TopLeft:
                    horizontalAlignment = HorizontalAlignment.Right;
                    verticalAlignment = VerticalAlignment.Bottom;
                    rotation=90+ Math.Atan( ActualHeight/ ActualWidth) * 180 / Math.PI;
                    break;
                case CornerType.TopRight:
                    horizontalAlignment = HorizontalAlignment.Left;
                    verticalAlignment = VerticalAlignment.Bottom;
                    rotation = 180+ Math.Atan(  ActualWidth/ ActualHeight) * 180 / Math.PI;
                    break;
                case CornerType.BottomRight:
                    horizontalAlignment = HorizontalAlignment.Left;
                    verticalAlignment = VerticalAlignment.Top;
                    rotation = 270 + Math.Atan(ActualHeight / ActualWidth) * 180 / Math.PI;
                    break;
                case CornerType.BottomLeft:
                    horizontalAlignment = HorizontalAlignment.Right;
                    verticalAlignment = VerticalAlignment.Top;
                    rotation =  Math.Atan(ActualWidth/ ActualHeight) * 180 / Math.PI;
                    break;
                default:
                    break;
            }


            var transform = _arrowElement.RenderTransform as CompositeTransform;
            transform.Rotation = rotation;
            _arrowElement.HorizontalAlignment = horizontalAlignment;
            _arrowElement.VerticalAlignment = verticalAlignment;
            _arrowElement.Height = StrokeThickness*2;
            _arrowElement.Width = StrokeThickness*2;

        }
    }
}
