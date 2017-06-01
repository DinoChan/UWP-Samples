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

namespace ShapeDemoSilverlight
{
    public partial class PointCollectionTestPage : UserControl
    {
        public PointCollectionTestPage()
        {
            InitializeComponent();
            Loaded += PointCollectionTestPage_Loaded;
        }

        private void PointCollectionTestPage_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard1.Begin();
        }
    }
}
