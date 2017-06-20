using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace ColorWheelUwp
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
