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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PathDemoUWwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PathDemo : Page
    {
        public PathDemo()
        {
            this.InitializeComponent();
            Loaded += PathDemo_Loaded;
        }

        private void PathDemo_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RingSegment.BeginUpdate();
                RingSegment.StartAngle = 30;
                RingSegment.EndAngle = 330;
                RingSegment.Radius = 100;
                RingSegment.InnerRadius = 80;
            }
            finally
            {
                RingSegment.EndUpdate();
            }

            using (RingSegment.DeferRefresh())
            {
                RingSegment.StartAngle = 30;
                RingSegment.EndAngle = 330;
                RingSegment.Radius = 100;
                RingSegment.InnerRadius = 80;
            }
        }
    }
}
