using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ShapeDemo
{
    public class EllipseStrokeDashArrayConverter : DependencyObject, IValueConverter
    {

        /// <summary>
        /// 获取或设置TargetEllipse的值
        /// </summary>  
        public Ellipse TargetEllipse
        {
            get { return (Ellipse)GetValue(TargetEllipseProperty); }
            set { SetValue(TargetEllipseProperty, value); }
        }

        /// <summary>
        /// 标识 TargetEllipse 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TargetEllipseProperty =
            DependencyProperty.Register("TargetEllipse", typeof(Ellipse), typeof(EllipseStrokeDashArrayConverter), new PropertyMetadata(null));


        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double == false)
                return new DoubleCollection { 0, double.MaxValue }; 

            var progress = (double)value;

            if (TargetEllipse == null)
                return new DoubleCollection { 0, double.MaxValue }; 

            var totalLength = GetTotalLength();
            totalLength = totalLength / TargetEllipse.StrokeThickness;
            var thirdSection = progress * totalLength / 100;
            var secondSection = (totalLength - thirdSection) / 2;
            var result = new DoubleCollection { 0, secondSection, thirdSection, double.MaxValue };
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private double GetTotalLength()
        {
            if (TargetEllipse == null)
                return 0;

            return (TargetEllipse.ActualHeight - TargetEllipse.StrokeThickness) * Math.PI;
        }
    }
}
