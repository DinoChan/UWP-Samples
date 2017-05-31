using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ShapeDemo2
{
    public class BehaviorBase : DependencyObject
    {

        /// <summary>
        /// 获取或设置TargetElement的值
        /// </summary>  
        public UIElement TargetElement
        {
            get { return (UIElement)GetValue(TargetElementProperty); }
            set { SetValue(TargetElementProperty, value); }
        }

        /// <summary>
        /// 标识 TargetElement 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register("TargetElement", typeof(UIElement), typeof(BehaviorBase), new PropertyMetadata(null, OnTargetElementChanged));

        private static void OnTargetElementChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            BehaviorBase target = obj as BehaviorBase;
            UIElement oldValue = (UIElement)args.OldValue;
            UIElement newValue = (UIElement)args.NewValue;
            if (oldValue != newValue)
                target.OnTargetElementChanged(oldValue, newValue);
        }

        protected virtual void OnTargetElementChanged(UIElement oldValue, UIElement newValue)
        {
        }
    }
}
