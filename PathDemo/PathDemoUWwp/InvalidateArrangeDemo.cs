using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PathDemoUWwp
{
    public class InvalidateArrangeDemo : DependencyObject
    {
        public InvalidateArrangeDemo()
        {
            
        }

        protected bool ArrangeDirty { get; set; }

        public void IndalidateArrange()
        {
            if (ArrangeDirty == true)
                return;

            ArrangeDirty = true;
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                ArrangeDirty = false;
                lock (this)
                {
                    //要测试这里是不是只会执行一次
                    //Measure
                    //Arrange
                    Debug.WriteLine("IndalidateArrange");
                }
            });
        }
    }
}
