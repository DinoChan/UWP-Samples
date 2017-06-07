using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ColorWheelUwp
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

        protected override void OnApplyTemplate()
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
            //diameter = 800;
            int radius = diameter / 2;
            var source = new WriteableBitmap(diameter, diameter);
            var pixels = source.PixelBuffer.GetPixels();
            double[,] array = new double[diameter, diameter];
            for (int i = 0; i < diameter * diameter; i++)
            {

                int x = i % diameter;
                int y = i / diameter;
                double distance = Math.Sqrt(Math.Pow(radius - x, 2) + Math.Pow(radius - y, 2));
                var saturation = distance * 100 / radius;
                array[x, y] = saturation;
                if (saturation >= 100)
                {
                    pixels.Bytes[i*4] = (Byte)0;
                    pixels.Bytes[i*4 + 1] = (Byte)0;
                    pixels.Bytes[i*4 + 2] = (Byte)0;
                    pixels.Bytes[i*4 + 3] = (Byte)0;
                }
                else
                {
                    int cx = x - radius;
                    int cy = y - radius;

                    double theta = Math.Atan2(cy, cx);

                    if (theta < 0)
                    {
                        theta += 2 * Math.PI;
                    }

                    double alpha = Math.Sqrt((cx * cx) + (cy * cy));

                    var hue = theta / (Math.PI * 2) * 360.0;
                    var color = ColorHelper.FromHsv(hue, saturation, 100);
                    pixels.Bytes[i*4] = (Byte)color.B;
                    pixels.Bytes[i*4 + 1] = (Byte)color.G;
                    pixels.Bytes[i *4+ 2] = (Byte)color.R;
                    pixels.Bytes[i *4+ 3] = (Byte)255;
                }
            }
            pixels.UpdateFromBytes();
            source.Invalidate();
            _imageElement.Source = source;
        }
    }
}
