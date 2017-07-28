using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Svg
{
    /// <summary>
    /// Scalable Vector Graphics - Represents a Pen object.
    /// </summary>
    public class SvgPen: SvgObject,Gdi.IGdiPen
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
        /// Construdor padrão.
        /// </summary>
        /// <param name="gdi"></param>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="color"></param>
        public SvgPen(
            SvgGdi gdi,
            int style,
            int width,
            int color)
            :base(gdi)
        {
            _style = style;
            _width = (width > 0) ? width : 1;
            _color = color;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() 
        {
		    int PRIME = 31;
		    int result = 1;
		    result = PRIME * result + _color;
		    result = PRIME * result + _style;
		    result = PRIME * result + _width;
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
		    if (typeof(SvgPen) != obj.GetType())
			    return false;
		    SvgPen other = (SvgPen) obj;
		    if (_color != other._color)
			    return false;
		    if (_style != other._style)
			    return false;
		    if (_width != other._width)
			    return false;
		    return true;
	    }

        /// <summary>
        /// Create inner text element.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public System.Xml.XmlText CreateTextNode(String id)
        {
            return GDI.Document.CreateTextNode("." + id + " { " + ToString() + " }\r\n");
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder();

            if (_style == (int)Gdi.PenPSEnum.PS_NULL)
            {
                buffer.Append("stroke: none; ");
            }
            else
            {
                // stroke
                buffer.Append("stroke: " + ToColor(_color) + "; ");

                // stroke-width
                buffer.Append("stroke-width: " + _width + "; ");

                // stroke-linejoin
                buffer.Append("stroke-linejoin: round; ");

                // stroke-dasharray
                if (_width == 1 && (int)Gdi.PenPSEnum.PS_DASH <= _style && _style <= (int)Gdi.PenPSEnum.PS_DASHDOTDOT)
                {
                    buffer.Append("stroke-dasharray: ");
                    switch (_style)
                    {
                        case (int)Gdi.PenPSEnum.PS_DASH:
                            buffer.Append(
                                "" + ToRealSize(18) + "," + ToRealSize(6));
                            break;
                        case (int)Gdi.PenPSEnum.PS_DOT:
                            buffer.Append("" + ToRealSize(3) + "," + ToRealSize(3));
                            break;
                        case (int)Gdi.PenPSEnum.PS_DASHDOT:
                            buffer.Append(
                                ""
                                    + ToRealSize(9)
                                    + ","
                                    + ToRealSize(3)
                                    + ","
                                    + ToRealSize(3)
                                    + ","
                                    + ToRealSize(3));
                            break;
                        case (int)Gdi.PenPSEnum.PS_DASHDOTDOT:
                            buffer.Append(
                                ""
                                    + ToRealSize(9)
                                    + ","
                                    + ToRealSize(3)
                                    + ","
                                    + ToRealSize(3)
                                    + ","
                                    + ToRealSize(3)
                                    + ","
                                    + ToRealSize(3)
                                    + ","
                                    + ToRealSize(3));
                            break;
                    }
                    buffer.Append("; ");
                }
            }

            if (buffer.Length > 0) 
                buffer.Length = buffer.Length - 1;
            return buffer.ToString();
        }

        #endregion
    }
}
