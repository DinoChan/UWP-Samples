using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using System.Collections.Specialized;

namespace ShapeDemo2
{
    public class BehaviorCollection : ObservableCollection<BehaviorBase>
    {

        private UIElement _targetElement;

        /// <summary>
        /// 获取或设置 TargetElement 的值
        /// </summary>
        public UIElement TargetElement
        {
            get { return _targetElement; }
            set
            {
                if (_targetElement == value)
                    return;

                _targetElement = value;
                foreach (var item in Items)
                {
                    item.TargetElement = value;
                }
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<BehaviorBase>())
                {
                    item.TargetElement = TargetElement;
                }
            }
        }


      

    }
}
