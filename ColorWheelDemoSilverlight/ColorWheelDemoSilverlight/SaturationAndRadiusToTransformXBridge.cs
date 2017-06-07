using System.Windows;

namespace ColorWheelDemoSilverlight
{
    public class SaturationAndRadiusToTransformXBridge : DependencyObject
    {
        /// <summary>
        ///     标识 Radius 依赖属性。
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(SaturationAndRadiusToTransformXBridge), new PropertyMetadata(0, OnRadiusChanged));

        /// <summary>
        ///     标识 Saturation 依赖属性。
        /// </summary>
        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register("Saturation", typeof(double), typeof(SaturationAndRadiusToTransformXBridge), new PropertyMetadata(0, OnSaturationChanged));

        /// <summary>
        ///     标识 TransformX 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TransformXProperty =
            DependencyProperty.Register("TransformX", typeof(double), typeof(SaturationAndRadiusToTransformXBridge), new PropertyMetadata(0d));

        /// <summary>
        ///     获取或设置Radius的值
        /// </summary>
        public double Radius
        {
            get { return (double) GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }


        /// <summary>
        ///     获取或设置Saturation的值
        /// </summary>
        public double Saturation
        {
            get { return (double) GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }


        /// <summary>
        ///     获取或设置TransformX的值
        /// </summary>
        public double TransformX
        {
            get { return (double) GetValue(TransformXProperty); }
            set { SetValue(TransformXProperty, value); }
        }

        private static void OnRadiusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as SaturationAndRadiusToTransformXBridge;
            var oldValue = (double) args.OldValue;
            var newValue = (double) args.NewValue;
            if (oldValue != newValue)
                target.OnRadiusChanged(oldValue, newValue);
        }

        protected virtual void OnRadiusChanged(double oldValue, double newValue)
        {
            UpdateTransformX();
        }

        private static void OnSaturationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as SaturationAndRadiusToTransformXBridge;
            var oldValue = (double) args.OldValue;
            var newValue = (double) args.NewValue;
            if (oldValue != newValue)
                target.OnSaturationChanged(oldValue, newValue);
        }

        protected virtual void OnSaturationChanged(double oldValue, double newValue)
        {
            UpdateTransformX();
        }


        private void UpdateTransformX()
        {
            TransformX = Saturation * Radius / 100;
        }
    }
}