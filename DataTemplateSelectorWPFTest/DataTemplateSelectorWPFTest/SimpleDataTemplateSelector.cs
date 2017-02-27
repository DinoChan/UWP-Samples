using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DataTemplateSelectorWPFTest
{
   public class SimpleDataTemplateSelector: DataTemplateSelector
   {
       public DataTemplate PassTemplate { get; set; }

       public DataTemplate FailTemplate { get; set; }

       public override DataTemplate SelectTemplate(object item, DependencyObject container)
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
