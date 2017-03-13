using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TemplatedControlSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PropertySamplePage : Page
    {
        public PropertySamplePage()
        {
            this.InitializeComponent();
        }
    }

    public class ClrModel
    {

    }

    public class DOModel
    {

    }

    public class TestControl : Control
    {
        public TestControl()
        {
            Items = new List<Object>();
        }

        public IList<object> Items { get; private set; }

        public object PropertyClr { get; set; }


        /// <summary>
        /// 获取或设置PropertyDP的值
        /// </summary>  
        public object PropertyDP
        {
            get { return (object)GetValue(PropertyDPProperty); }
            set { SetValue(PropertyDPProperty, value); }
        }

        /// <summary>
        /// 标识 PropertyDP 依赖属性。
        /// </summary>
        public static readonly DependencyProperty PropertyDPProperty =
            DependencyProperty.Register("PropertyDP", typeof(object), typeof(TestControl), new PropertyMetadata(null, OnPropertyDPChanged));

        private static void OnPropertyDPChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            TestControl target = obj as TestControl;
            object oldValue = (object)args.OldValue;
            object newValue = (object)args.NewValue;
            if (oldValue != newValue)
                target.OnPropertyDPChanged(oldValue, newValue);
        }

        protected virtual void OnPropertyDPChanged(object oldValue, object newValue)
        {
        }
    }
}
