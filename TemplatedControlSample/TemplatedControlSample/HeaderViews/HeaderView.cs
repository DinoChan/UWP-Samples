using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Reflection;

namespace TemplatedControlSample
{
    [TemplateVisualState(Name = NormalState, GroupName = CommonStates)]
    [TemplateVisualState(Name = PointerOverState, GroupName = CommonStates)]
    [TemplateVisualState(Name = NoHeaderState, GroupName = HeaderStates)]
    [TemplateVisualState(Name = HasHeaderState, GroupName = HeaderStates)]
    public class HeaderView : Control
    {
        public const string CommonStates = "CommonStates";
        public const string NormalState = "Normal";
        public const string PointerOverState = "PointerOver";

        public const string HeaderStates = "HeaderStates";
        public const string NoHeaderState = "NoHeader";
        public const string HasHeaderState = "HasHeader";

        /// <summary>
        //  从指定元素获取 Header 依赖项属性的值。
        /// </summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>Header 依赖项属性的值</returns>
        public static object GetHeader(DependencyObject obj)
        {
            return (object)obj.GetValue(HeaderProperty);
        }

        /// <summary>
        /// 将 Header 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">The element on which to set the property value.</param>
        /// <param name="value">The property value to set.</param>
        public static void SetHeader(DependencyObject obj, object value)
        {
            obj.SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// 标识 Header 依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.RegisterAttached("Header", typeof(object), typeof(HeaderView), new PropertyMetadata(null, OnHeaderChanged));

        private static void OnHeaderChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            object oldValue = (object)args.OldValue;
            object newValue = (object)args.NewValue;

            if (oldValue == newValue)
                return;

            HeaderView target = obj as HeaderView;
            if (target != null)
            {
                target.OnHeaderChanged(oldValue, newValue);
            }
            else
            {
                FrameworkElement element = obj as FrameworkElement;
                if (element == null)
                    return;

                var headerProperty = element.GetType().GetProperty("Header");
                if (headerProperty == null)
                    throw new NotSupportedException("调用的对象必须拥有Header属性");

                HeaderView view = new HeaderView
                {
                    Header = newValue,
                    AttachedElement = element
                };
                headerProperty.SetValue(element, view);
            }
        }

        /// <summary>
        /// 标识 AttachedElement 依赖属性。
        /// </summary>
        public static readonly DependencyProperty AttachedElementProperty =
            DependencyProperty.Register("AttachedElement", typeof(FrameworkElement), typeof(HeaderView), new PropertyMetadata(null, OnAttachedElementChanged));

        private static void OnAttachedElementChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            HeaderView target = obj as HeaderView;
            FrameworkElement oldValue = (FrameworkElement)args.OldValue;
            FrameworkElement newValue = (FrameworkElement)args.NewValue;
            if (oldValue != newValue)
                target.OnAttachedElementChanged(oldValue, newValue);
        }

        public HeaderView()
        {
            this.DefaultStyleKey = typeof(HeaderView);
        }

        /// <summary>
        /// 获取或设置Header的值
        /// </summary>  
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }


        /// <summary>
        /// 获取或设置AttachedElement的值
        /// </summary>  
        public FrameworkElement AttachedElement
        {
            get { return (FrameworkElement)GetValue(AttachedElementProperty); }
            set { SetValue(AttachedElementProperty, value); }
        }

        private bool _isPointerEntered;

        protected virtual void OnAttachedElementChanged(FrameworkElement oldValue, FrameworkElement newValue)
        {
            if (oldValue != null)
            {
                oldValue.PointerEntered -= OnPointerEntered;
                oldValue.PointerExited -= OnPointerExited;
            }

            if (newValue != null)
            {
                newValue.PointerEntered += OnPointerEntered;
                newValue.PointerExited += OnPointerExited;
            }
        }

        protected virtual void OnHeaderChanged(object oldValue, object newValue)
        {
            UpdateVisualState();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateVisualState(false);
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _isPointerEntered = true;
            UpdateVisualState();
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            _isPointerEntered = false;
            UpdateVisualState();
        }

        internal virtual void UpdateVisualState(bool useTransitions = true)
        {
            if (_isPointerEntered)
                VisualStateManager.GoToState(this, PointerOverState, useTransitions);
            else
                VisualStateManager.GoToState(this, NormalState, useTransitions);

            if (Header == null)
                VisualStateManager.GoToState(this, NoHeaderState, useTransitions);
            else
                VisualStateManager.GoToState(this, HasHeaderState, useTransitions);
        }
    }
}
