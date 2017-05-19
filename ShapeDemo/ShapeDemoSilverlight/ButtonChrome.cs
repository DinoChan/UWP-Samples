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

namespace ShapeDemoSilverlight
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
        private Point _lastPoint;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _pointerOverElement = GetTemplateChild(PointerOverElementName) as Ellipse;
            UpdateVisualState(false);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (_pointerOverElement == null)
                return;

            if (_pointerOverElement.StrokeThickness == 0)
                return;

            var rotateTransform = _pointerOverElement.RenderTransform as RotateTransform;
            if (rotateTransform == null)
                return;

            var point = e.GetPosition(this);

            var centerPoint = new Point(this.ActualWidth / 2, this.ActualHeight / 2);

            double angleOfLine = Math.Atan2((point.Y - centerPoint.Y), (point.X - centerPoint.X)) * 180 / Math.PI;
            rotateTransform.Angle = angleOfLine + 180;
            //var offset = (-_pointerOverElement.ActualHeight - _pointerOverElement.ActualWidth) / _pointerOverElement.StrokeThickness;
            //_pointerOverElement.StrokeDashOffset = offset;
            _isPointerEntered = true;
            UpdateVisualState();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _isPointerEntered = false;

            if (_pointerOverElement == null)
                return;

            if (_pointerOverElement.StrokeThickness == 0)
                return;

            var rotateTransform = _pointerOverElement.RenderTransform as RotateTransform;
            if (rotateTransform == null)
                return;

            var point = _lastPoint;

            var centerPoint = new Point(this.ActualWidth / 2, this.ActualHeight / 2);

            double angleOfLine = Math.Atan2((point.Y - centerPoint.Y), (point.X - centerPoint.X)) * 180 / Math.PI;
            rotateTransform.Angle = angleOfLine + 180;

            UpdateVisualState();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _lastPoint = e.GetPosition(this);
        }

        protected virtual void UpdateVisualState(bool useTransitions = true)
        {
            if (_isPointerEntered)
                VisualStateManager.GoToState(this, PointerOverStateName, useTransitions);
            else
                VisualStateManager.GoToState(this, NormalStateName, useTransitions);

        }
    }
}