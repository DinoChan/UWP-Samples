using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ColorWheelDemoSilverlight
{
    public class ColorPoint : DependencyObject
    {

        /// <summary>
        /// 获取或设置Color的值
        /// </summary>  
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// 标识 Color 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorPoint), new PropertyMetadata(default(Color), OnColorChanged));

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ColorPoint target = obj as ColorPoint;
            Color oldValue = (Color)args.OldValue;
            Color newValue = (Color)args.NewValue;
            if (oldValue != newValue)
                target.OnColorChanged(oldValue, newValue);
        }

        protected virtual void OnColorChanged(Color oldValue, Color newValue)
        {

        }
    }
}
