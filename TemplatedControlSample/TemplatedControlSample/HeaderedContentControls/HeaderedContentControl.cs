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

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace TemplatedControlSample
{
    public class HeaderedContentControl : ContentControl
    {
        public HeaderedContentControl()
        {
            this.DefaultStyleKey = typeof(HeaderedContentControl);
            TemplateSettings = new HeaderedContentControlTemplateSettings();
        }

        public HeaderedContentControlTemplateSettings TemplateSettings { get; }

        /// <summary>
        /// 获取或设置Header的值
        /// </summary>  
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// 标识 Header 依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(HeaderedContentControl), new PropertyMetadata(null, OnHeaderChanged));

        private static void OnHeaderChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            HeaderedContentControl target = obj as HeaderedContentControl;
            object oldValue = (object)args.OldValue;
            object newValue = (object)args.NewValue;
            if (oldValue != newValue)
                target.OnHeaderChanged(oldValue, newValue);
        }

        /// <summary>
        /// 获取或设置HeaderTemplate的值
        /// </summary>  
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// 标识 HeaderTemplate 依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(HeaderedContentControl), new PropertyMetadata(null, OnHeaderTemplateChanged));

        private static void OnHeaderTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            HeaderedContentControl target = obj as HeaderedContentControl;
            DataTemplate oldValue = (DataTemplate)args.OldValue;
            DataTemplate newValue = (DataTemplate)args.NewValue;
            if (oldValue != newValue)
                target.OnHeaderTemplateChanged(oldValue, newValue);
        }

        protected virtual void OnHeaderChanged(object oldValue, object newValue)
        {
        }

        protected virtual void OnHeaderTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
        }
    }
}
