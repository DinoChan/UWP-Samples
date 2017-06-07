using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ColorWheelDemoSilverlight
{
    public class ColorPointVisual : Control
    {
        public ColorPointVisual()
        {
            this.DefaultStyleKey = typeof(ColorPointVisual);
        }


        /// <summary>
        /// 获取或设置ColorPoint的值
        /// </summary>  
        public ColorPoint ColorPoint
        {
            get { return (ColorPoint)GetValue(ColorPointProperty); }
            set { SetValue(ColorPointProperty, value); }
        }

        /// <summary>
        /// 标识 ColorPoint 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ColorPointProperty =
            DependencyProperty.Register("ColorPoint", typeof(ColorPoint), typeof(ColorPointVisual), new PropertyMetadata(null, OnColorPointChanged));

        private static void OnColorPointChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ColorPointVisual target = obj as ColorPointVisual;
            ColorPoint oldValue = (ColorPoint)args.OldValue;
            ColorPoint newValue = (ColorPoint)args.NewValue;
            if (oldValue != newValue)
                target.OnColorPointChanged(oldValue, newValue);
        }

        protected virtual void OnColorPointChanged(ColorPoint oldValue, ColorPoint newValue)
        {
        }
    }
}
