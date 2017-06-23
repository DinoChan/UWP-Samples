using System.Windows;
using System.Windows.Controls;

namespace ExpanderSampleSilverlight
{
    [TemplatePart(Name = ElementContentSiteName, Type = typeof(ContentPresenter))]
    public class ExpandableContentControl : ContentControl
    {
        private double _contentHeight;


        public ExpandableContentControl()
        {
            DefaultStyleKey = typeof(ExpandableContentControl);
        }


        public override void OnApplyTemplate()
        {
            ContentSite = GetTemplateChild(ElementContentSiteName) as ContentPresenter;
        }

        #region TemplateParts

        private const string ElementContentSiteName = "ContentSite";


        private ContentPresenter _contentSite;


        private ContentPresenter ContentSite
        {
            get { return _contentSite; }
            set
            {
                if (_contentSite != null)
                    _contentSite.SizeChanged -= OnContentSiteSizeChanged;
                _contentSite = value;
                if (_contentSite != null)
                    _contentSite.SizeChanged += OnContentSiteSizeChanged;
            }
        }

        private void OnContentSiteSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Percentage >= 1)
                _contentHeight = e.NewSize.Height;
        }

        #endregion TemplateParts


        #region public double Percentage

        /// <summary>
        ///     Gets or sets the relative percentage of the content that is
        ///     currently visible. A percentage of 1 corresponds to the complete
        ///     TargetSize.
        /// </summary>
        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }


        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register(
                "Percentage",
                typeof(double),
                typeof(ExpandableContentControl),
                new PropertyMetadata(0.0, OnPercentagePropertyChanged));


        private static void OnPercentagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (ExpandableContentControl)d;
            source.OnPercentagePropertyChanged();
        }

        private void OnPercentagePropertyChanged()
        {
            if (ContentSite == null)
                return;

            if (Percentage >= 1)
                ContentSite.MaxHeight = double.MaxValue;
            else
                ContentSite.MaxHeight = _contentHeight * Percentage;
        }

        #endregion public double Percentage
    }
}