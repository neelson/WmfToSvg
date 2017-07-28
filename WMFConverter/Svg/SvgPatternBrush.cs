using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Svg
{
    /// <summary>
    /// Scalable Vector Graphics - Represents a SVG Pattern Brush object.
    /// </summary>
    public class SvgPatternBrush : SvgObject, Gdi.IGdiPatternBrush
    {
        #region Local Variables

        private byte[] _bmp;

        #endregion

        #region Properties

        /// <summary>
        /// Object patterns
        /// </summary>
        public byte[] Pattern
        {
            get
            {
                return _bmp;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="gdi"></param>
        /// <param name="bmp"></param>
        public SvgPatternBrush(SvgGdi gdi, byte[] bmp)
            : base(gdi)
        {
            _bmp = bmp;
        }

        #endregion
    }
}
