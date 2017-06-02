using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ShapeDemo
{
    [TemplateVisualState(GroupName = CommonStatesName, Name = NormalStateName)]
    [TemplateVisualState(GroupName = CommonStatesName, Name = PointerOverStateName)]
    [TemplatePart(Name = PointerOverElementName, Type = typeof(Ellipse))]
    public class ButtonChrome : Control
    {
        private const string PointerOverElementName = "PointerOverElement";
        private const string CommonStatesName = "CommonStates";
        private const string NormalStateName = "Normal";
        private const string PointerOverStateName = "PointerOver";

        private bool _isPointerEntered;
        private Ellipse _pointerOverElement;

        public ButtonChrome()
        {
            DefaultStyleKey = typeof(ButtonChrome);
        }


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _pointerOverElement = GetTemplateChild(PointerOverElementName) as Ellipse;
            UpdateVisualState(false);
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            _isPointerEntered = true;
            UpdateAngle(e);
            UpdateVisualState();
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            _isPointerEntered = false;
            UpdateAngle(e);
            UpdateVisualState();
        }

        protected virtual void UpdateVisualState(bool useTransitions = true)
        {
            if (_isPointerEntered)
                VisualStateManager.GoToState(this, PointerOverStateName, useTransitions);
            else
                VisualStateManager.GoToState(this, NormalStateName, useTransitions);
        }

        private void UpdateAngle(PointerRoutedEventArgs e)
        {
            if (_pointerOverElement == null)
                return;

            if (_pointerOverElement.StrokeThickness == 0)
                return;

            var rotateTransform = _pointerOverElement.RenderTransform as RotateTransform;
            if (rotateTransform == null)
                return;

            var point = e.GetCurrentPoint(this).Position;
            var centerPoint = new Point(ActualWidth / 2, ActualHeight / 2);
            var angleOfLine = Math.Atan2(point.Y - centerPoint.Y, point.X - centerPoint.X) * 180 / Math.PI;
            rotateTransform.Angle = angleOfLine + 180;
        }
    }
}