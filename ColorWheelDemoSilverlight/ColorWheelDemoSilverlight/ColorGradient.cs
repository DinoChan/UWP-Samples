using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ColorWheelDemoSilverlight
{
    public class ColorGradient : Control
    {
        private const string ValueIndicatorElementName = "ValueIndicatorElement";

        /// <summary>
        ///     标识 Color 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorGradient), new PropertyMetadata(default(Color), OnColorChanged));

        /// <summary>
        ///     标识 Maximum 依赖属性。
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(ColorGradient), new PropertyMetadata(0d, OnMaximumChanged));

        /// <summary>
        ///     标识 Minimum 依赖属性。
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(ColorGradient), new PropertyMetadata(0d, OnMinimumChanged));

        /// <summary>
        ///     标识 ColorConverter 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ColorConverterProperty =
            DependencyProperty.Register("ColorConverter", typeof(IColorConverter), typeof(ColorGradient), new PropertyMetadata(null, OnColorConverterChanged));

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
        ///     获取或设置Color的值
        /// </summary>
        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }


        /// <summary>
        ///     获取或设置Maximum的值
        /// </summary>
        public double Maximum
        {
            get { return (double) GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }


        /// <summary>
        ///     获取或设置Minimum的值
        /// </summary>
        public double Minimum
        {
            get { return (double) GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }


        /// <summary>
        ///     获取或设置ColorConverter的值
        /// </summary>
        public IColorConverter ColorConverter
        {
            get { return (IColorConverter) GetValue(ColorConverterProperty); }
            set { SetValue(ColorConverterProperty, value); }
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

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as ColorGradient;
            var oldValue = (Color) args.OldValue;
            var newValue = (Color) args.NewValue;
            if (oldValue != newValue)
                target.OnColorChanged(oldValue, newValue);
        }

        protected virtual void OnColorChanged(Color oldValue, Color newValue)
        {
            UpdateVisual();
        }

        private static void OnMaximumChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as ColorGradient;
            var oldValue = (double) args.OldValue;
            var newValue = (double) args.NewValue;
            if (oldValue != newValue)
                target.OnMaximumChanged(oldValue, newValue);
        }

        protected virtual void OnMaximumChanged(double oldValue, double newValue)
        {
            UpdateVisual();
        }

        private static void OnMinimumChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as ColorGradient;
            var oldValue = (double) args.OldValue;
            var newValue = (double) args.NewValue;
            if (oldValue != newValue)
                target.OnMinimumChanged(oldValue, newValue);
        }

        protected virtual void OnMinimumChanged(double oldValue, double newValue)
        {
            UpdateVisual();
        }

        private static void OnColorConverterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as ColorGradient;
            var oldValue = (IColorConverter) args.OldValue;
            var newValue = (IColorConverter) args.NewValue; 
            if (oldValue != newValue)
                target.OnColorConverterChanged(oldValue, newValue);
        }

        protected virtual void OnColorConverterChanged(IColorConverter oldValue, IColorConverter newValue)
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
           var v= ReadLocalValue(ColorProperty);
        }
    }
}