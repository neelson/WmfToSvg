using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Gdi
{
    /// <summary>
    /// Graphics Device Interface - Represents a Pattern Brush object.
    /// </summary>
    public interface IGdiPatternBrush : IGdiObject
    {
        #region Properties

        /// <summary>
        /// Object Patterns.
        /// </summary>
        /// <returns></returns>
        byte[] Pattern { get; }

        #endregion
    }
}
