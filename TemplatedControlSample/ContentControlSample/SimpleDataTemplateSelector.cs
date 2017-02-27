using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ContentControlSample
{
    public class SimpleDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PassTemplate { get; set; }

        public DataTemplate FailTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var model = item as ScoreModel;
            if (model == null)
                return null;

            if (model.Score >= 60)
                return PassTemplate;
            else
                return FailTemplate;
        }
    }
}
