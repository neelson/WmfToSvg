using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Svg
{
    /// <summary>
    /// Scalable Vector Graphics - SVG base object.
    /// </summary>
    public abstract class SvgObject
    {
        #region Local Variables

        private SvgGdi _gdi;

        #endregion

        #region Properties

        /// <summary>
        /// Gdi object instance.
        /// </summary>
        /// <returns></returns>
        public SvgGdi GDI
        {
            get
            {
                return _gdi;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="gdi"></param>
        public SvgObject(SvgGdi gdi)
        {
            _gdi = gdi;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Convert to real size.
        /// </summary>
        /// <param name="px"></param>
        /// <returns></returns>
        public int ToRealSize(int px)
        {
            return GDI.DC.Dpi * px / 90;
        }

        /// <summary>
        /// Convert int color to rgb color.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ToColor(int color)
        {
            int b = (0x00FF0000 & color) >> 16;
            int g = (0x0000FF00 & color) >> 8;
            int r = (0x000000FF & color);

            return "rgb(" + r + "," + g + "," + b + ")";
        }

        #endregion
    }
}
