using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ColorWheelDemoSilverlight
{
    [TemplatePart(Name = ImageElementName, Type = typeof(Image))]
    public class HsvWheel : Control
    {
        private const string ImageElementName = "ImageElement";

        public HsvWheel()
        {
            this.DefaultStyleKey = typeof(HsvWheel);
            this.SizeChanged += OnHsvWheelSizeChanged;
        }


        /// <summary>
        /// 获取或设置Radius的值
        /// </summary>  
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        /// 标识 Radius 依赖属性。
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(HsvWheel), new PropertyMetadata(0d));

        

        private Image _imageElement;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _imageElement = this.GetTemplateChild(ImageElementName) as Image;
        }

        private void OnHsvWheelSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_imageElement == null)
                return;

            this.SizeChanged -= OnHsvWheelSizeChanged;
            int width = Convert.ToInt32(e.NewSize.Width);
            int height = Convert.ToInt32(e.NewSize.Height);
            int diameter = width < height ? width : height;
            diameter = 800;
            int radius = diameter / 2;
            Radius = radius;
            var source = new WriteableBitmap(diameter, diameter);
            double[,] array = new double[diameter, diameter];
            for (int i = 0; i < diameter * diameter; i++)
            {
                //source.Pixels[i * 4] = 255 << 24 | 255 << 16 | 255 << 8 | 255;
                //source.Pixels[i * 4+1] = 255 << 24 | 255 << 16 | 255 << 8 | 255;
                //source.Pixels[i * 4+2] = 255 << 24 | 255 << 16 | 255 << 8 | 255;
                //source.Pixels[i*4+3] = 255 << 24 | 255 << 16 | 255 << 8 | 255;
                //continue;
                int x = i % diameter;
                int y = i / diameter;
                double distance = Math.Sqrt(Math.Pow(radius - x, 2) + Math.Pow(radius - y, 2));
                var saturation = distance  / radius;
                array[x, y] = saturation;
                if (saturation >= 1)
                {
                    source.Pixels[i] = 255 << 24 | 255 << 16 | 255 << 8 | 255;
                }
                else
                {
                    //var deltaX = x * x - radius * radius;
                    //var deltaY = y * y - radius * radius;
                    int cx = x - radius;
                    int cy = y - radius;

                    double theta = Math.Atan2(-cy, cx);

                    if (theta < 0)
                    {
                        theta += 2 * Math.PI;
                    }

                    double alpha = Math.Sqrt((cx * cx) + (cy * cy));

                    var hue = (int)((theta / (Math.PI * 2)) * 360.0);
                    var color =ColorHelper.FromHsv(hue, saturation, 1);
                   

                    source.Pixels[i] = 255 << 24 | color.R << 16 | color.G << 8 | color.B;
                }
            }
            //string s = "";
            //for (int i = 0; i < diameter; i++)
            //{
            //    for (int j = 0; j < diameter; j++)
            //    {
            //        s += array[i,j] + "\t";
            //    }
            //    s += Environment.NewLine;
            //}

            _imageElement.Source = source;
        }
    }
}
