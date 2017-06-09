using System;
using System.Windows.Media;

namespace ColorWheelDemoSilverlight
{
    public class ArgbConverter : IColorConverter
    {
        public ArgbMode Mode { get; set; }

        public Color ToColor(Color color, double value)
        {
            var byteOfValue = Convert.ToByte(Math.Min(255, value));
            switch (Mode)
            {
                case ArgbMode.Alpha:
                  return  Color.FromArgb(byteOfValue, color.R, color.G, color.B);
                case ArgbMode.Red:
                    return Color.FromArgb(color.A, byteOfValue, color.G, color.B);
                case ArgbMode.Green:
                    return Color.FromArgb(color.A, color.R, byteOfValue, color.B);
                case ArgbMode.Blue:
                    return Color.FromArgb(color.A, color.R, color.G, byteOfValue);
            }

            return Color.FromArgb(0, 0, 0, 0);
        }

        public double ToValue(Color color)
        {
            switch (Mode)
            {
                case ArgbMode.Alpha:
                    return color.A;
                case ArgbMode.Red:
                    return color.R;
                case ArgbMode.Green:
                    return color.G;
                case ArgbMode.Blue:
                    return color.B;
            }
            return double.NaN;
        }
    }

    public enum ArgbMode
    {
        Alpha = 0,
        Red = 1,
        Green = 2,
        Blue = 3
    }
}