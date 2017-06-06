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
    public partial class PointAnimationPage : UserControl
    {
        public PointAnimationPage()
        {
            InitializeComponent();
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
            //Storyboard2.Begin();


        }

        private Storyboard _storyboard;
        private void OnStartPointAnimation(object sender, RoutedEventArgs e)
        {
            if (_storyboard != null)
                _storyboard.Stop();

            _storyboard = new Storyboard();
            Random random = new Random();

            for (int i = 0; i < _pathFigure.Segments.Count; i++)
            {
                var animation = new PointAnimation { Duration = TimeSpan.FromSeconds(3) };
                Storyboard.SetTarget(animation, _pathFigure.Segments[i]);
                Storyboard.SetTargetProperty(animation, new PropertyPath(LineSegment.PointProperty));

                animation.EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseOut };
                animation.To = new Point((_pathFigure.Segments[i] as LineSegment).Point.X, (i % 2 == 0 ? 1 : -1) * i*1.2 + 60);
                _storyboard.Children.Add(animation);
            }
            _storyboard.Begin();
         
        }
    }
}
