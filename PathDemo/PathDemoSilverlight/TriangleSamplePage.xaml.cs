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
using System.Windows.Shapes;

namespace PathDemoSilverlight
{
    public partial class TriangleSamplePage : UserControl
    {
        public TriangleSamplePage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Triangle.Direction = Direction.Left;
        }

        private void OnDown(object sender, RoutedEventArgs e)
        {
            Triangle.Direction = Direction.Down;
        }

        private void OnRight(object sender, RoutedEventArgs e)
        {
            Triangle.Direction = Direction.Right;
        }

        private void OnUp(object sender, RoutedEventArgs e)
        {
            Triangle.Direction = Direction.Up;
        }
    }
}
