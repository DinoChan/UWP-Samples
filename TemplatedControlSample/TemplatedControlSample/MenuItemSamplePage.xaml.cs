using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Forms;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TemplatedControlSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MenuItemSamplePage : Page
    {
        public MenuItemSamplePage()
        {
            this.InitializeComponent();
            var command = new DelegateCommand<object>(Click, CanExecute);
            this.DataContext = command;
        }

        private void Click(object parameter)
        {
            MessageBox.Show(parameter.ToString());
        }

        private bool CanExecute(object parameter)
        {
            string text = parameter as string;
            return string.IsNullOrWhiteSpace(text) == false;
        }
    }
}
