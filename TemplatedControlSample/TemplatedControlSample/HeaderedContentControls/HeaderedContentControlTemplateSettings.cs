using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TemplatedControlSample
{
    public class HeaderedContentControlTemplateSettings: DependencyObject
    {
        public Thickness HeaderMargin
        {
            get
            {
                return new Thickness(0, 0, 0, 8);
            }
        }

        public FontWeight HeaderFontWeight
        {
            get
            {
                return FontWeights.Normal;
            }
        }
    }
}
