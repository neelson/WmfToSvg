using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Gdi
{
    /// <summary>
    /// Graphics Device Interface - Represents a Font object.
    /// </summary>
    public interface IGdiFont : IGdiObject
    {
        #region Properties

        /// <summary>
        /// Object Height.
        /// </summary>
        /// <returns></returns>
        int Height {get;}

        /// <summary>
        /// Object Width.
        /// </summary>
        /// <returns></returns>
        int Width { get; }

        /// <summary>
        /// Object Escapement.
        /// </summary>
        /// <returns></returns>
        int Escapement { get; }

        /// <summary>
        /// Object Orientation.
        /// </summary>
        /// <returns></returns>
        int Orientation { get; }

        /// <summary>
        /// Object Weight.
        /// </summary>
        /// <returns></returns>
        int Weight { get; }

        /// <summary>
        /// Italic font.
        /// </summary>
        /// <returns></returns>
        bool IsItalic { get; }

        /// <summary>
        /// Underlined font.
        /// </summary>
        /// <returns></returns>
        bool IsUnderlined { get; }

        /// <summary>
        /// Striked font.
        /// </summary>
        /// <returns></returns>
        bool IsStrikedOut { get; }

        /// <summary>
        /// Object Charset.
        /// </summary>
        /// <returns></returns>
        int Charset { get; }

        /// <summary>
        /// Object out precision.
        /// </summary>
        /// <returns></returns>
        int OutPrecision { get; }

        /// <summary>
        /// Object clip precision.
        /// </summary>
        /// <returns></returns>
        int ClipPrecision { get; }

        /// <summary>
        /// Object Quality.
        /// </summary>
        /// <returns></returns>
        int Quality { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int PitchAndFamily { get; }

        /// <summary>
        /// Font face name.
        /// </summary>
        /// <returns></returns>
        string FaceName { get; }

        #endregion
    }
}
