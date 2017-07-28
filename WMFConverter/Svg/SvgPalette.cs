using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Svg
{
    /// <summary>
    /// Scalable Vector Graphics - Represents a SVG Palette object.
    /// </summary>
    public class SvgPalette: SvgObject, Gdi.IGdiPalette
    {
        #region Local Variables

        private int _version;
        private int[] _entries;

        #endregion

        #region Properties

        /// <summary>
        /// Object version.
        /// </summary>
        public int Version
        {
            get
            {
                return _version;
            }
        }

        /// <summary>
        /// Object entries.
        /// </summary>
        public int[] Entries
        {
            get
            {
                return _entries;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="gdi"></param>
        /// <param name="version"></param>
        /// <param name="entries"></param>
        public SvgPalette(
            SvgGdi gdi,
            int version,
            int[] entries)
            :base(gdi)
        {
            _version = version;
            _entries = entries;
        }

        #endregion
    }
}
