using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace PathDemoUWwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Polyline s;s.Stretch 
             PointCollection pointCollection = new PointCollection();
            for (int i = 0; i < 300; i++)
            {
                pointCollection.Add(new Point(i, i % 2 == 0 ? 0 : 500));
            }
            Polyline.Points = pointCollection;
            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            bool isIncrease = true;
            double position = 0;
            while (true)
            {
                if (position < 0)
                    isIncrease = true;
                else if (position > 500)
                    isIncrease = false;

                if (isIncrease)
                    position++;
                else
                    position--;

                Canvas.SetLeft(Polyline, position);
                Canvas.SetTop(Polyline, position);
                await Task.Delay(100);
            }
        }
    }
}
