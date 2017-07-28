using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Gdi
{
    /// <summary>
    /// Graphics Device Interface - Represents a Palette object.
    /// </summary>
    public interface IGdiPalette : IGdiObject
    {
        #region Properties

        /// <summary>
        /// Object Version.
        /// </summary>
        /// <returns></returns>
        int Version { get; }

        /// <summary>
        /// Object Entries.
        /// </summary>
        /// <returns></returns>
        int[] Entries { get; }

        #endregion
    }
}
