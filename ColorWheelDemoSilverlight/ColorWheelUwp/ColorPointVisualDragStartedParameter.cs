using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace ColorWheelUwp
{
    public class ColorPointVisualDragStartedParameter
    {
        public ColorPointVisual ColorPointVisual { get; private set; }

        public Point Position { get; private set; }

        public ColorPointVisualDragStartedParameter(ColorPointVisual colorPointVisual, Point position)
        {
            ColorPointVisual = colorPointVisual;
            Position = position;
        }

    }
}
