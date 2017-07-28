using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Wmf
{
    /// <summary>
    /// Windows Metafile - Represents WMF Pen object.
    /// </summary>
    class WmfPen: WmfObject,Gdi.IGdiPen
    {
        #region Local Variables

        private int _style;
        private int _width;
        private int _color;

        #endregion

        #region Properties

        /// <summary>
        /// Object style.
        /// </summary>
        public int Style
        {
            get
            {
                return _style;
            }
        }

        /// <summary>
        /// Object width.
        /// </summary>
        public int Width
        {
            get
            {
                return _width;
            }
        }

        /// <summary>
        /// Object color.
        /// </summary>
        public int Color
        {
            get
            {
                return _color;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="color"></param>
        public WmfPen(int id, int style, int width, int color)
            : base(id)
        {
        }

        #endregion
    }
}
