using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Svg
{
    /// <summary>
    /// Scalable Vector Graphics - Represents a Brush SVG object.
    /// </summary>
    public class SvgBrush : SvgObject, Gdi.IGdiBrush
    {
        #region Local Variables 

        private int _style;
        private int _color;
        private int _hatch;

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
        /// Object color.
        /// </summary>
        public int Color
        {
            get
            {
                return _color;
            }
        }

        /// <summary>
        /// Object hatch.
        /// </summary>
        public int Hatch
        {
            get
            {
                return _hatch;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="gdi"></param>
        /// <param name="style"></param>
        /// <param name="color"></param>
        /// <param name="hatch"></param>
        public SvgBrush(
            SvgGdi gdi,
            int style,
            int color,
            int hatch)
            : base(gdi)
        {
            _style = style;
            _color = color;
            _hatch = hatch;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create element FillPatern.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public System.Xml.XmlElement CreateFillPattern(string id)
        {
            System.Xml.XmlElement pattern = GDI.Document.CreateElement("pattern");

            if (_style == (int)Gdi.BrushBSEnum.BS_HATCHED)
            {
                pattern.SetAttribute("id", id);
                pattern.SetAttribute("patternUnits", "userSpaceOnUse");
                pattern.SetAttribute("x", "" + ToRealSize(0));
                pattern.SetAttribute("y", "" + ToRealSize(0));
                pattern.SetAttribute("width", "" + ToRealSize(8));
                pattern.SetAttribute("height", "" + ToRealSize(8));
                GDI.Document.AppendChild(pattern);

                if (GDI.DC.BkMode == (int)Gdi.GdiEnum.OPAQUE)
                {
                    System.Xml.XmlElement rect = GDI.Document.CreateElement("rect");
                    rect.SetAttribute("fill", ToColor(GDI.DC.BkColor));
                    rect.SetAttribute("x", "" + ToRealSize(0));
                    rect.SetAttribute("y", "" + ToRealSize(0));
                    rect.SetAttribute("width", "" + ToRealSize(8));
                    rect.SetAttribute("height", "" + ToRealSize(8));
                    pattern.AppendChild(rect);
                }

                switch (_hatch)
                {
                    case (int)Gdi.BrushHSEnum.HS_HORIZONTAL:
                        {
                            System.Xml.XmlElement path = GDI.Document.CreateElement("line");
                            path.SetAttribute("stroke", ToColor(_color));
                            path.SetAttribute("x1", "" + ToRealSize(0));
                            path.SetAttribute("y1", "" + ToRealSize(4));
                            path.SetAttribute("x2", "" + ToRealSize(8));
                            path.SetAttribute("y2", "" + ToRealSize(4));
                            pattern.AppendChild(path);
                        } break;
                    case (int)Gdi.BrushHSEnum.HS_VERTICAL:
                        {
                            System.Xml.XmlElement path = GDI.Document.CreateElement("line");
                            path.SetAttribute("stroke", ToColor(_color));
                            path.SetAttribute("x1", "" + ToRealSize(4));
                            path.SetAttribute("y1", "" + ToRealSize(0));
                            path.SetAttribute("x2", "" + ToRealSize(4));
                            path.SetAttribute("y2", "" + ToRealSize(8));
                            pattern.AppendChild(path);
                        } break;
                    case (int)Gdi.BrushHSEnum.HS_FDIAGONAL:
                        {
                            System.Xml.XmlElement path = GDI.Document.CreateElement("line");
                            path.SetAttribute("stroke", ToColor(_color));
                            path.SetAttribute("x1", "" + ToRealSize(0));
                            path.SetAttribute("y1", "" + ToRealSize(0));
                            path.SetAttribute("x2", "" + ToRealSize(8));
                            path.SetAttribute("y2", "" + ToRealSize(8));
                            pattern.AppendChild(path);
                        } break;
                    case (int)Gdi.BrushHSEnum.HS_BDIAGONAL:
                        {
                            System.Xml.XmlElement path = GDI.Document.CreateElement("line");
                            path.SetAttribute("stroke", ToColor(_color));
                            path.SetAttribute("x1", "" + ToRealSize(0));
                            path.SetAttribute("y1", "" + ToRealSize(8));
                            path.SetAttribute("x2", "" + ToRealSize(8));
                            path.SetAttribute("y2", "" + ToRealSize(0));
                            pattern.AppendChild(path);
                        } break;
                    case (int)Gdi.BrushHSEnum.HS_CROSS:
                        {
                            System.Xml.XmlElement path1 = GDI.Document.CreateElement("line");
                            path1.SetAttribute("stroke", ToColor(_color));
                            path1.SetAttribute("x1", "" + ToRealSize(0));
                            path1.SetAttribute("y1", "" + ToRealSize(4));
                            path1.SetAttribute("x2", "" + ToRealSize(8));
                            path1.SetAttribute("y2", "" + ToRealSize(4));
                            pattern.AppendChild(path1);
                            System.Xml.XmlElement path2 = GDI.Document.CreateElement("line");
                            path2.SetAttribute("stroke", ToColor(_color));
                            path2.SetAttribute("x1", "" + ToRealSize(4));
                            path2.SetAttribute("y1", "" + ToRealSize(0));
                            path2.SetAttribute("x2", "" + ToRealSize(4));
                            path2.SetAttribute("y2", "" + ToRealSize(8));
                            pattern.AppendChild(path2);
                        } break;
                    case (int)Gdi.BrushHSEnum.HS_DIAGCROSS:
                        {
                            System.Xml.XmlElement path1 = GDI.Document.CreateElement("line");
                            path1.SetAttribute("stroke", ToColor(_color));
                            path1.SetAttribute("x1", "" + ToRealSize(0));
                            path1.SetAttribute("y1", "" + ToRealSize(0));
                            path1.SetAttribute("x2", "" + ToRealSize(8));
                            path1.SetAttribute("y2", "" + ToRealSize(8));
                            pattern.AppendChild(path1);
                            System.Xml.XmlElement path2 = GDI.Document.CreateElement("line");
                            path2.SetAttribute("stroke", ToColor(_color));
                            path2.SetAttribute("x1", "" + ToRealSize(0));
                            path2.SetAttribute("y1", "" + ToRealSize(8));
                            path2.SetAttribute("x2", "" + ToRealSize(8));
                            path2.SetAttribute("y2", "" + ToRealSize(0));
                            pattern.AppendChild(path2);
                        } break;
                }
            }

            return pattern;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int PRIME = 31;
            int result = 1;
            result = PRIME * result + _color;
            result = PRIME * result + _hatch;
            result = PRIME * result + _style;
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
            if (typeof(SvgBrush) != obj.GetType())
                return false;
            SvgBrush other = (SvgBrush)obj;
            if (_color != other._color)
                return false;
            if (_hatch != other._hatch)
                return false;
            if (_style != other._style)
                return false;
            return true;
        }

        /// <summary>
        /// Create element inner text.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public System.Xml.XmlText CreateTextNode(string id)
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

            // fill
            switch (_style)
            {
                case (int)Gdi.BrushBSEnum.BS_SOLID:
                    buffer.Append("fill: ").Append(ToColor(_color)).Append("; ");
                    break;
                case (int)Gdi.BrushBSEnum.BS_HATCHED:
                    break;
                default:
                    buffer.Append("fill: none; ");
                    break;
            }

            if (buffer.Length > 0)
                buffer.Length = buffer.Length - 1;
            return buffer.ToString();
        }

        #endregion
    }
}
