using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ShapeDemoSilverlight
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



        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double == false)
                return 0d;

            var progress = (double)value;

            if (TargetContentControl == null)
                return 0d;

            var element = TargetContentControl.Content as FrameworkElement;
            if (element == null)
                return 0d;

            return element.Height * (100 - progress) / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
