using System;
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

        public ButtonChrome()
        {
            this.DefaultStyleKey = typeof(ButtonChrome);
        }

        private Ellipse _pointerOverElement;
        private bool _isPointerEntered;
        //private Point _lastPoint;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _pointerOverElement = GetTemplateChild(PointerOverElementName) as Ellipse;
            var converter = GetTemplateChild("EllipseStrokeDashArrayConverter") as EllipseStrokeDashArrayConverter;
            if (converter != null)
                converter.TargetEllipse = _pointerOverElement;

            UpdateVisualState(false);
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            if (_pointerOverElement == null)
                return;

            if (_pointerOverElement.StrokeThickness == 0)
                return;

            var rotateTransform = _pointerOverElement.RenderTransform as RotateTransform;
            if (rotateTransform == null)
                return;

            var point = e.GetCurrentPoint(this).Position;

            var centerPoint = new Point(this.ActualWidth / 2, this.ActualHeight / 2);

            double angleOfLine = Math.Atan2((point.Y - centerPoint.Y), (point.X - centerPoint.X)) * 180 / Math.PI;
            rotateTransform.Angle = angleOfLine + 180;
            //var offset = (-_pointerOverElement.ActualHeight - _pointerOverElement.ActualWidth) / _pointerOverElement.StrokeThickness;
            //_pointerOverElement.StrokeDashOffset = offset;
            _isPointerEntered = true;
            UpdateVisualState();
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
          
            _isPointerEntered = false;

            if (_pointerOverElement == null)
                return;

            if (_pointerOverElement.StrokeThickness == 0)
                return;

            var rotateTransform = _pointerOverElement.RenderTransform as RotateTransform;
            if (rotateTransform == null)
                return;

            var point = e.GetCurrentPoint(this).Position;

            var centerPoint = new Point(this.ActualWidth / 2, this.ActualHeight / 2);

            double angleOfLine = Math.Atan2((point.Y - centerPoint.Y), (point.X - centerPoint.X)) * 180 / Math.PI;
            rotateTransform.Angle = angleOfLine + 180;

            UpdateVisualState();
        }

        //protected override void OnPointerMoved(PointerRoutedEventArgs e)
        //{
        //    base.OnPointerMoved(e);
        //    _lastPoint = e.GetCurrentPoint(this).Position;
        //}


        protected virtual void UpdateVisualState(bool useTransitions = true)
        {
            if (_isPointerEntered)
                VisualStateManager.GoToState(this, PointerOverStateName, useTransitions);
            else
                VisualStateManager.GoToState(this, NormalStateName, useTransitions);

        }
    }
}
