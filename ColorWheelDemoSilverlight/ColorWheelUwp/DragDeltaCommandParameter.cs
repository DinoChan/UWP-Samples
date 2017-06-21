using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace ColorWheelUwp
{
    public class DragDeltaCommandParameter
    {
        public Point Translation { get; private set; }

        public ColorPointVisual ColorPointVisual { get; private set; }

        public DragDeltaCommandParameter(ColorPointVisual colorPointVisual, Point translation)
        {
            ColorPointVisual = colorPointVisual;
            Translation = translation;
        }
    }
}
