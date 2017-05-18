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
    [TemplatePart(Name = PointerOverElementName, Type = typeof(Rectangle))]
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

        private Rectangle _pointerOverElement;
        private bool _isPointerEntered;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _pointerOverElement = GetTemplateChild(PointerOverElementName) as Rectangle;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (_pointerOverElement == null)
                return;

            if (_pointerOverElement.StrokeThickness == 0)
                return;

            var point = e.GetPosition(this);
            
            var offset = (-_pointerOverElement.ActualHeight - _pointerOverElement.ActualWidth)/_pointerOverElement.StrokeThickness;

            _pointerOverElement.StrokeDashOffset = offset;
            _isPointerEntered = true;
            UpdateVisualState();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _isPointerEntered = false;
            UpdateVisualState();
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