﻿using System;
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
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TemplatedControlSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ItemsControlTest : Page
    {
        public ItemsControlTest()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private int _count;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            List<int> items = new List<int>();
            for (int i = 0; i < 1000; i++)
            {
                items.Add(i);
            }
            ItemsControl.ItemsSource = items;
        }

        private void OnItemLoaded(object sender, RoutedEventArgs e)
        {
            _count++;
            CountElement.Text = _count.ToString();
        }
    }
}
