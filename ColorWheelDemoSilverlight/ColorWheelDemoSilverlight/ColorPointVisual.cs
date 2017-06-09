using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Linq;

namespace ColorWheelDemoSilverlight
{
    [TemplatePart(Name = ThumbElementName, Type = typeof(Thumb))]
    public class ColorPointVisual : Control
    {
        private const string ThumbElementName = "ThumbElement";

        /// <summary>
        ///     标识 ColorPoint 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ColorPointProperty =
            DependencyProperty.Register("ColorPoint", typeof(ColorPoint), typeof(ColorPointVisual), new PropertyMetadata(null, OnColorPointChanged));

        /// <summary>
        ///     标识 DragStartedCommand 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DragStartedCommandProperty =
            DependencyProperty.Register("DragStartedCommand", typeof(ICommand), typeof(ColorPointVisual), new PropertyMetadata(null));

        /// <summary>
        ///     标识 DragDeltaCommand 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DragDeltaCommandProperty =
            DependencyProperty.Register("DragDeltaCommand", typeof(ICommand), typeof(ColorPointVisual), new PropertyMetadata(null));

        private Thumb _thumbElement;
        private bool _hasMouseCaptured;
        private Point _lastPoint;
        private UIElement _visualParent;

        public ColorPointVisual()
        {
            DefaultStyleKey = typeof(ColorPointVisual);

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_hasMouseCaptured == false)
                return;

            var point = e.GetPosition(_visualParent);
           
            if (DragDeltaCommand != null && DragDeltaCommand.CanExecute(null))
            {
                var translation = new Point(point.X - _lastPoint.X, point.Y - _lastPoint.Y);
                var parameter = new ColorPointVisualDragDeltaParameter(this, translation);
                DragDeltaCommand.Execute(parameter);
            }
            _lastPoint = point;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            _visualParent = this.GetVisualAncestors().OfType<UIElement>().FirstOrDefault();
            if (_visualParent == null)
                return;

            _hasMouseCaptured = CaptureMouse();
            if (_hasMouseCaptured == false)
                return;

            _lastPoint = e.GetPosition(_visualParent);
            if (DragStartedCommand != null && DragStartedCommand.CanExecute(null))
            {
                var point = new Point(_lastPoint.X, _lastPoint.Y);
                var parameter = new ColorPointVisualDragStartedParameter(this, point);
                DragStartedCommand.Execute(parameter);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            ReleaseMouseCapture();
            _hasMouseCaptured = false;
        }


        /// <summary>
        ///     获取或设置ColorPoint的值
        /// </summary>
        public ColorPoint ColorPoint
        {
            get { return (ColorPoint)GetValue(ColorPointProperty); }
            set { SetValue(ColorPointProperty, value); }
        }


        /// <summary>
        ///     获取或设置DragStartedCommand的值
        /// </summary>
        public ICommand DragStartedCommand
        {
            get { return (ICommand)GetValue(DragStartedCommandProperty); }
            set { SetValue(DragStartedCommandProperty, value); }
        }


        /// <summary>
        ///     获取或设置DragDeltaCommand的值
        /// </summary>
        public ICommand DragDeltaCommand
        {
            get { return (ICommand)GetValue(DragDeltaCommandProperty); }
            set { SetValue(DragDeltaCommandProperty, value); }
        }

        private static void OnColorPointChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as ColorPointVisual;
            var oldValue = (ColorPoint)args.OldValue;
            var newValue = (ColorPoint)args.NewValue;
            if (oldValue != newValue)
                target.OnColorPointChanged(oldValue, newValue);
        }

        protected virtual void OnColorPointChanged(ColorPoint oldValue, ColorPoint newValue)
        {
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _thumbElement = GetTemplateChild(ThumbElementName) as Thumb;
            if (_thumbElement != null)
            {
                _thumbElement.DragStarted += OnDragStarted;
                _thumbElement.DragDelta += OnDragDelta;
            }
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (DragDeltaCommand != null && DragDeltaCommand.CanExecute(null))
            {
                var point = new Point(e.HorizontalChange, e.VerticalChange);
                var parameter = new ColorPointVisualDragDeltaParameter(this, point);
                DragDeltaCommand.Execute(parameter);
            }
        }

        private void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            if (DragStartedCommand != null && DragStartedCommand.CanExecute(null))
            {
                var point = new Point(e.HorizontalOffset, e.VerticalOffset);
                var parameter = new ColorPointVisualDragStartedParameter(this, point);
                DragStartedCommand.Execute(parameter);
            }
        }
    }
}