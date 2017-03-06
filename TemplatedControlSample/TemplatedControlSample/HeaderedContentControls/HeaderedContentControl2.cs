using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TemplatedControlSample
{
    [StyleTypedProperty(Property = "HeaderStyle", StyleTargetType = typeof(ContentPresenter))]
    public class HeaderedContentControl2 : HeaderedContentControl
    {
        public HeaderedContentControl2()
        {
            this.DefaultStyleKey = typeof(HeaderedContentControl2);
            TemplateSettings = new HeaderedContentControlTemplateSettings();
        }

        public HeaderedContentControlTemplateSettings TemplateSettings { get; }




        /// <summary>
        /// 获取或设置HeaderStyle的值
        /// </summary>  
        public Style HeaderStyle
        {
            get { return (Style)GetValue(HeaderStyleProperty); }
            set { SetValue(HeaderStyleProperty, value); }
        }

        /// <summary>
        /// 标识 HeaderStyle 依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderStyleProperty =
            DependencyProperty.Register("HeaderStyle", typeof(Style), typeof(HeaderedContentControl2), new PropertyMetadata(null, OnHeaderStyleChanged));

        private static void OnHeaderStyleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            HeaderedContentControl2 target = obj as HeaderedContentControl2;
            Style oldValue = (Style)args.OldValue;
            Style newValue = (Style)args.NewValue;
            if (oldValue != newValue)
                target.OnHeaderStyleChanged(oldValue, newValue);
        }

        protected virtual void OnHeaderStyleChanged(Style oldValue, Style newValue)
        {
        }
    }
}
