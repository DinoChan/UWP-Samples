using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TemplatedControlSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventListViewSamplePage : Page
    {
        public EventListViewSamplePage()
        {
            this.InitializeComponent();
            Loaded += EventListViewSamplePage_Loaded;
        }

        private async void EventListViewSamplePage_Loaded(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                List.Items.Add("Button clicked at "+DateTime.Now.ToString("mm:ss"));
            }
            //Task.Run(async() =>
            //{
            //    await Task.Delay(1000);
            //    this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //     {
            //         List.Items.Add("aaaa");
            //     });
            //});
        }
    }
}
