using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace TemplatedControlSample
{
    [TemplateVisualState(Name = NormalState, GroupName = CommonStates)]
    [TemplateVisualState(Name = PointerOverState, GroupName = CommonStates)]
    [TemplateVisualState(Name = NoHeaderState, GroupName = HeaderStates)]
    [TemplateVisualState(Name = HasHeaderState, GroupName = HeaderStates)]
    public class ContentView2 : HeaderedContentControl
    {
        public const string CommonStates = "CommonStates";
        public const string NormalState = "Normal";
        public const string PointerOverState = "PointerOver";

        public const string HeaderStates = "HeaderStates";
        public const string NoHeaderState = "NoHeader";
        public const string HasHeaderState = "HasHeader";

        public ContentView2()
        {
            this.DefaultStyleKey = typeof(ContentView2);
        }

        private bool _isPointerEntered;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateVisualState(false);
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            _isPointerEntered = true;
            UpdateVisualState();
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            _isPointerEntered = false;
            UpdateVisualState();
        }

        protected override void OnHeaderChanged(object oldValue, object newValue)
        {
            base.OnHeaderChanged(oldValue, newValue);
            UpdateVisualState();
        }

        internal virtual void UpdateVisualState(bool useTransitions = true)
        {
            if (_isPointerEntered)
                VisualStateManager.GoToState(this, PointerOverState, useTransitions);
            else
                VisualStateManager.GoToState(this, NormalState, useTransitions);

            if (Header == null)
                VisualStateManager.GoToState(this, NoHeaderState, useTransitions);
            else
                VisualStateManager.GoToState(this, HasHeaderState, useTransitions);
        }
    }
}