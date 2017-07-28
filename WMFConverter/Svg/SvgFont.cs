using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Svg
{
    /// <summary>
    /// Scalable Vector Graphics - Represents a SVG Font object.
    /// </summary>
    public class SvgFont : SvgObject,Gdi.IGdiFont
    {
        #region Properties

        /// <summary>
        /// Object height.
        /// </summary>
        public int Height
        {
            get
            {
                return _height;
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
        /// Object escapement.
        /// </summary>
        public int Escapement
        {
            get
            {
                return _escapement;
            }
        }

        /// <summary>
        /// Object orientation
        /// </summary>
        public int Orientation
        {
            get
            {
                return _orientation;
            }
        }

        /// <summary>
        /// Object weight.
        /// </summary>
        public int Weight
        {
            get
            {
                return _weight;
            }
        }

        /// <summary>
        /// Italic font.
        /// </summary>
        public bool IsItalic
        {
            get
            {
                return _italic;
            }
        }

        /// <summary>
        /// Underlined font.
        /// </summary>
        public bool IsUnderlined
        {
            get
            {
                return _underline;
            }
        }

        /// <summary>
        /// Striked font.
        /// </summary>
        public bool IsStrikedOut
        {
            get
            {
                return _strikeout;
            }
        }

        /// <summary>
        /// Object charset.
        /// </summary>
        public int Charset
        {
            get
            {
                return _charset;
            }
        }

        /// <summary>
        /// Object precision.
        /// </summary>
        public int OutPrecision
        {
            get
            {
                return _outPrecision;
            }
        }

        /// <summary>
        /// Object ClipPrecision.
        /// </summary>
        public int ClipPrecision
        {
            get
            {
                return _clipPrecision;
            }
        }

        /// <summary>
        /// Font quality.
        /// </summary>
        public int Quality
        {
            get
            {
                return _quality;
            }
        }

        /// <summary>
        /// Defines pitch and family.
        /// </summary>
        public int PitchAndFamily
        {
            get
            {
                return _pitchAndFamily;
            }
        }

        /// <summary>
        /// Specifies the font name.
        /// </summary>
        public string FaceName
        {
            get
            {
                return _faceName;
            }
        }

        /// <summary>
        /// Font language.
        /// </summary>
        public string Lang
        {
            get
            {
                return _lang;
            }
        }
        
        /// <summary>
        /// Font size.
        /// </summary>
        public int FontSize
        {
            get
            {
                return Math.Abs((int)GDI.DC.ToRelativeY(_height * _heightMultiply));
            }
        }

        #endregion

        #region Local Variables

        private int _height;
        private int _width;
        private int _escapement;
        private int _orientation;
        private int _weight;
        private bool _italic;
        private bool _underline;
        private bool _strikeout;
        private int _charset;
        private int _outPrecision;
        private int _clipPrecision;
        private int _quality;
        private int _pitchAndFamily;

        private string _faceName;
        private double _heightMultiply = 1.0;
        private string _lang;

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="gdi"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="escapement"></param>
        /// <param name="orientation"></param>
        /// <param name="weight"></param>
        /// <param name="italic"></param>
        /// <param name="underline"></param>
        /// <param name="strikeout"></param>
        /// <param name="charset"></param>
        /// <param name="outPrecision"></param>
        /// <param name="clipPrecision"></param>
        /// <param name="quality"></param>
        /// <param name="pitchAndFamily"></param>
        /// <param name="faceName"></param>
        public SvgFont(
            SvgGdi gdi,
            int height,
            int width,
            int escapement,
            int orientation,
            int weight,
            bool italic,
            bool underline,
            bool strikeout,
            int charset,
            int outPrecision,
            int clipPrecision,
            int quality,
            int pitchAndFamily,
            byte[] faceName)
            : base (gdi)
        {
            _height = height;
            _width = width;
            _escapement = escapement;
            _orientation = orientation;
            _weight = weight;
            _italic = italic;
            _underline = underline;
            _strikeout = strikeout;
            _charset = charset;
            _outPrecision = outPrecision;
            _clipPrecision = clipPrecision;
            _quality = quality;
            _pitchAndFamily = pitchAndFamily;
            _faceName = WMFConverter.Gdi.GdiUtils.ConvertString(faceName, charset);

            // xml:lang
            _lang = WMFConverter.Gdi.GdiUtils.GetLanguage(charset);

            string emheight = gdi.GetProperty("font-emheight." + _faceName);
            if (emheight == null)
            {
                string alter = gdi.GetProperty("alternative-font." + _faceName);
                if (alter != null)
                {
                    emheight = gdi.GetProperty("font-emheight." + alter);
                }
            }

            if (emheight != null)
            {
                _heightMultiply = Convert.ToDouble(emheight);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="dx"></param>
        /// <returns></returns>
        public int[] ValidateDx(byte[] chars, int[] dx)
        {
            if (dx == null || dx.Length == 0)
            {
                return null;
            }

            int[,] area = WMFConverter.Gdi.GdiUtils.GetFirstByteArea(_charset);
            if (area == null)
            {
                return dx;
            }

            int n = 0;
            bool skip = false;

            for (int i = 0; i < chars.Length && i < dx.Length; i++)
            {
                int c = (0xFF & chars[i]);

                if (skip)
                {
                    dx[n - 1] += dx[i];
                    skip = false;
                    continue;
                }

                for (int j = 0; j < area.Length; j++)
                {
                    if (area[j,0] <= c && c <= area[j,1])
                    {
                        skip = true;
                        break;
                    }
                }

                dx[n++] = dx[i];
            }

            int[] ndx = new int[n];
            Array.Copy(dx, 0, ndx, 0, n);

            return ndx;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() 
        {
		    int PRIME = 31;
		    int result = 1;
		    result = PRIME * result + _charset;
		    result = PRIME * result + _clipPrecision;
		    result = PRIME * result + _escapement;
		    result = PRIME * result + ((_faceName == null) ? 0 : _faceName.GetHashCode());
		    result = PRIME * result + _height;
		    result = PRIME * result + (_italic ? 1231 : 1237);
		    result = PRIME * result + _orientation;
		    result = PRIME * result + _outPrecision;
		    result = PRIME * result + _pitchAndFamily;
		    result = PRIME * result + _quality;
		    result = PRIME * result + (_strikeout ? 1231 : 1237);
		    result = PRIME * result + (_underline ? 1231 : 1237);
		    result = PRIME * result + _weight;
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
		    if (typeof(SvgFont) != obj.GetType())
			    return false;
		    SvgFont other = (SvgFont) obj;
		    if (_charset != other._charset)
			    return false;
		    if (_clipPrecision != other._clipPrecision)
			    return false;
		    if (_escapement != other._escapement)
			    return false;
		    if (_faceName == null) 
            {
			    if (other._faceName != null)
				    return false;
		    } 
            else if (!_faceName.Equals(other._faceName))
			    return false;
		    if (_height != other._height)
			    return false;
		    if (_italic != other._italic)
			    return false;
		    if (_orientation != other._orientation)
			    return false;
		    if (_outPrecision != other._outPrecision)
			    return false;
		    if (_pitchAndFamily != other._pitchAndFamily)
			    return false;
		    if (_quality != other._quality)
			    return false;
		    if (_strikeout != other._strikeout)
			    return false;
		    if (_underline != other._underline)
			    return false;
		    if (_weight != other._weight)
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

            // font-style
            if (_italic)
                buffer.Append("font-style: italic; ");

            // font-weight
            if (_weight != (int)Gdi.FontFWEnum.FW_DONTCARE && _weight != (int)Gdi.FontFWEnum.FW_NORMAL)
            {
                if (_weight < 100)
                    _weight = 100;
                else if (_weight > 900)
                    _weight = 900;
                else
                    _weight = (_weight / 100) * 100;

                if (_weight == (int)Gdi.FontFWEnum.FW_BOLD)
                    buffer.Append("font-weight: bold; ");
                else
                    buffer.Append("font-weight: " + _weight + "; ");
            }

            int fontSize = FontSize;
            if (fontSize != 0) 
                buffer.Append("font-size: ").Append(fontSize).Append("px; ");

            // font-family
            List<string> fontList = new List<string>();
            if (_faceName.Length != 0)
            {
                string fontFamily = _faceName;
                if (_faceName.ElementAt(0) == '@') fontFamily = _faceName.Substring(1);
                fontList.Add(fontFamily);

                string altfont = GDI.GetProperty("alternative-font." + fontFamily);
                if (altfont != null && altfont.Length != 0)
                {
                    fontList.Add(altfont);
                }
            }

            // int pitch = pitchAndFamily & 0x00000003;
            int family = _pitchAndFamily & 0x000000F0;
            switch (family)
            {
                case (int)Gdi.FontFFEnum.FF_DECORATIVE:
                    fontList.Add("fantasy");
                    break;
                case (int)Gdi.FontFFEnum.FF_MODERN:
                    fontList.Add("monospace");
                    break;
                case (int)Gdi.FontFFEnum.FF_ROMAN:
                    fontList.Add("serif");
                    break;
                case (int)Gdi.FontFFEnum.FF_SCRIPT:
                    fontList.Add("cursive");
                    break;
                case (int)Gdi.FontFFEnum.FF_SWISS:
                    fontList.Add("sans-serif");
                    break;
            }

            if (fontList != null)
                if (fontList.Count()>0)
                {
                    buffer.Append("font-family:");
                    for (int i = 0; i < fontList.Count(); i ++ )
                    {
                        string font = fontList[i];
                        if (font.IndexOf(" ") != -1)
                            buffer.Append(" \"" + font + "\"");
                        else
                            buffer.Append(" " + font);

                        if (i < fontList.Count())
                            buffer.Append(",");
                    }
                    buffer.Append("; ");
                }

            // text-decoration
            if (_underline || _strikeout)
            {
                buffer.Append("text-decoration:");
                if (_underline)
                    buffer.Append(" underline");
                if (_strikeout)
                    buffer.Append(" overline");
                buffer.Append("; ");
            }

            if (buffer.Length > 0) 
                buffer.Length = buffer.Length - 1;
            return buffer.ToString();
        }

        #endregion
    }
}
