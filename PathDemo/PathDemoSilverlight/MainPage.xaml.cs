using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Expression.Shapes;

namespace PathDemoSilverlight
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
            Storyboard_Copy1.Begin();

        }

        private double h = 500;
        private double t = 1;

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            this.Height = h;
            this.Width = h;
            h--;
            Triangle.StrokeThickness = t++;
           

        }

        private void Re_LayoutUpdated(object sender, EventArgs e)
        {
            //Debug.WriteLine("Re_LayoutUpdated");
        }

        private void Arc_LayoutUpdated(object sender, EventArgs e)
        {
            //Debug.WriteLine("Arc_LayoutUpdated");
        }

        private void Arc_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Debug.WriteLine("Arc_SizeChanged");
        }

        private void Re_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Debug.WriteLine("Re_SizeChanged");
        }
    }
}
