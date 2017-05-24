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
    public partial class AttachedPropertyTest : UserControl
    {
        public AttachedPropertyTest()
        {
            InitializeComponent();
            Loaded += AttachedPropertyTest_Loaded;
        }

        private void AttachedPropertyTest_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard1.Begin();
            return;
            Storyboard storyboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation {Duration = TimeSpan.FromSeconds(5),To=100};
            Storyboard.SetTarget(animation, Triangle);
            Storyboard.SetTargetProperty(animation, new PropertyPath(PathExtention.ProgressProperty));
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }
    }
}
