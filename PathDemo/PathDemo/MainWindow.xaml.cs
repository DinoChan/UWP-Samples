using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PathDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
            //Polyline.SizeChanged += Polyline_SizeChanged;
        }

        private void Polyline_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("SizeChanged");
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            watch.Stop();
            MessageBox.Show(watch.ElapsedMilliseconds.ToString());
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            //PointCollection pointCollection = new PointCollection();
            //for (int i = 0; i <1000; i++)
            //{
            //    pointCollection.Add(new Point(i, random.Next(200)));
            //}
            //Polyline.Points = pointCollection;
            PathFigure figure = new PathFigure();
            figure.IsClosed = true;
            figure.IsFilled = true;

            Point startPoint;

            startPoint = new Point(0, 0);
            figure.StartPoint = startPoint;

            for (int i = 0; i < 1000; i++)
            {
                figure.Segments.Add(new LineSegment { Point = new Point(i, random.Next(200)) });
            }

            figure.Segments.Add(new LineSegment { Point = new Point(0, 200) });

            if (figure.Segments.Count > 1)
            {
                PathGeometry geometry = new PathGeometry();
                geometry.Figures.Add(figure);
                Path.Data = geometry;
            }


        }
        Stopwatch watch = new Stopwatch();
        private async void OnChangeSize(object sender, RoutedEventArgs e)
        {
           
            watch.Start();
            Random random = new Random();
            this.Height = random.Next(800);
            //this.Dispatcher.InvokeAsync(() => {
            //    this.Dispatcher.InvokeAsync(() => {
              
            //});
            //});
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            //Action action = new Action(() => {
               
            //});
            this.BeginInvoke(() =>
            {
                this.BeginInvoke(() =>
                {
                    this.BeginInvoke(() => {
                     
                    });
                });
            });
            //this.Dispatcher.BeginInvoke(action);
        }

        private void BeginInvoke(Action action)
        {
            this.Dispatcher.BeginInvoke(action);
        }
    }


}
