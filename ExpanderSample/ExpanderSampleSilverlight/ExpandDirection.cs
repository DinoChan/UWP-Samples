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

namespace ExpanderSampleSilverlight
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
