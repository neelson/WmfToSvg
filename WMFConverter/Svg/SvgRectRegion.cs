using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Svg
{
    /// <summary>
    /// Scalable Vector Graphics - Represents SVG Rect Region object.
    /// </summary>
    public class SvgRectRegion : SvgRegion
    {
        #region Local Variables

        private int _left;
        private int _top;
        private int _right;
        private int _bottom;

        #endregion

        #region Properties

        /// <summary>
        /// Left value.
        /// </summary>
        public int Left
        {
            get
            {
                return _left;
            }
        }

        /// <summary>
        /// Top value.
        /// </summary>
        public int Top
        {
            get
            {
                return _top;
            }
        }

        /// <summary>
        /// Right value.
        /// </summary>
        public int Right
        {
            get
            {
                return _right;
            }
        }

        /// <summary>
        /// Bottom value.
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
        /// <param name="gdi"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public SvgRectRegion(SvgGdi gdi, int left, int top, int right, int bottom)
            : base(gdi)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create rect element.
        /// </summary>
        /// <returns></returns>
        public override System.Xml.XmlElement CreateElement()
        {
            System.Xml.XmlElement elem = GDI.Document.CreateElement("rect");
            elem.SetAttribute("x", "" + (int)GDI.DC.ToAbsoluteX(Left));
            elem.SetAttribute("y", "" + (int)GDI.DC.ToAbsoluteY(Top));
            elem.SetAttribute("width", "" + (int)GDI.DC.ToRelativeX(Right - Left));
            elem.SetAttribute("height", "" + (int)GDI.DC.ToRelativeY(Bottom - Top));
            return elem;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
		    int prime = 31;
		    int result = 1;
		    result = prime * result + _bottom;
		    result = prime * result + _left;
		    result = prime * result + _right;
		    result = prime * result + _top;
		    return result;
	    }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (typeof(SvgRectRegion) != obj.GetType())
                return false;
            SvgRectRegion other = (SvgRectRegion)obj;
            if (_bottom != other._bottom)
                return false;
            if (_left != other._left)
                return false;
            if (_right != other._right)
                return false;
            if (_top != other._top)
                return false;
            return true;
        }

        #endregion
    }
}
