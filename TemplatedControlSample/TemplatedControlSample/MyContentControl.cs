using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace TemplatedControlSample
{
    [ContentProperty(Name = "Content")]
    [StyleTypedProperty]
    public class MyContentControl : Control
    {
        public MyContentControl()
        {
            this.DefaultStyleKey = typeof(MyContentControl);
        }

        /// <summary>
        /// 获取或设置Content的值
        /// </summary>  
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// 标识 Content 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(MyContentControl), new PropertyMetadata(null, OnContentChanged));

        private static void OnContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MyContentControl target = obj as MyContentControl;
            object oldValue = (object)args.OldValue;
            object newValue = (object)args.NewValue;
            if (oldValue != newValue)
                target.OnContentChanged(oldValue, newValue);
        }

        protected virtual void OnContentChanged(object oldValue, object newValue)
        {
        }



        /// <summary>
        /// 获取或设置ContentTemplate的值
        /// </summary>  
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        /// <summary>
        /// 标识 ContentTemplate 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(MyContentControl), new PropertyMetadata(null, OnContentTemplateChanged));

        private static void OnContentTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MyContentControl target = obj as MyContentControl;
            DataTemplate oldValue = (DataTemplate)args.OldValue;
            DataTemplate newValue = (DataTemplate)args.NewValue;
            if (oldValue != newValue)
                target.OnContentTemplateChanged(oldValue, newValue);
        }

        protected virtual void OnContentTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {

        }
    }
}
