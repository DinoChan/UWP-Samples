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
    public class TemplatedParentBridge : DependencyObject
    {

        /// <summary>
        /// 获取或设置TemplatedParent的值
        /// </summary>  
        public object TemplatedParent
        {
            get { return (object)GetValue(TemplatedParentProperty); }
            set { SetValue(TemplatedParentProperty, value); }
        }

        /// <summary>
        /// 标识 TemplatedParent 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TemplatedParentProperty =
            DependencyProperty.Register("TemplatedParent", typeof(object), typeof(TemplatedParentBridge), new PropertyMetadata(null, OnTemplatedParentChanged));

        private static void OnTemplatedParentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            TemplatedParentBridge target = obj as TemplatedParentBridge;
            object oldValue = (object)args.OldValue;
            object newValue = (object)args.NewValue;
            if (oldValue != newValue)
                target.OnTemplatedParentChanged(oldValue, newValue);
        }

        protected virtual void OnTemplatedParentChanged(object oldValue, object newValue)
        {
        }

    }
}
