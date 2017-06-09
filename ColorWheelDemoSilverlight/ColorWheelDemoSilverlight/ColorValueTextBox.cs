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
    public class ColorValueTextBox:Control
    {
        public ColorValueTextBox()
        {
            DefaultStyleKey = typeof(ColorValueTextBox);
        
        }



        /// <summary>
        /// 获取或设置Decimals的值
        /// </summary>  
        public int Decimals
        {
            get { return (int)GetValue(DecimalsProperty); }
            set { SetValue(DecimalsProperty, value); }
        }

        /// <summary>
        /// 标识 Decimals 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DecimalsProperty =
            DependencyProperty.Register("Decimals", typeof(int), typeof(ColorValueTextBox), new PropertyMetadata(0, OnDecimalsChanged));

        private static void OnDecimalsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ColorValueTextBox target = obj as ColorValueTextBox;
            int oldValue = (int)args.OldValue;
            int newValue = (int)args.NewValue;
            if (oldValue != newValue)
                target.OnDecimalsChanged(oldValue, newValue);
        }

        protected virtual void OnDecimalsChanged(int oldValue, int newValue)
        {
        }
    }
}
