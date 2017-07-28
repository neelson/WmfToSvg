using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Wmf
{
    /// <summary>
    /// Windows Metafile - Represents WMF Brush object.
    /// </summary>
    public class WmfBrush : WmfObject, Gdi.IGdiBrush
    {
        #region Local Variables

        private int _style;
        private int _color;
        private int _hatch;

        #endregion

        #region Properties

        /// <summary>
        /// Object Style.
        /// </summary>
        public int Style
        {
            get
            {
                return _style;
            }
        }

        /// <summary>
        /// Object Color.
        /// </summary>
        public int Color
        {
            get
            {
                return _color;
            }
        }

        /// <summary>
        /// Object Hatch.
        /// </summary>
        public int Hatch
        {
            get
            {
                return _hatch;
            }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="style"></param>
        /// <param name="color"></param>
        /// <param name="hatch"></param>
        public WmfBrush(int id, int style, int color, int hatch)
            :base (id)
        {
            _style = style;
            _color = color;
            _hatch = hatch;
        }

        #endregion
    }
}
