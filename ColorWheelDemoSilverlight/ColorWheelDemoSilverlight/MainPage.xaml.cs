using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ColorWheelDemoSilverlight
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
           //var color= Color.FromArgb(255, 255, 255, 0);
           // var v=HsvColor.FromColor(color);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ssss(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
