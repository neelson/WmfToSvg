using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Gdi
{
    /// <summary>
    /// Graphics Device Interface - Represents a Brush object.
    /// </summary>
    public interface IGdiBrush : IGdiObject
    {
        #region Properties

        /// <summary>
        /// Object Style.
        /// </summary>
        /// <returns></returns>
        int Style { get; }

        /// <summary>
        /// Object Color.
        /// </summary>
        /// <returns></returns>
        int Color {get;}

        /// <summary>
        /// Object Hatch.
        /// </summary>
        /// <returns></returns>
        int Hatch { get; }

        #endregion
    }
}
