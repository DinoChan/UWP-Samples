using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ShapeDemoSilverlight
{
    public class ProgressToStrokeDashArrayConverter2 : ProgressToStrokeDashArrayConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double == false)
                return null;

            var progress = (double)value;

            if (TargetTriangle == null)
                return null;

            var totalLength = GetTriangleTotalLength();
            totalLength = totalLength / TargetTriangle.StrokeThickness;
            var thirdSection = progress * totalLength / 100 ;
            var secondSection = (totalLength - thirdSection) / 2;
            var result = new DoubleCollection { 0, secondSection, thirdSection, double.MaxValue };
            return result;
        }

       
    }
}
