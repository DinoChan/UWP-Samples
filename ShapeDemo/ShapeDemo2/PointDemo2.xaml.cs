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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ShapeDemo2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PointDemo2 : Page
    {
        public PointDemo2()
        {
            this.InitializeComponent();
            Loaded += PointAnimationPage_Loaded;

            var geometry = PathSeries.Data as PathGeometry;
            _pathFigure = geometry.Figures[0];
            for (int i = 0; i < 20; i++)
            {
                var point = new Point((i + 1) * PathSeries.Width / 20, 60);
                _pathFigure.Segments.Add(new LineSegment { Point = point });
            }
        }

        private PathFigure _pathFigure;

        private void PointAnimationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard1.Begin();
            Storyboard2.Begin();


        }

        private Storyboard _storyboard;
        private void OnStartPointAnimation(object sender, RoutedEventArgs e)
        {
            if (_storyboard != null)
                _storyboard.Stop();

            _storyboard = new Storyboard();
            Random random = new Random();

            for (int i = 0; i < 20; i++)
            {
                var animation = new PointAnimation { Duration = TimeSpan.FromSeconds(3) };
                Storyboard.SetTarget(animation, _pathFigure.Segments[i]);
                Storyboard.SetTargetProperty(animation, "(LineSegment.Point)");
                animation.EnableDependentAnimation = true;
                animation.EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseOut };
                animation.To = new Point((_pathFigure.Segments[i] as LineSegment).Point.X, (i % 2 == 0 ? 1 : -1) * i * 1.2 + 60);
                _storyboard.Children.Add(animation);
            }
            _storyboard.Begin();

        }
    }
}
