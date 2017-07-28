using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Gdi
{
    /// <summary>
    /// Graphics Device Interface - Represents a Pen object.
    /// </summary>
    public interface IGdiPen : IGdiObject
    {
        #region Properties

        /// <summary>
        /// Object Style.
        /// </summary>
        int Style { get; }

        /// <summary>
        /// Object Width.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Object Color.
        /// </summary>
        int Color { get; }

        #endregion
    }
}
