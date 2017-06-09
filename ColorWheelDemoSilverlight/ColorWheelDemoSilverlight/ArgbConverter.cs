using System;
using System.Windows.Media;

namespace ColorWheelDemoSilverlight
{
    public class ArgbConverter : IColorConverter
    {
        public ArgbModel Model { get; set; }

        public Color ToColor(Color color, double value)
        {
            var byteOfValue = Convert.ToByte(Math.Min(255, value));
            switch (Model)
            {
                case ArgbModel.Alpha:
                  return  Color.FromArgb(byteOfValue, color.R, color.G, color.B);
                case ArgbModel.Red:
                    return Color.FromArgb(color.A, byteOfValue, color.G, color.B);
                case ArgbModel.Green:
                    return Color.FromArgb(color.A, color.R, byteOfValue, color.B);
                case ArgbModel.Blue:
                    return Color.FromArgb(color.A, color.R, color.G, byteOfValue);
            }

            return Color.FromArgb(0, 0, 0, 0);
        }

        public double ToValue(Color color)
        {
            switch (Model)
            {
                case ArgbModel.Alpha:
                    return color.A;
                case ArgbModel.Red:
                    return color.R;
                case ArgbModel.Green:
                    return color.G;
                case ArgbModel.Blue:
                    return color.B;
            }
            return double.NaN;
        }
    }

    public enum ArgbModel
    {
        Alpha = 0,
        Red = 1,
        Green = 2,
        Blue = 3
    }
}