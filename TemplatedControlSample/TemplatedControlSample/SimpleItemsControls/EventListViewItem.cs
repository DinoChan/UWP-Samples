using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TemplatedControlSample
{
    public sealed class EventListViewItem : ListViewItem
    {
        public EventListViewItem()
        {
            this.DefaultStyleKey = typeof(EventListViewItem);
        }

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(EventListViewItem), new PropertyMetadata(null));


    }
}
