using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace ShapeDemo
{
    public class ProgressToStrokeDashArrayConverter2 : ProgressToStrokeDashArrayConverter
    {
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double == false)
                return null;

            var progress = (double)value;

            if (TargetTriangle == null)
                return null;

            var totalLength = GetTriangleTotalLength();
            totalLength = totalLength / TargetTriangle.StrokeThickness;
            var thirdSection = progress * totalLength / 100;
            var secondSection = (totalLength - thirdSection) / 2;
            var result = new DoubleCollection { 0, secondSection, thirdSection, double.MaxValue };
            return result;
        }
    }
}
