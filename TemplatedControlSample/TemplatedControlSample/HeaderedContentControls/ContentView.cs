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

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace TemplatedControlSample
{
    [TemplatePart(Name = HeaderPartName, Type = typeof(FrameworkElement))]
    public sealed class ContentView : HeaderedContentControl
    {
        public const string HeaderPartName = "HeaderContentPresenter";

        public ContentView()
        {
            this.DefaultStyleKey = typeof(ContentView);
        }

        private FrameworkElement _headerPart;
        private bool _isPointerEntered;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _headerPart = GetTemplateChild(HeaderPartName) as FrameworkElement;
            UpdateHeaderVisual();
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            _isPointerEntered = true;
            UpdateHeaderVisual();
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            _isPointerEntered = false;
            UpdateHeaderVisual();
        }

        protected override void OnHeaderChanged(object oldValue, object newValue)
        {
            base.OnHeaderChanged(oldValue, newValue);
            UpdateHeaderVisual();
        }

        private void UpdateHeaderVisual()
        {
            if (_headerPart == null)
                return;

            if (_isPointerEntered)
                _headerPart.Opacity = 1;
            else
                _headerPart.Opacity = 0.7;

            if (Header == null)
                _headerPart.Visibility = Visibility.Collapsed;
            else
                _headerPart.Visibility = Visibility.Visible;
        }
    }
}
