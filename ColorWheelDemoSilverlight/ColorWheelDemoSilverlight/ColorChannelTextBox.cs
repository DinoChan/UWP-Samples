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
    [TemplatePart(Name = NumericTextBoxName, Type = typeof(NumericUpDown))]
    public class ColorChannelTextBox : ColorChannelEditor
    {
        private const string NumericTextBoxName = "NumericTextBox";

        public ColorChannelTextBox()
        {
            DefaultStyleKey = typeof(ColorChannelTextBox);
        }

        /// <summary>
        /// 获取或设置DecimalPlaces的值
        /// </summary>  
        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }

        /// <summary>
        /// 标识 DecimalPlaces 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register("DecimalPlaces", typeof(int), typeof(ColorChannelTextBox), new PropertyMetadata(0));

        

        /// <summary>
        /// 获取或设置Value的值
        /// </summary>  
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// 标识 Value 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(ColorChannelTextBox), new PropertyMetadata(0d, OnValueChanged));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ColorChannelTextBox target = obj as ColorChannelTextBox;
            double oldValue = (double)args.OldValue;
            double newValue = (double)args.NewValue;
            if (oldValue != newValue)
                target.OnValueChanged(oldValue, newValue);
        }

        protected virtual void OnValueChanged(double oldValue, double newValue)
        {
            if (ColorConverter == null)
                return;

            Color = ColorConverter.ToColor(Color, newValue);
        }

        protected override void OnColorChanged(Color oldValue, Color newValue)
        {
            base.OnColorChanged(oldValue, newValue);
            if (ColorConverter == null)
                return;

            Value = ColorConverter.ToValue(Color);
        }
    }
}
