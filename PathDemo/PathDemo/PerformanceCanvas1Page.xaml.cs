using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PathDemo
{
    /// <summary>
    /// PerformanceCanvas1Page.xaml 的交互逻辑
    /// </summary>
    public partial class PerformanceCanvas1Page : Window
    {
        public PerformanceCanvas1Page()
        {
            InitializeComponent();
            PointCollection pointCollection = new PointCollection();
            for (int i = 0; i < 300; i++)
            {
                pointCollection.Add(new Point(i, i % 2 == 0 ? 0 : 300));
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
                else if (position > 100)
                    isIncrease = false;

                if (isIncrease)
                    position++;
                else
                    position--;

                Canvas.SetLeft(Polyline, position);
                Canvas.SetTop(Polyline, position);
                //await Task.Delay(100);
            }
        }
    }
}
