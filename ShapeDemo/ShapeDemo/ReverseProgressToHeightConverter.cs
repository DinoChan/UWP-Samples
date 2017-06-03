using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
namespace ShapeDemo
{
    public class ReverseProgressToHeightConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// 获取或设置TargetContentControl的值
        /// </summary>  
        public ContentControl TargetContentControl
        {
            get { return (ContentControl)GetValue(TargetContentControlProperty); }
            set { SetValue(TargetContentControlProperty, value); }
        }

        /// <summary>
        /// 标识 TargetContentControl 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TargetContentControlProperty =
            DependencyProperty.Register("TargetContentControl", typeof(ContentControl), typeof(ReverseProgressToHeightConverter), new PropertyMetadata(null));



        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double == false)
                return double.NaN;

            var progress = (double)value;

            if (TargetContentControl == null)
                return double.NaN;

            var element = TargetContentControl.Content as FrameworkElement;
            if (element == null)
                return double.NaN;

            return element.Height * (100 - progress) / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }
}
