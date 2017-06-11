using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ButtonDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _compositor = ElementCompositionPreview.GetElementVisual(Button).Compositor;

            //get interop visual for XAML TextBlock
            var text = ElementCompositionPreview.GetElementVisual(Button);

            _pointLight = _compositor.CreatePointLight();
            _pointLight.Color = Colors.White;
            _pointLight.CoordinateSpace = text; //set up co-ordinate space for offset
            _pointLight.Targets.Add(text); //target XAML TextBlock

            //starts out to the left; vertically centered; light's z-offset is related to fontsize
            _pointLight.Offset = new Vector3(-(float)Button.ActualWidth, (float)Button.ActualHeight / 2, (float)Button.FontSize);

            //simple offset.X animation that runs forever
            //var animation = _compositor.CreateScalarKeyFrameAnimation();
            //animation.InsertKeyFrame(1, 2 * (float)Button.ActualWidth);
            //animation.Duration = TimeSpan.FromSeconds(3.3f);
            //animation.IterationBehavior = AnimationIterationBehavior.Forever;

            //_pointLight.StartAnimation("Offset.X", animation);
        }

        private Compositor _compositor;
        private PointLight _pointLight;

        private void Button_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(Button);
            _pointLight.Offset = new Vector3((float)position.Position.X, (float)position.Position.Y, (float)Button.FontSize/2);
        }

        private void Button_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("...");
        }

        private void Button_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            
            Button.Margin = new Thickness(e.Delta.Translation.X, e.Delta.Translation.Y,0,0);
        }
    }
}
