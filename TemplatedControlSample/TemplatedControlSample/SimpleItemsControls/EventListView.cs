﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TemplatedControlSample
{
    public class EventListView : ListView
    {
        public EventListView()
        {
            _items = new Dictionary<object, DateTime>();
        }

        private Dictionary<object, DateTime> _items;

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new EventListViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is EventListViewItem;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var control = element as EventListViewItem;
            if (control == null)
                return;

            control.Content = item;
            if (_items.ContainsKey(item))
            {
                var time = _items[item];
                control.Header = time.ToString("HH:mm:ss")+": ";
            }
        }

        protected override void OnItemsChanged(object e)
        {
            base.OnItemsChanged(e);
            foreach (var item in Items)
            {
                if (_items.ContainsKey(item) == false)
                    _items.Add(item, DateTime.Now);
            }
        }
    }
}
