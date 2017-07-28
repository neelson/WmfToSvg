using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Wmf
{
    /// <summary>
    /// Windows Metafile - Representes WMF RecRegion object.
    /// </summary>
    public class WmfRectRegion: WmfObject, Gdi.IGdiRegion
    {
        #region Local Variables

        private int _left;
        private int _top;
        private int _right;
        private int _bottom;

        #endregion

        #region Properties
        
        /// <summary>
        /// Get left value.
        /// </summary>
        public int Left
        {
            get
            {
                return _left;
            }
        }

        /// <summary>
        /// Get top value.
        /// </summary>
        public int Top
        {
            get
            {
                return _top;
            }
        }

        /// <summary>
        /// Get right value.
        /// </summary>
        public int Right
        {
            get
            {
                return _right;
            }
        }

        /// <summary>
        /// Get Bottom value.
        /// </summary>
        public int Bottom
        {
            get
            {
                return _bottom;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public WmfRectRegion(int id, int left, int top, int right, int bottom)
            :base(id)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        #endregion
    }
}
