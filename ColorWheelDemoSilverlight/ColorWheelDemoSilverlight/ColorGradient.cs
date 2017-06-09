using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ColorWheelDemoSilverlight
{
    public class ColorGradient : ColorValueEditor
    {
        private const string ValueIndicatorElementName = "ValueIndicatorElement";

        /// <summary>
        ///     标识 MaximumColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty MaximumColorProperty =
            DependencyProperty.Register("MaximumColor", typeof(Color), typeof(ColorGradient), new PropertyMetadata(default(Color)));

        /// <summary>
        ///     标识 MinimumColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty MinimumColorProperty =
            DependencyProperty.Register("MinimumColor", typeof(Color), typeof(ColorGradient), new PropertyMetadata(default(Color)));

        public ColorGradient()
        {
            DefaultStyleKey = typeof(ColorGradient);
        }


        /// <summary>
        ///     获取或设置MaximumColor的值
        /// </summary>
        public Color MaximumColor
        {
            get { return (Color) GetValue(MaximumColorProperty); }
            set { SetValue(MaximumColorProperty, value); }
        }

        /// <summary>
        ///     获取或设置MinimumColor的值
        /// </summary>
        public Color MinimumColor
        {
            get { return (Color) GetValue(MinimumColorProperty); }
            set { SetValue(MinimumColorProperty, value); }
        }


        protected override void OnColorChanged(Color oldValue, Color newValue)
        {
            UpdateVisual();
        }


        protected override void OnMaximumChanged(double oldValue, double newValue)
        {
            UpdateVisual();
        }


        protected override void OnMinimumChanged(double oldValue, double newValue)
        {
            UpdateVisual();
        }


        protected override void OnColorConverterChanged(IColorConverter oldValue, IColorConverter newValue)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (ColorConverter == null)
                return;

            MinimumColor = ColorConverter.ToColor(Color, Minimum);
            MaximumColor = ColorConverter.ToColor(Color, Maximum);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }
    }
}