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

            int width = Convert.ToInt32(e.NewSize.Width);
            int height = Convert.ToInt32(e.NewSize.Height);
            int diameter = width < height ? width : height;
            int radius = diameter / 2;
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
                var saturation = distance * 100 / radius;
                array[x, y] = saturation;
                if (saturation >= 100)
                {
                    source.Pixels[i] = 255 << 24 | 255 << 16 | 255 << 8 | 255;
                }
                else
                {
                    //var deltaX = x * x - radius * radius;
                    //var deltaY = y * y - radius * radius;
                    int cx = x - radius;
                    int cy = y - radius;

                    double theta = Math.Atan2(cy, cx);

                    if (theta < 0)
                    {
                        theta += 2 * Math.PI;
                    }

                    double alpha = Math.Sqrt((cx * cx) + (cy * cy));

                    var hue = (int)((theta / (Math.PI * 2)) * 360.0);
                    var color = new HsvColor(hue, Convert.ToInt32(saturation), 100);
                    var rgbColor = color.ToRgb();

                    source.Pixels[i] = 255 << 24 | rgbColor.Red << 16 | rgbColor.Green << 8 | rgbColor.Blue;
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
