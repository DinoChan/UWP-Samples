using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ColorWheelDemoSilverlight
{
    public class ColorPointVisualDragDeltaParameter
    {
        public Point Translation { get; private set; }

        public ColorPointVisual ColorPointVisual { get; private set; }

        public ColorPointVisualDragDeltaParameter(ColorPointVisual colorPointVisual, Point translation)
        {
            ColorPointVisual = colorPointVisual;
            Translation = translation;
        }
    }
}
