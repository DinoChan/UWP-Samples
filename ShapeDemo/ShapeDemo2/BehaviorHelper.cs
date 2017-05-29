using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ShapeDemo2
{
   public class BehaviorHelper
    {
        /// <summary>
        //  从指定元素获取 Behaviours 依赖项属性的值。
        /// </summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>Behaviours 依赖项属性的值</returns>
        public static BehaviorCollection GetBehaviours(DependencyObject obj)
        {
            return (BehaviorCollection)obj.GetValue(BehavioursProperty);
        }

        /// <summary>
        /// 将 Behaviours 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">The element on which to set the property value.</param>
        /// <param name="value">The property value to set.</param>
        public static void SetBehaviours(DependencyObject obj, BehaviorCollection value)
        {
            obj.SetValue(BehavioursProperty, value);
        }

        /// <summary>
        /// 标识 Behaviours 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty BehavioursProperty =
            DependencyProperty.RegisterAttached("Behaviours", typeof(BehaviorCollection), typeof(BehaviorHelper), new PropertyMetadata(null, OnBehavioursChanged));


        private static void OnBehavioursChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as UIElement;
            if (target == null)
                return;

            BehaviorCollection oldValue = (BehaviorCollection)args.OldValue;
            BehaviorCollection newValue = (BehaviorCollection)args.NewValue;
            if (oldValue == newValue || newValue == null)
                return;

            newValue.TargetElement = target;
        }

    }
}
