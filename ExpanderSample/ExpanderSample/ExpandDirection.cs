using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanderSample
{
    /// <summary>
    /// Specifies the direction in which an
    /// <see cref="T:System.Windows.Controls.Expander" /> control opens.
    /// </summary>
    /// <QualityBand>Stable</QualityBand>
    public enum ExpandDirection
    {
        /// <summary>
        /// Specifies that the <see cref="T:System.Windows.Controls.Expander" />
        /// control opens in the down direction.
        /// </summary>
        Down = 0,

        /// <summary>
        /// Specifies that the <see cref="T:System.Windows.Controls.Expander" />
        /// control opens in the up direction.
        /// </summary>
        Up = 1,

        /// <summary>
        /// Specifies that the <see cref="T:System.Windows.Controls.Expander" />
        /// control opens in the left direction.
        /// </summary>
        Left = 2,

        /// <summary>
        /// Specifies that the <see cref="T:System.Windows.Controls.Expander" />
        /// control opens in the right direction.
        /// </summary>
        Right = 3,
    }
}
