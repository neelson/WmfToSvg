using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Svg
{
    /// <summary>
    /// Represents a SVG object of GDI object.
    /// </summary>
    public class SvgGdi : Gdi.IGdi
    {
        #region Local Variables

        private bool _compatible;

        private bool _replaceSymbolFont = false;

        private Properties _props = new Properties();

        private SvgDc _dc;

        private LinkedList<SvgDc> _saveDC = new LinkedList<SvgDc>();

        private System.Xml.XmlDocument _doc = null;

        private System.Xml.XmlElement _parentNode = null;

        private System.Xml.XmlElement _styleNode = null;

        private System.Xml.XmlElement _defsNode = null;

        private int _brushNo = 0;

        private int _fontNo = 0;

        private int _penNo = 0;

        private int _patternNo = 0;

        private int _rgnNo = 0;

        private int _clipPathNo = 0;

        private int _maskNo = 0;

        private Dictionary<WMFConverter.Gdi.IGdiObject, string> _nameMap = new Dictionary<WMFConverter.Gdi.IGdiObject, string>();

        private System.Text.StringBuilder _buffer = new System.Text.StringBuilder();

        private SvgBrush _defaultBrush;

        private SvgPen _defaultPen;

        private SvgFont _defaultFont;

        #endregion

        #region Properties

        /// <summary>
        /// SVG Document - XML
        /// </summary>
        public System.Xml.XmlDocument Document
        {
            get
            {
                return _doc;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Xml.XmlElement DefsElement
        {
            get
            {
                return _defsNode;
            }
        }

        /// <summary>
        /// Style node element.
        /// </summary>
        public System.Xml.XmlElement StyleElement
        {
            get
            {
                return _styleNode;
            }
        }

        /// <summary>
        /// IE9-compatible style output
        /// </summary>
        public bool Compatible
        {
            get
            {
                return _compatible;
            }
            set
            {
                _compatible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ReplaceSymbolFont
        {
            get
            {
                return _replaceSymbolFont;
            }
            set
            {
                _replaceSymbolFont = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SvgDc DC
        {
            get
            {
                return _dc;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SvgGdi() 
            : this(false)
        {

        }

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="compatible">IE9-compatible style output</param>
        public SvgGdi(bool compatible) 
        {
                _compatible = compatible;

                _doc = new System.Xml.XmlDocument();
                var node = _doc.CreateXmlDeclaration("1.0", "UTF-8", "no");
                _doc.AppendChild(node);

                var doctype = _doc.CreateDocumentType("svg", "-//W3C//DTD SVG 1.0//EN", "", null);
                _doc.AppendChild(doctype);
                var svg = _doc.CreateElement("svg","http://www.w3.org/2000/svg");
                _doc.AppendChild(svg);
                _props.load();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Write SVG XML document.
        /// </summary>
        /// <param name="output"></param>
        public void Write(System.IO.Stream output)  
        {
             var writer = System.Xml.XmlWriter.Create(output, new System.Xml.XmlWriterSettings
                {
                    Indent = true,
                    NamespaceHandling = System.Xml.NamespaceHandling.OmitDuplicates
                });
            _doc.Save(writer);
            writer.Flush();
        }

        /// <summary>
        /// Get property value using key value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetProperty(string key)
        {
            string outstring = "";
            _props.TryGetValue(key, out outstring);
            return outstring;
        }

        /// <summary>
        /// Define header document.
        /// </summary>
        /// <param name="wsx"></param>
        /// <param name="wsy"></param>
        /// <param name="wex"></param>
        /// <param name="wey"></param>
        /// <param name="dpi"></param>
        public void PlaceableHeader(int wsx, int wsy, int wex, int wey, int dpi)
        {
            if (_parentNode == null)
                init();

            _dc.SetWindowExtEx(Math.Abs(wex - wsx), Math.Abs(wey - wsy), null);
            _dc.SetDpi(dpi);

            System.Xml.XmlElement root = _doc.DocumentElement;
            root.SetAttribute("width", (Math.Abs(wex - wsx) / (double)_dc.Dpi).ToString(System.Globalization.CultureInfo.InvariantCulture)+ "in");
            root.SetAttribute("height", (Math.Abs(wey - wsy) / (double)_dc.Dpi).ToString(System.Globalization.CultureInfo.InvariantCulture) + "in");
        }

        /// <summary>
        /// Initialize header document.
        /// </summary>
        public void Header()
        {
            if (_parentNode == null)
                init();
        }

        /// <summary>
        /// Replaces entries in the specified logical palette.
        /// </summary>
        /// <param name="palette"></param>
        /// <param name="startIndex"></param>
        /// <param name="entries"></param>
        public void AnimatePalette(Gdi.IGdiPalette palette, int startIndex, int[] entries)
        {
            // TODO
            Console.Write("not implemented: animatePalette");
        }

        /// <summary>
        /// Draws an elliptical arc.
        /// </summary>
        /// <param name="sxr"></param>
        /// <param name="syr"></param>
        /// <param name="exr"></param>
        /// <param name="eyr"></param>
        /// <param name="sxa"></param>
        /// <param name="sya"></param>
        /// <param name="exa"></param>
        /// <param name="eya"></param>
        public void Arc(int sxr, int syr, int exr, int eyr, int sxa, int sya,
                int exa, int eya)
        {

            double rx = Math.Abs(exr - sxr) / 2.0;
            double ry = Math.Abs(eyr - syr) / 2.0;
            if (rx <= 0 || ry <= 0)
                return;

            double cx = Math.Min(sxr, exr) + rx;
            double cy = Math.Min(syr, eyr) + ry;

            System.Xml.XmlElement elem = null;
            if (sxa == exa && sya == eya)
            {
                if (rx == ry)
                {
                    elem = _doc.CreateElement("circle", "http://www.w3.org/2000/svg");
                    elem.SetAttribute("cx", "" + _dc.ToAbsoluteX(cx));
                    elem.SetAttribute("cy", "" + _dc.ToAbsoluteY(cy));
                    elem.SetAttribute("r", "" + _dc.ToRelativeX(rx));
                }
                else
                {
                    elem = _doc.CreateElement("ellipse", "http://www.w3.org/2000/svg");
                    elem.SetAttribute("cx", "" + _dc.ToAbsoluteX(cx));
                    elem.SetAttribute("cy", "" + _dc.ToAbsoluteY(cy));
                    elem.SetAttribute("rx", "" + _dc.ToRelativeX(rx));
                    elem.SetAttribute("ry", "" + _dc.ToRelativeY(ry));
                }
            }
            else
            {
                double sa = Math.Atan2((sya - cy) * rx, (sxa - cx) * ry);
                double sx = rx * Math.Cos(sa);
                double sy = ry * Math.Sin(sa);

                double ea = Math.Atan2((eya - cy) * rx, (exa - cx) * ry);
                double ex = rx * Math.Cos(ea);
                double ey = ry * Math.Sin(ea);

                double a = Math.Atan2((ex - sx) * (-sy) - (ey - sy) * (-sx), (ex - sx) * (-sx) + (ey - sy) * (-sy));

                elem = _doc.CreateElement("path");
                elem.SetAttribute("d", "M " + _dc.ToAbsoluteX(sx + cx) + "," + _dc.ToAbsoluteY(sy + cy)
                        + " A " + _dc.ToRelativeX(rx) + "," + _dc.ToRelativeY(ry)
                        + " 0 " + (a > 0 ? "1" : "0") + " 0"
                        + " " + _dc.ToAbsoluteX(ex + cx) + "," + _dc.ToAbsoluteY(ey + cy));
            }

            if (_dc.Pen != null)
                elem.SetAttribute("class", GetClassString(_dc.Pen));

            elem.SetAttribute("fill", "none");
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// Execute a bit-block transfer of the color data corresponding to a rectangle of pixels from the specified source device context into a destination device context.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dw"></param>
        /// <param name="dh"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="rop"></param>
        public void BitBlt(byte[] image, int dx, int dy, int dw, int dh,
                int sx, int sy, long rop)
        {
            bmpToSvg(image, dx, dy, dw, dh, sx, sy, dw, dh, (int)Gdi.GdiEnum.DIB_RGB_COLORS, rop);
        }

        /// <summary>
        /// Draws a chord (a region bounded by the intersection of an ellipse and a line segment, called a secant).
        /// The chord is outlined by using the current pen and filled by using the current brush.
        /// </summary>
        /// <param name="sxr"></param>
        /// <param name="syr"></param>
        /// <param name="exr"></param>
        /// <param name="eyr"></param>
        /// <param name="sxa"></param>
        /// <param name="sya"></param>
        /// <param name="exa"></param>
        /// <param name="eya"></param>
        public void Chord(int sxr, int syr, int exr, int eyr, int sxa, int sya,
                int exa, int eya)
        {
            double rx = Math.Abs(exr - sxr) / 2.0;
            double ry = Math.Abs(eyr - syr) / 2.0;
            if (rx <= 0 || ry <= 0) return;

            double cx = Math.Min(sxr, exr) + rx;
            double cy = Math.Min(syr, eyr) + ry;

            System.Xml.XmlElement elem = null;
            if (sxa == exa && sya == eya)
            {
                if (rx == ry)
                {
                    elem = _doc.CreateElement("circle", "http://www.w3.org/2000/svg");
                    elem.SetAttribute("cx", "" + _dc.ToAbsoluteX(cx));
                    elem.SetAttribute("cy", "" + _dc.ToAbsoluteY(cy));
                    elem.SetAttribute("r", "" + _dc.ToRelativeX(rx));
                }
                else
                {
                    elem = _doc.CreateElement("ellipse", "http://www.w3.org/2000/svg");
                    elem.SetAttribute("cx", "" + _dc.ToAbsoluteX(cx));
                    elem.SetAttribute("cy", "" + _dc.ToAbsoluteY(cy));
                    elem.SetAttribute("rx", "" + _dc.ToRelativeX(rx));
                    elem.SetAttribute("ry", "" + _dc.ToRelativeY(ry));
                }
            }
            else
            {
                double sa = Math.Atan2((sya - cy) * rx, (sxa - cx) * ry);
                double sx = rx * Math.Cos(sa);
                double sy = ry * Math.Sin(sa);

                double ea = Math.Atan2((eya - cy) * rx, (exa - cx) * ry);
                double ex = rx * Math.Cos(ea);
                double ey = ry * Math.Sin(ea);

                double a = Math.Atan2((ex - sx) * (-sy) - (ey - sy) * (-sx), (ex - sx) * (-sx) + (ey - sy) * (-sy));

                elem = _doc.CreateElement("path", "http://www.w3.org/2000/svg");
                elem.SetAttribute("d", "M " + _dc.ToAbsoluteX(sx + cx) + "," + _dc.ToAbsoluteY(sy + cy)
                        + " A " + _dc.ToRelativeX(rx) + "," + _dc.ToRelativeY(ry)
                        + " 0 " + (a > 0 ? "1" : "0") + " 0"
                        + " " + _dc.ToAbsoluteX(ex + cx) + "," + _dc.ToAbsoluteY(ey + cy) + " Z");
            }

            if (_dc.Pen != null || _dc.Brush != null)
            {
                elem.SetAttribute("class", getClassString(_dc.Pen, _dc.Brush));
                if (_dc.Brush != null && _dc.Brush.Style == (int)Gdi.BrushBSEnum.BS_HATCHED)
                {
                    string id = "pattern" + (_patternNo++);
                    elem.SetAttribute("fill", "url(#" + id + ")");
                    _defsNode.AppendChild(_dc.Brush.CreateFillPattern(id));
                }
            }

            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// Creates a logical brush that has the specified style, color, and pattern.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="color"></param>
        /// <param name="hatch"></param>
        /// <returns></returns>
        public WMFConverter.Gdi.IGdiBrush CreateBrushIndirect(int style, int color, int hatch)
        {
            SvgBrush brush = new SvgBrush(this, style, color, hatch);
            if (!_nameMap.ContainsKey(brush))
            {
                string name = "brush" + (_brushNo++);
                _nameMap.Add(brush, name);
                _styleNode.AppendChild(brush.CreateTextNode(name));
            }
            return brush;
        }

        /// <summary>
        /// Creates a logical font that has the specified characteristics. 
        /// The font can subsequently be selected as the current font for any device context.
        /// </summary>
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
        /// <returns></returns>
        public WMFConverter.Gdi.IGdiFont CreateFontIndirect(int height, int width, int escapement,
                int orientation, int weight, bool italic, bool underline,
                bool strikeout, int charset, int outPrecision,
                int clipPrecision, int quality, int pitchAndFamily, byte[] faceName)
        {
            SvgFont font = new SvgFont(this, height, width, escapement,
                    orientation, weight, italic, underline, strikeout, charset,
                    outPrecision, clipPrecision, quality, pitchAndFamily, faceName);
            if (!_nameMap.ContainsKey(font))
            {
                string name = "font" + (_fontNo++);
                _nameMap.Add(font, name);
                _styleNode.AppendChild(font.CreateTextNode(name));
            }
            return font;
        }

        /// <summary>
        /// Creates a logical palette.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="entries"></param>
        /// <returns></returns>
        public WMFConverter.Gdi.IGdiPalette CreatePalette(int version, int[] entries)
        {
            return new SvgPalette(this, version, entries);
        }

        /// <summary>
        /// Creates a logical brush with the specified bitmap pattern.
        /// The bitmap can be a DIB section bitmap, which is created by the CreateDIBSection function, or it can be a device-dependent bitmap.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public WMFConverter.Gdi.IGdiPatternBrush CreatePatternBrush(byte[] image)
        {
            return new SvgPatternBrush(this, image);
        }

        /// <summary>
        /// Creates a logical cosmetic pen that has the style, width, and color specified in a structure.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public WMFConverter.Gdi.IGdiPen CreatePenIndirect(int style, int width, int color)
        {
            SvgPen pen = new SvgPen(this, style, width, color);
            if (!_nameMap.ContainsKey(pen))
            {
                string name = "pen" + (_penNo++);
                _nameMap.Add(pen, name);
                _styleNode.AppendChild(pen.CreateTextNode(name));
            }
            return pen;
        }

        /// <summary>
        /// Creates a rectangular region.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public WMFConverter.Gdi.IGdiRegion CreateRectRgn(int left, int top, int right, int bottom)
        {
            SvgRectRegion rgn = new SvgRectRegion(this, left, top, right, bottom);
            if (!_nameMap.ContainsKey(rgn))
            {
                _nameMap.Add(rgn, "rgn" + (_rgnNo++));
                _defsNode.AppendChild(rgn.CreateElement());
            }
            return rgn;
        }

        /// <summary>
        /// Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the object. 
        /// After the object is deleted, the specified handle is no longer valid.
        /// </summary>
        /// <param name="obj"></param>
        public void DeleteObject(WMFConverter.Gdi.IGdiObject obj)
        {
            if (_dc.Brush == obj)
                _dc.Brush = _defaultBrush;
            else if (_dc.Font == obj)
                _dc.Font = _defaultFont;
            else if (_dc.Pen == obj)
                _dc.Pen = _defaultPen;
        }

        /// <summary>
        /// Function performs a bit-block transfer of the color data corresponding to a rectangle of pixels from the specified source device context into a destination device context.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dw"></param>
        /// <param name="dh"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="rop"></param>
        public void DibBitBlt(byte[] image, int dx, int dy, int dw, int dh,
                int sx, int sy, long rop)
        {
            BitBlt(image, dx, dy, dw, dh, sx, sy, rop);
        }

        /// <summary>
        /// Create Dib Pattern Brush object instance.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="usage"></param>
        /// <returns></returns>
        public WMFConverter.Gdi.IGdiPatternBrush DibCreatePatternBrush(byte[] image, int usage)
        {
            // TODO usage
            return new SvgPatternBrush(this, image);
        }

        /// <summary>
        /// Copies a bitmap from a source rectangle into a destination rectangle, stretching or compressing the bitmap to fit the dimensions of the destination rectangle, if necessary. 
        /// The system stretches or compresses the bitmap according to the stretching mode currently set in the destination device context.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dw"></param>
        /// <param name="dh"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="sw"></param>
        /// <param name="sh"></param>
        /// <param name="rop"></param>
        public void DibStretchBlt(byte[] image, int dx, int dy, int dw, int dh,
                int sx, int sy, int sw, int sh, long rop)
        {
            this.StretchDIBits(dx, dy, dw, dh, sx, sy, sw, sh, image, (int)Gdi.GdiEnum.DIB_RGB_COLORS, rop);
        }

        /// <summary>
        /// Draws an ellipse. 
        /// The center of the ellipse is the center of the specified bounding rectangle.
        /// The ellipse is outlined by using the current pen and is filled by using the current brush.
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        public void Ellipse(int sx, int sy, int ex, int ey)
        {
            System.Xml.XmlElement elem = _doc.CreateElement("ellipse", "http://www.w3.org/2000/svg");

            if (_dc.Pen != null || _dc.Brush != null)
            {
                elem.SetAttribute("class", getClassString(_dc.Pen, _dc.Brush));
                if (_dc.Brush != null && _dc.Brush.Style == (int)Gdi.BrushBSEnum.BS_HATCHED)
                {
                    string id = "pattern" + (_patternNo++);
                    elem.SetAttribute("fill", "url(#" + id + ")");
                    _defsNode.AppendChild(_dc.Brush.CreateFillPattern(id));
                }
            }

            elem.SetAttribute("cx", "" + (int)_dc.ToAbsoluteX((sx + ex) / 2));
            elem.SetAttribute("cy", "" + (int)_dc.ToAbsoluteY((sy + ey) / 2));
            elem.SetAttribute("rx", "" + (int)_dc.ToRelativeX((ex - sx) / 2));
            elem.SetAttribute("ry", "" + (int)_dc.ToRelativeY((ey - sy) / 2));
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="data"></param>
        public void Escape(byte[] data)
        {

        }

        /// <summary>
        ///  Creates a new clipping region that consists of the existing clipping region minus the specified rectangle.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public int ExcludeClipRect(int left, int top, int right, int bottom)
        {
            System.Xml.XmlElement mask = _dc.Mask;
            if (mask != null)
            {
                mask = (System.Xml.XmlElement)mask.CloneNode(true);
                string name = "mask" + (_maskNo++);
                mask.SetAttribute("id", name);
                _defsNode.AppendChild(mask);

                System.Xml.XmlElement unclip = _doc.CreateElement("rect", "http://www.w3.org/2000/svg");
                unclip.SetAttribute("x", "" + (int)_dc.ToAbsoluteX(left));
                unclip.SetAttribute("y", "" + (int)_dc.ToAbsoluteY(top));
                unclip.SetAttribute("width", "" + (int)_dc.ToRelativeX(right - left));
                unclip.SetAttribute("height", "" + (int)_dc.ToRelativeY(bottom - top));
                unclip.SetAttribute("fill", "black");
                mask.AppendChild(unclip);
                _dc.Mask = mask;

                // TODO
                return (int)Gdi.RegionEnum.COMPLEXREGION;
            }
            else
                return (int)Gdi.RegionEnum.NULLREGION;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <param name="type"></param>
        public void ExtFloodFill(int x, int y, int color, int type)
        {
            // TODO
            Console.Write("not implemented: extFloodFill");
        }

        /// <summary>
        /// Draws text using the currently selected font, background color, and text color. 
        /// You can optionally provide dimensions to be used for clipping, opaquing, or both.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="options"></param>
        /// <param name="rect"></param>
        /// <param name="text"></param>
        /// <param name="dx"></param>
        public void ExtTextOut(int x, int y, int options, int[] rect, byte[] text, int[] dx)
        {
            System.Xml.XmlElement elem = _doc.CreateElement("text", "http://www.w3.org/2000/svg");

            int escapement = 0;
            bool vertical = false;
            if (_dc.Font != null)
            {
                elem.SetAttribute("class", GetClassString(_dc.Font));
                if (_dc.Font.FaceName.StartsWith("@"))
                {
                    vertical = true;
                    escapement = _dc.Font.Escapement - 2700;
                }
                else
                    escapement = _dc.Font.Escapement;
            }
            elem.SetAttribute("fill", SvgObject.ToColor(_dc.TextColor));

            // style
            _buffer.Length = 0;
            int align = _dc.TextAlign;

            if ((align & ((int)Gdi.GdiEnum.TA_LEFT | (int)Gdi.GdiEnum.TA_CENTER | (int)Gdi.GdiEnum.TA_RIGHT)) == (int)Gdi.GdiEnum.TA_RIGHT)
                _buffer.Append("text-anchor: end; ");
            else if ((align & ((int)Gdi.GdiEnum.TA_LEFT | (int)Gdi.GdiEnum.TA_CENTER | (int)Gdi.GdiEnum.TA_RIGHT)) == (int)Gdi.GdiEnum.TA_CENTER)
                _buffer.Append("text-anchor: middle; ");

            if (_compatible)
                _buffer.Append("dominant-baseline: alphabetic; ");
            else
            {
                if (vertical)
                    elem.SetAttribute("writing-mode", "tb");
                else
                {
                    if ((align & ((int)Gdi.GdiEnum.TA_BOTTOM | (int)Gdi.GdiEnum.TA_TOP | (int)Gdi.GdiEnum.TA_BASELINE)) == (int)Gdi.GdiEnum.TA_BASELINE)
                        _buffer.Append("dominant-baseline: alphabetic; ");
                    else
                        _buffer.Append("dominant-baseline: text-before-edge; ");
                }
            }

            if ((align & (int)Gdi.GdiEnum.TA_RTLREADING) == (int)Gdi.GdiEnum.TA_RTLREADING || (options & (int)Gdi.GdiEnum.ETO_RTLREADING) > 0)
                _buffer.Append("unicode-bidi: bidi-override; direction: rtl; ");

            if (_dc.TextSpace > 0)
                _buffer.Append("word-spacing: ").Append(_dc.TextSpace).Append("; ");

            if (_buffer.Length > 0)
                _buffer.Length = _buffer.Length - 1;
            elem.SetAttribute("style", _buffer.ToString());

            elem.SetAttribute("stroke", "none");

            if ((align & ((int)Gdi.GdiEnum.TA_NOUPDATECP | (int)Gdi.GdiEnum.TA_UPDATECP)) == (int)Gdi.GdiEnum.TA_UPDATECP)
            {
                x = _dc.CurrentX;
                y = _dc.CurrentY;
            }

            // x
            int ax = (int)_dc.ToAbsoluteX(x);
            int width = 0;
            if (vertical)
            {
                elem.SetAttribute("x", ax.ToString());
                if (_dc.Font != null)
                    width = Math.Abs(_dc.Font.FontSize);
            }
            else
            {
                if (_dc.Font != null)
                    dx = _dc.Font.ValidateDx(text, dx);

                if (dx != null && dx.Length > 0)
                {
                    for (int i = 0; i < dx.Length; i++)
                        width += dx[i];

                    int tx = x;

                    if ((align & ((int)Gdi.GdiEnum.TA_LEFT | (int)Gdi.GdiEnum.TA_CENTER | (int)Gdi.GdiEnum.TA_RIGHT)) == (int)Gdi.GdiEnum.TA_RIGHT)
                        tx -= (width - dx[dx.Length - 1]);
                    else if ((align & ((int)Gdi.GdiEnum.TA_LEFT | (int)Gdi.GdiEnum.TA_CENTER | (int)Gdi.GdiEnum.TA_RIGHT)) == (int)Gdi.GdiEnum.TA_CENTER)
                        tx -= (width - dx[dx.Length - 1]) / 2;

                    _buffer.Length = 0;
                    for (int i = 0; i < dx.Length; i++)
                    {
                        if (i > 0)
                            _buffer.Append(" ");
                        _buffer.Append((int)_dc.ToAbsoluteX(tx));
                        tx += dx[i];
                    }
                    if ((align & ((int)Gdi.GdiEnum.TA_NOUPDATECP | (int)Gdi.GdiEnum.TA_UPDATECP)) == (int)Gdi.GdiEnum.TA_UPDATECP)
                        _dc.MoveToEx(tx, y, null);
                    elem.SetAttribute("x", _buffer.ToString());
                }
                else
                {
                    if (_dc.Font != null) width = Math.Abs(_dc.Font.FontSize * text.Length) / 2;
                    elem.SetAttribute("x", ax.ToString());
                }
            }

            // y
            int ay = (int)_dc.ToAbsoluteY(y);
            int height = 0;
            if (vertical)
            {
                if (_dc.Font != null)
                    dx = _dc.Font.ValidateDx(text, dx);

                _buffer.Length = 0;
                if (align == 0)
                    _buffer.Append(ay + (int)_dc.ToRelativeY(Math.Abs(_dc.Font.Height)));
                else
                    _buffer.Append(ay);

                if (dx != null && dx.Length > 0)
                {
                    for (int i = 0; i < dx.Length - 1; i++)
                        height += dx[i];

                    int ty = y;

                    if ((align & ((int)Gdi.GdiEnum.TA_LEFT | (int)Gdi.GdiEnum.TA_CENTER | (int)Gdi.GdiEnum.TA_RIGHT)) == (int)Gdi.GdiEnum.TA_RIGHT)
                        ty -= (height - dx[dx.Length - 1]);
                    else if ((align & ((int)Gdi.GdiEnum.TA_LEFT | (int)Gdi.GdiEnum.TA_CENTER | (int)Gdi.GdiEnum.TA_RIGHT)) == (int)Gdi.GdiEnum.TA_CENTER)
                        ty -= (height - dx[dx.Length - 1]) / 2;

                    for (int i = 0; i < dx.Length; i++)
                    {
                        _buffer.Append(" ");
                        _buffer.Append((int)_dc.ToAbsoluteY(ty));
                        ty += dx[i];
                    }

                    if ((align & ((int)Gdi.GdiEnum.TA_NOUPDATECP | (int)Gdi.GdiEnum.TA_UPDATECP)) == (int)Gdi.GdiEnum.TA_UPDATECP)
                        _dc.MoveToEx(x, ty, null);
                }
                else
                {
                    if (_dc.Font != null)
                        height = Math.Abs(_dc.Font.FontSize * text.Length) / 2;
                }
                elem.SetAttribute("y", _buffer.ToString());
            }
            else
            {
                if (_dc.Font != null)
                    height = Math.Abs(_dc.Font.FontSize);
                if (_compatible)
                {
                    if ((align & ((int)Gdi.GdiEnum.TA_BOTTOM | (int)Gdi.GdiEnum.TA_TOP | (int)Gdi.GdiEnum.TA_BASELINE)) == (int)Gdi.GdiEnum.TA_TOP)
                        elem.SetAttribute("y", (ay + (int)_dc.ToRelativeY(height * 0.88)).ToString());
                    else if ((align & ((int)Gdi.GdiEnum.TA_BOTTOM | (int)Gdi.GdiEnum.TA_TOP | (int)Gdi.GdiEnum.TA_BASELINE)) == (int)Gdi.GdiEnum.TA_BOTTOM)
                        elem.SetAttribute("y", (ay + rect[3] - rect[1] + (int)_dc.ToRelativeY(height * 0.88)).ToString());
                    else
                        elem.SetAttribute("y", ay.ToString());
                }
                else
                {
                    if ((align & ((int)Gdi.GdiEnum.TA_BOTTOM | (int)Gdi.GdiEnum.TA_TOP | (int)Gdi.GdiEnum.TA_BASELINE)) == (int)Gdi.GdiEnum.TA_BOTTOM && rect != null)
                        elem.SetAttribute("y", (ay + rect[3] - rect[1] - (int)_dc.ToRelativeY(height)).ToString());
                    else
                        elem.SetAttribute("y", ay.ToString());
                }
            }

            System.Xml.XmlElement bk = null;
            if (_dc.BkMode == (int)Gdi.GdiEnum.OPAQUE || (options & (int)Gdi.GdiEnum.ETO_OPAQUE) > 0)
            {
                if (rect == null && _dc.Font != null)
                {
                    rect = new int[4];
                    if (vertical)
                    {
                        if ((align & ((int)Gdi.GdiEnum.TA_BOTTOM | (int)Gdi.GdiEnum.TA_TOP | (int)Gdi.GdiEnum.TA_BASELINE)) == (int)Gdi.GdiEnum.TA_BOTTOM)
                            rect[0] = x - width;
                        else if ((align & ((int)Gdi.GdiEnum.TA_BOTTOM | (int)Gdi.GdiEnum.TA_TOP | (int)Gdi.GdiEnum.TA_BASELINE)) == (int)Gdi.GdiEnum.TA_BASELINE)
                            rect[0] = x - (int)(width * 0.85);
                        else
                            rect[0] = x;
                        if ((align & ((int)Gdi.GdiEnum.TA_LEFT | (int)Gdi.GdiEnum.TA_RIGHT | (int)Gdi.GdiEnum.TA_CENTER)) == (int)Gdi.GdiEnum.TA_RIGHT)
                            rect[1] = y - height;
                        else if ((align & ((int)Gdi.GdiEnum.TA_LEFT | (int)Gdi.GdiEnum.TA_RIGHT | (int)Gdi.GdiEnum.TA_CENTER)) == (int)Gdi.GdiEnum.TA_CENTER)
                            rect[1] = y - height / 2;
                        else
                            rect[1] = y;
                    }
                    else
                    {
                        if ((align & ((int)Gdi.GdiEnum.TA_LEFT | (int)Gdi.GdiEnum.TA_RIGHT | (int)Gdi.GdiEnum.TA_CENTER)) == (int)Gdi.GdiEnum.TA_RIGHT)
                            rect[0] = x - width;
                        else if ((align & ((int)Gdi.GdiEnum.TA_LEFT | (int)Gdi.GdiEnum.TA_RIGHT | (int)Gdi.GdiEnum.TA_CENTER)) == (int)Gdi.GdiEnum.TA_CENTER)
                            rect[0] = x - width / 2;
                        else
                            rect[0] = x;
                        if ((align & ((int)Gdi.GdiEnum.TA_BOTTOM | (int)Gdi.GdiEnum.TA_TOP | (int)Gdi.GdiEnum.TA_BASELINE)) == (int)Gdi.GdiEnum.TA_BOTTOM)
                            rect[1] = y - height;
                        else if ((align & ((int)Gdi.GdiEnum.TA_BOTTOM | (int)Gdi.GdiEnum.TA_TOP | (int)Gdi.GdiEnum.TA_BASELINE)) == (int)Gdi.GdiEnum.TA_BASELINE)
                            rect[1] = y - (int)(height * 0.85);
                        else
                            rect[1] = y;
                    }
                    rect[2] = rect[0] + width;
                    rect[3] = rect[1] + height;
                }
                bk = _doc.CreateElement("rect", "http://www.w3.org/2000/svg");
                bk.SetAttribute("x", ((int)_dc.ToAbsoluteX(rect[0])).ToString());
                bk.SetAttribute("y", ((int)_dc.ToAbsoluteY(rect[1])).ToString());
                bk.SetAttribute("width", ((int)_dc.ToRelativeX(rect[2] - rect[0])).ToString());
                bk.SetAttribute("height", ((int)_dc.ToRelativeY(rect[3] - rect[1])).ToString());
                bk.SetAttribute("fill", SvgObject.ToColor(_dc.BkColor));
            }

            System.Xml.XmlElement clip = null;
            if ((options & (int)Gdi.GdiEnum.ETO_CLIPPED) > 0)
            {
                string name = "clipPath" + (_clipPathNo++);
                clip = _doc.CreateElement("clipPath", "http://www.w3.org/2000/svg");
                clip.SetAttribute("id", name);
                //clip.setIdAttribute("id", true);

                System.Xml.XmlElement clipRect = _doc.CreateElement("rect", "http://www.w3.org/2000/svg");
                clipRect.SetAttribute("x", ((int)_dc.ToAbsoluteX(rect[0])).ToString());
                clipRect.SetAttribute("y", ((int)_dc.ToAbsoluteY(rect[1])).ToString());
                clipRect.SetAttribute("width", ((int)_dc.ToRelativeX(rect[2] - rect[0])).ToString());
                clipRect.SetAttribute("height", ((int)_dc.ToRelativeY(rect[3] - rect[1])).ToString());

                clip.AppendChild(clipRect);
                elem.SetAttribute("clip-path", "url(#" + name + ")");
            }

            string str = null;
            if (_dc.Font != null)
                str = Gdi.GdiUtils.ConvertString(text, _dc.Font.Charset);
            else
                str = Gdi.GdiUtils.ConvertString(text, (int)Gdi.FontCharsetEnum.DEFAULT_CHARSET);

            if (_dc.Font != null && _dc.Font.Lang != null)
                elem.SetAttribute("xml:lang", _dc.Font.Lang);

            elem.SetAttribute("xml:space", "preserve");
            AppendText(elem, str);

            if (bk != null || clip != null)
            {
                System.Xml.XmlElement g = _doc.CreateElement("g", "http://www.w3.org/2000/svg");
                if (bk != null) g.AppendChild(bk);
                if (clip != null) g.AppendChild(clip);
                g.AppendChild(elem);
                elem = g;
            }

            if (escapement != 0)
                elem.SetAttribute("transform", "rotate(" + (-escapement / 10.0) + ", " + ax + ", " + ay + ")");
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// Fills a region by using the specified brush.
        /// </summary>
        /// <param name="rgn"></param>
        /// <param name="brush"></param>
        public void FillRgn(Gdi.IGdiRegion rgn, Gdi.IGdiBrush brush)
        {
            if (rgn == null) return;

            System.Xml.XmlElement elem = _doc.CreateElement("use", "http://www.w3.org/2000/svg");
            var output = "";
            _nameMap.TryGetValue(rgn, out output);
            elem.SetAttribute("xlink:href", "url(#" + output + ")");
            elem.SetAttribute("class", GetClassString(brush));
            SvgBrush sbrush = (SvgBrush)brush;
            if (sbrush.Style == (int)Gdi.BrushBSEnum.BS_HATCHED)
            {
                string id = "pattern" + (_patternNo++);
                elem.SetAttribute("fill", "url(#" + id + ")");
                _defsNode.AppendChild(sbrush.CreateFillPattern(id));
            }
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void FloodFill(int x, int y, int color)
        {
            // TODO
            Console.Write("not implemented: floodFill");
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="rgn"></param>
        /// <param name="brush"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void FrameRgn(Gdi.IGdiRegion rgn, Gdi.IGdiBrush brush, int width, int height)
        {
            // TODO
            Console.Write("not implemented: frameRgn");
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public void IntersectClipRect(int left, int top, int right, int bottom)
        {
            // TODO
            //log.fine("not implemented: intersectClipRect");
            Console.Write("not implemented: intersectClipRect");
        }

        /// <summary>
        /// Inverts the colors in the specified region.
        /// </summary>
        /// <param name="rgn"></param>
        public void InvertRgn(Gdi.IGdiRegion rgn)
        {
            if (rgn == null) return;

            System.Xml.XmlElement elem = _doc.CreateElement("use", "http://www.w3.org/2000/svg");
            var output = "";
            _nameMap.TryGetValue(rgn, out output);
            elem.SetAttribute("xlink:href", "url(#" + output + ")");
            string ropFilter = _dc.GetRopFilter((int)Gdi.GdiEnum.DSTINVERT);
            if (ropFilter != null)
                elem.SetAttribute("filter", ropFilter);
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// Draws a line from the current position up to, but not including, the specified point.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        public void LineTo(int ex, int ey)
        {
            System.Xml.XmlElement elem = _doc.CreateElement("line", "http://www.w3.org/2000/svg");
            if (_dc.Pen != null)
                elem.SetAttribute("class", GetClassString(_dc.Pen));

            elem.SetAttribute("fill", "none");

            elem.SetAttribute("x1", "" + (int)_dc.ToAbsoluteX(_dc.CurrentX));
            elem.SetAttribute("y1", "" + (int)_dc.ToAbsoluteY(_dc.CurrentY));
            elem.SetAttribute("x2", "" + (int)_dc.ToAbsoluteX(ex));
            elem.SetAttribute("y2", "" + (int)_dc.ToAbsoluteY(ey));
            _parentNode.AppendChild(elem);

            _dc.MoveToEx(ex, ey, null);
        }

        /// <summary>
        /// Updates the current position to the specified point and optionally returns the previous position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        public void MoveToEx(int x, int y, WMFConverter.Gdi.Point old)
        {
            _dc.MoveToEx(x, y, old);
        }

        /// <summary>
        /// Moves the clipping region of a device context by the specified offsets.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void OffsetClipRgn(int x, int y)
        {
            _dc.OffSetClipRgn(x, y);
            System.Xml.XmlElement mask = _dc.Mask;
            if (mask != null)
            {
                mask = (System.Xml.XmlElement)mask.CloneNode(true);
                string name = "mask" + (_maskNo++);
                mask.SetAttribute("id", name);
                if (_dc.OffsetClipX != 0 || _dc.OffsetClipY != 0)
                    mask.SetAttribute("transform", "translate(" + _dc.OffsetClipX + "," + _dc.OffsetClipY + ")");
                _defsNode.AppendChild(mask);

                if (!_parentNode.HasChildNodes)
                    _doc.DocumentElement.RemoveChild(_parentNode);
                _parentNode = _doc.CreateElement("g", "http://www.w3.org/2000/svg");
                _parentNode.SetAttribute("mask", name);
                _doc.DocumentElement.AppendChild(_parentNode);

                _dc.Mask = mask;
            }
        }

        /// <summary>
        /// Modifies the viewport origin for a device context using the specified horizontal and vertical offsets.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="point"></param>
        public void OffsetViewportOrgEx(int x, int y, WMFConverter.Gdi.Point point)
        {
            _dc.OffSetViewportOrgEx(x, y, point);
        }

        /// <summary>
        /// Modifies the window origin for a device context using the specified horizontal and vertical offsets.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="point"></param>
        public void OffsetWindowOrgEx(int x, int y, WMFConverter.Gdi.Point point)
        {
            _dc.OffSetWindowOrgEx(x, y, point);
        }

        /// <summary>
        /// Paints the specified region by using the brush currently selected into the device context.
        /// </summary>
        /// <param name="rgn"></param>
        public void PaintRgn(Gdi.IGdiRegion rgn)
        {
            FillRgn(rgn, _dc.Brush);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rop"></param>
        public void PatBlt(int x, int y, int width, int height, long rop)
        {
            // TODO
            Console.Write("not implemented: patBlt");
        }

        /// <summary>
        /// Draws a pie-shaped wedge bounded by the intersection of an ellipse and two radials. 
        /// The pie is outlined by using the current pen and filled by using the current brush.
        /// </summary>
        /// <param name="sxr"></param>
        /// <param name="syr"></param>
        /// <param name="exr"></param>
        /// <param name="eyr"></param>
        /// <param name="sxa"></param>
        /// <param name="sya"></param>
        /// <param name="exa"></param>
        /// <param name="eya"></param>
        public void Pie(int sxr, int syr, int exr, int eyr, int sxa, int sya,
                int exa, int eya)
        {
            double rx = Math.Abs(exr - sxr) / 2.0;
            double ry = Math.Abs(eyr - syr) / 2.0;
            if (rx <= 0 || ry <= 0) return;

            double cx = Math.Min(sxr, exr) + rx;
            double cy = Math.Min(syr, eyr) + ry;

            System.Xml.XmlElement elem = null;
            if (sxa == exa && sya == eya)
            {
                if (rx == ry)
                {
                    elem = _doc.CreateElement("circle", "http://www.w3.org/2000/svg");
                    elem.SetAttribute("cx", "" + _dc.ToAbsoluteX(cx));
                    elem.SetAttribute("cy", "" + _dc.ToAbsoluteY(cy));
                    elem.SetAttribute("r", "" + _dc.ToRelativeX(rx));
                }
                else
                {
                    elem = _doc.CreateElement("ellipse", "http://www.w3.org/2000/svg");
                    elem.SetAttribute("cx", "" + _dc.ToAbsoluteX(cx));
                    elem.SetAttribute("cy", "" + _dc.ToAbsoluteY(cy));
                    elem.SetAttribute("rx", "" + _dc.ToRelativeX(rx));
                    elem.SetAttribute("ry", "" + _dc.ToRelativeY(ry));
                }
            }
            else
            {
                double sa = Math.Atan2((sya - cy) * rx, (sxa - cx) * ry);
                Console.Write(sa + " " + Math.Cos(sa));
                double sx = rx * Math.Cos(sa);
                double sy = ry * Math.Sin(sa);

                double ea = Math.Atan2((eya - cy) * rx, (exa - cx) * ry);
                double ex = rx * Math.Cos(ea);
                double ey = ry * Math.Sin(ea);

                double a = Math.Atan2((ex - sx) * (-sy) - (ey - sy) * (-sx), (ex - sx) * (-sx) + (ey - sy) * (-sy));

                elem = _doc.CreateElement("path", "http://www.w3.org/2000/svg");
                elem.SetAttribute("d", "M " + _dc.ToAbsoluteX(cx) + "," + _dc.ToAbsoluteY(cy)
                        + " L " + _dc.ToAbsoluteX(sx + cx) + "," + _dc.ToAbsoluteY(sy + cy)
                        + " A " + _dc.ToRelativeX(rx) + "," + _dc.ToRelativeY(ry)
                        + " 0 " + (a > 0 ? "1" : "0") + " 0"
                        + " " + _dc.ToAbsoluteX(ex + cx) + "," + _dc.ToAbsoluteY(ey + cy) + " Z");
            }

            if (_dc.Pen != null || _dc.Brush != null)
            {
                elem.SetAttribute("class", getClassString(_dc.Pen, _dc.Brush));
                if (_dc.Brush != null && _dc.Brush.Style == (int)Gdi.BrushBSEnum.BS_HATCHED)
                {
                    string id = "pattern" + (_patternNo++);
                    elem.SetAttribute("fill", "url(#" + id + ")");
                    _defsNode.AppendChild(_dc.Brush.CreateFillPattern(id));
                }
            }
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// Draws a polygon consisting of two or more vertices connected by straight lines. 
        /// The polygon is outlined by using the current pen and filled by using the current brush and polygon fill mode.
        /// </summary>
        /// <param name="points"></param>
        public void Polygon(WMFConverter.Gdi.Point[] points)
        {
            System.Xml.XmlElement elem = _doc.CreateElement("polygon", "http://www.w3.org/2000/svg");

            if (_dc.Pen != null || _dc.Brush != null)
            {
                elem.SetAttribute("class", getClassString(_dc.Pen, _dc.Brush));
                if (_dc.Brush != null && _dc.Brush.Style == (int)Gdi.BrushBSEnum.BS_HATCHED)
                {
                    string id = "pattern" + (_patternNo++);
                    elem.SetAttribute("fill", "url(#" + id + ")");
                    _defsNode.AppendChild(_dc.Brush.CreateFillPattern(id));
                }
                if (_dc.PolyFillMode == (int)Gdi.GdiEnum.WINDING)
                    elem.SetAttribute("fill-rule", "nonzero");
            }

            _buffer.Length = 0;
            for (int i = 0; i < points.Length; i++)
            {
                if (i != 0)
                    _buffer.Append(" ");
                _buffer.Append((int)_dc.ToAbsoluteX(points[i].X)).Append(",");
                _buffer.Append((int)_dc.ToAbsoluteY(points[i].Y));
            }
            elem.SetAttribute("points", _buffer.ToString());
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// Element is an SVG basic shape that creates straight lines connecting several points. 
        /// Typically a polyline is used to create open shapes as the last point doesn't have to be connected to the first point. 
        /// </summary>
        /// <param name="points"></param>
        public void Polyline(WMFConverter.Gdi.Point[] points)
        {
            System.Xml.XmlElement elem = _doc.CreateElement("polyline", "http://www.w3.org/2000/svg");
            if (_dc.Pen != null)
                elem.SetAttribute("class", GetClassString(_dc.Pen));
            elem.SetAttribute("fill", "none");

            _buffer.Length = 0;
            for (int i = 0; i < points.Length; i++)
            {
                if (i != 0)
                    _buffer.Append(" ");
                _buffer.Append((int)_dc.ToAbsoluteX(points[i].X)).Append(",");
                _buffer.Append((int)_dc.ToAbsoluteY(points[i].Y));
            }
            elem.SetAttribute("points", _buffer.ToString());
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        ///  Draws a series of closed polygons. 
        ///  Each polygon is outlined by using the current pen and filled by using the current brush and polygon fill mode. 
        ///  The polygons drawn by this function can overlap.
        /// </summary>
        /// <param name="points"></param>
        public void PolyPolygon(WMFConverter.Gdi.Point[][] points)
        {
            System.Xml.XmlElement elem = _doc.CreateElement("path", "http://www.w3.org/2000/svg");

            if (_dc.Pen != null || _dc.Brush != null)
            {
                elem.SetAttribute("class", getClassString(_dc.Pen, _dc.Brush));
                if (_dc.Brush != null && _dc.Brush.Style == (int)Gdi.BrushBSEnum.BS_HATCHED)
                {
                    string id = "pattern" + (_patternNo++);
                    elem.SetAttribute("fill", "url(#" + id + ")");
                    _defsNode.AppendChild(_dc.Brush.CreateFillPattern(id));
                }
                if (_dc.PolyFillMode == (int)Gdi.GdiEnum.WINDING)
                    elem.SetAttribute("fill-rule", "nonzero");
            }

            _buffer.Length = 0;
            for (int i = 0; i < points.Length; i++)
            {
                if (i != 0)
                    _buffer.Append(" ");
                for (int j = 0; j < points[i].Length; j++)
                {
                    if (j == 0)
                        _buffer.Append("M ");
                    else if (j == 1)
                        _buffer.Append(" L ");
                    _buffer.Append((int)_dc.ToAbsoluteX(points[i][j].X)).Append(",");
                    _buffer.Append((int)_dc.ToAbsoluteY(points[i][j].Y)).Append(" ");
                    if (j == points[i].Length - 1)
                        _buffer.Append("z");
                }
            }
            elem.SetAttribute("d", _buffer.ToString());
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void RealizePalette()
        {
            // TODO
            Console.Write("not implemented: realizePalette");
        }

        /// <summary>
        /// Restores a device context (DC) to the specified state. 
        /// The DC is restored by popping state information off a stack created by earlier calls to the SaveDC function.
        /// </summary>
        /// <param name="savedDC"></param>
        public void RestoreDC(int savedDC)
        {
            int limit = (savedDC < 0) ? -savedDC : _saveDC.Count() - savedDC;
            for (int i = 0; i < limit; i++)
            {
                _dc = (SvgDc)_saveDC.Last();
                _saveDC.RemoveLast();
            }

            if (!_parentNode.HasChildNodes)
                _doc.DocumentElement.RemoveChild(_parentNode);
            _parentNode = _doc.CreateElement("g", "http://www.w3.org/2000/svg");
            System.Xml.XmlElement mask = _dc.Mask;
            if (mask != null)
                _parentNode.SetAttribute("mask", "url(#" + mask.GetAttribute("id") + ")");
            _doc.DocumentElement.AppendChild(_parentNode);
        }

        /// <summary>
        /// Draws a rectangle. 
        /// The rectangle is outlined by using the current pen and filled by using the current brush.
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        public void Rectangle(int sx, int sy, int ex, int ey)
        {
            System.Xml.XmlElement elem = _doc.CreateElement("rect", "http://www.w3.org/2000/svg");

            if (_dc.Pen != null || _dc.Brush != null)
            {
                elem.SetAttribute("class", getClassString(_dc.Pen, _dc.Brush));
                if (_dc.Brush != null && _dc.Brush.Style == (int)Gdi.BrushBSEnum.BS_HATCHED)
                {
                    string id = "pattern" + (_patternNo++);
                    elem.SetAttribute("fill", "url(#" + id + ")");
                    _defsNode.AppendChild(_dc.Brush.CreateFillPattern(id));
                }
            }

            elem.SetAttribute("x", "" + (int)_dc.ToAbsoluteX(sx));
            elem.SetAttribute("y", "" + (int)_dc.ToAbsoluteY(sy));
            elem.SetAttribute("width", "" + (int)_dc.ToRelativeX(ex - sx));
            elem.SetAttribute("height", "" + (int)_dc.ToRelativeY(ey - sy));
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="palette"></param>
        public void ResizePalette(Gdi.IGdiPalette palette)
        {
            // TODO
            Console.Write("not implemented: ResizePalette");
        }

        /// <summary>
        /// Draws a rectangle with rounded corners. 
        /// The rectangle is outlined by using the current pen and filled by using the current brush.
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        /// <param name="rw"></param>
        /// <param name="rh"></param>
        public void RoundRect(int sx, int sy, int ex, int ey, int rw, int rh)
        {
            System.Xml.XmlElement elem = _doc.CreateElement("rect", "http://www.w3.org/2000/svg");

            if (_dc.Pen != null || _dc.Brush != null)
            {
                elem.SetAttribute("class", getClassString(_dc.Pen, _dc.Brush));
                if (_dc.Brush != null && _dc.Brush.Style == (int)Gdi.BrushBSEnum.BS_HATCHED)
                {
                    string id = "pattern" + (_patternNo++);
                    elem.SetAttribute("fill", "url(#" + id + ")");
                    _defsNode.AppendChild(_dc.Brush.CreateFillPattern(id));
                }
            }

            elem.SetAttribute("x", "" + (int)_dc.ToAbsoluteX(sx));
            elem.SetAttribute("y", "" + (int)_dc.ToAbsoluteY(sy));
            elem.SetAttribute("width", "" + (int)_dc.ToRelativeX(ex - sx));
            elem.SetAttribute("height", "" + (int)_dc.ToRelativeY(ey - sy));
            elem.SetAttribute("rx", "" + (int)_dc.ToRelativeX(rw));
            elem.SetAttribute("ry", "" + (int)_dc.ToRelativeY(rh));
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// Save device context (DC).
        /// </summary>
        public void SeveDC()
        {
            _saveDC.AddLast((SvgDc)_dc.Clone());
        }

        /// <summary>
        /// Modifies the viewport for a device context using the ratios formed by the specified multiplicands and divisors.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="xd"></param>
        /// <param name="y"></param>
        /// <param name="yd"></param>
        /// <param name="old"></param>
        public void ScaleViewportExtEx(int x, int xd, int y, int yd, WMFConverter.Gdi.Size old)
        {
            _dc.ScaleViewportExtEx(x, xd, y, yd, old);
        }

        /// <summary>
        /// Modifies the window for a device context using the ratios formed by the specified multiplicands and divisors.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="xd"></param>
        /// <param name="y"></param>
        /// <param name="yd"></param>
        /// <param name="old"></param>
        public void ScaleWindowExtEx(int x, int xd, int y, int yd, WMFConverter.Gdi.Size old)
        {
            _dc.ScaleWindowExtEx(x, xd, y, yd, old);
        }

        /// <summary>
        /// Selects a region as the current clipping region for the specified device context.
        /// </summary>
        /// <param name="rgn"></param>
        public void SelectClipRgn(Gdi.IGdiRegion rgn)
        {
            if (!_parentNode.HasChildNodes)
                _doc.DocumentElement.RemoveChild(_parentNode);
            _parentNode = _doc.CreateElement("g", "http://www.w3.org/2000/svg");

            if (rgn != null)
            {
                System.Xml.XmlElement mask = _doc.CreateElement("mask", "http://www.w3.org/2000/svg");
                mask.SetAttribute("id", "mask" + (_maskNo++));
                //mask.setIdAttribute("id", true);

                if (_dc.OffsetClipX != 0 || _dc.OffsetClipY != 0)
                    mask.SetAttribute("transform", "translate(" + _dc.OffsetClipX + "," + _dc.OffsetClipY + ")");
                _defsNode.AppendChild(mask);

                System.Xml.XmlElement clip = _doc.CreateElement("use", "http://www.w3.org/2000/svg");
                var output = "";
                _nameMap.TryGetValue(rgn, out output);
                clip.SetAttribute("xlink:href", "url(#" + output + ")");
                clip.SetAttribute("fill", "white");

                mask.AppendChild(clip);

                _parentNode.SetAttribute("mask", "url(#" + mask.GetAttribute("id") + ")");
            }

            _doc.DocumentElement.AppendChild(_parentNode);
        }

        /// <summary>
        /// Selects an object into the specified device context (DC). The new object replaces the previous object of the same type.
        /// </summary>
        /// <param name="obj"></param>
        public void SelectObject(WMFConverter.Gdi.IGdiObject obj)
        {
            if (obj.GetType() == typeof(SvgBrush))
                _dc.Brush = (SvgBrush)obj;
            else if (obj.GetType() == typeof(SvgFont))
                _dc.Font = (SvgFont)obj;
            else if (obj.GetType() == typeof(SvgPen))
                _dc.Pen = (SvgPen)obj;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="palette"></param>
        /// <param name="mode"></param>
        public void SelectPalette(Gdi.IGdiPalette palette, bool mode)
        {
            // TODO
            Console.Write("not implemented: selectPalette");
        }

        /// <summary>
        /// Sets the current background color to the specified color value, or to the nearest physical color if the device cannot represent the specified color value.
        /// </summary>
        /// <param name="color"></param>
        public void SetBkColor(int color)
        {
            _dc.BkColor = color;
        }

        /// <summary>
        /// Sets the background mix mode of the specified device context. 
        /// The background mix mode is used with text, hatched brushes, and pen styles that are not solid lines.
        /// </summary>
        /// <param name="mode"></param>
        public void SetBkMode(int mode)
        {
            _dc.BkMode = mode;
        }

        /// <summary>
        /// Sets the pixels in the specified rectangle on the device that is associated with the destination device context using color data from a DIB, JPEG, or PNG image.
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dw"></param>
        /// <param name="dh"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="startscan"></param>
        /// <param name="scanlines"></param>
        /// <param name="image"></param>
        /// <param name="colorUse"></param>
        public void SetDIBitsToDevice(int dx, int dy, int dw, int dh, int sx,
                int sy, int startscan, int scanlines, byte[] image, int colorUse)
        {
            StretchDIBits(dx, dy, dw, dh, sx, sy, dw, dh, image, colorUse, (int)Gdi.GdiEnum.SRCCOPY);
        }

        /// <summary>
        /// Changes the layout of a device context (DC).
        /// </summary>
        /// <param name="layout"></param>
        public void SetLayout(long layout)
        {
            _dc.Layout = layout;
        }

        /// <summary>
        /// Sets the mapping mode of the specified device context. 
        /// The mapping mode defines the unit of measure used to transform page-space units into device-space units, and also defines the orientation of the device's x and y axes.
        /// </summary>
        /// <param name="mode"></param>
        public void SetMapMode(int mode)
        {
            _dc.SetMapMode(mode);
        }

        /// <summary>
        /// Alters the algorithm the font mapper uses when it maps logical fonts to physical fonts.
        /// </summary>
        /// <param name="flags"></param>
        public void SetMapperFlags(long flags)
        {
            _dc.MapperFlags = flags;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="palette"></param>
        /// <param name="startIndex"></param>
        /// <param name="entries"></param>
        public void SetPaletteEntries(Gdi.IGdiPalette palette, int startIndex, int[] entries)
        {
            // TODO
            Console.Write("not implemented: setPaletteEntries");
        }

        /// <summary>
        /// Sets the pixel at the specified coordinates to the specified color.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, int color)
        {
            System.Xml.XmlElement elem = _doc.CreateElement("rect", "http://www.w3.org/2000/svg");
            elem.SetAttribute("stroke", "none");
            elem.SetAttribute("fill", SvgPen.ToColor(color));
            elem.SetAttribute("x", "" + (int)_dc.ToAbsoluteX(x));
            elem.SetAttribute("y", "" + (int)_dc.ToAbsoluteY(y));
            elem.SetAttribute("width", "" + (int)_dc.ToRelativeX(1));
            elem.SetAttribute("height", "" + (int)_dc.ToRelativeY(1));
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// Sets the polygon fill mode for functions that fill polygons.
        /// </summary>
        /// <param name="mode"></param>
        public void SetPolyFillMode(int mode)
        {
            _dc.PolyFillMode = mode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        public void SetRelAbs(int mode)
        {
            _dc.RelAbs = mode;
        }

        /// <summary>
        /// Sets the current foreground mix mode. 
        /// GDI uses the foreground mix mode to combine pens and interiors of filled objects with the colors already on the screen. 
        /// The foreground mix mode defines how colors from the brush or pen and the colors in the existing image are to be combined.
        /// </summary>
        /// <param name="mode"></param>
        public void SetROP2(int mode)
        {
            _dc.ROP2 = mode;
        }

        /// <summary>
        /// Sets the bitmap stretching mode in the specified device context.
        /// </summary>
        /// <param name="mode"></param>
        public void SetStretchBltMode(int mode)
        {
            _dc.StretchBltMode = mode;
        }

        /// <summary>
        /// Sets the text-alignment flags for the specified device context.
        /// </summary>
        /// <param name="align"></param>
        public void SetTextAlign(int align)
        {
            _dc.TextAlign = align;
        }

        /// <summary>
        /// Sets the intercharacter spacing. 
        /// Intercharacter spacing is added to each character, including break characters, when the system writes a line of text.
        /// </summary>
        /// <param name="extra"></param>
        public void SetTextCharacterExtra(int extra)
        {
            _dc.TextCharacterExtra = extra;
        }

        /// <summary>
        /// Sets the text color for the specified device context to the specified color.
        /// </summary>
        /// <param name="color"></param>
        public void SetTextColor(int color)
        {
            _dc.TextColor = color;
        }

        /// <summary>
        /// Specifies the amount of space the system should add to the break characters in a string of text. 
        /// The space is added when an application calls the TextOut or ExtTextOut functions.
        /// </summary>
        /// <param name="breakExtra"></param>
        /// <param name="breakCount"></param>
        public void SetTextJustification(int breakExtra, int breakCount)
        {
            if (breakCount > 0)
                _dc.TextSpace = Math.Abs((int)_dc.ToRelativeX(breakExtra)) / breakCount;
        }

        /// <summary>
        /// Sets the horizontal and vertical extents of the viewport for a device context by using the specified values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        public void SetViewportExtEx(int x, int y, WMFConverter.Gdi.Size old)
        {
            _dc.SetViewportExtEx(x, y, old);
        }

        /// <summary>
        /// Specifies which device point maps to the window origin (0,0).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        public void SetViewportOrgEx(int x, int y, WMFConverter.Gdi.Point old)
        {
            _dc.SetViewportOrgEx(x, y, old);
        }

        /// <summary>
        /// Sets the horizontal and vertical extents of the window for a device context by using the specified values.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="old"></param>
        public void SetWindowExtEx(int width, int height, WMFConverter.Gdi.Size old)
        {
            _dc.SetWindowExtEx(width, height, old);
        }

        /// <summary>
        /// Specifies which window point maps to the viewport origin (0,0).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        public void SetWindowOrgEx(int x, int y, WMFConverter.Gdi.Point old)
        {
            _dc.SetWindowOrgEx(x, y, old);
        }

        /// <summary>
        /// Copies a bitmap from a source rectangle into a destination rectangle, stretching or compressing the bitmap to fit the dimensions of the destination rectangle, if necessary. 
        /// The system stretches or compresses the bitmap according to the stretching mode currently set in the destination device context.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dw"></param>
        /// <param name="dh"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="sw"></param>
        /// <param name="sh"></param>
        /// <param name="rop"></param>
        public void StretchBlt(byte[] image, int dx, int dy, int dw, int dh, int sx, int sy,
                int sw, int sh, long rop)
        {
            DibStretchBlt(image, dx, dy, dw, dh, sx, sy, sw, sh, rop);
        }

        /// <summary>
        /// Copies the color data for a rectangle of pixels in a DIB, JPEG, or PNG image to the specified destination rectangle. 
        /// If the destination rectangle is larger than the source rectangle, this function stretches the rows and columns of color data to fit the destination rectangle. If the destination rectangle is smaller than the source rectangle, this function compresses the rows and columns by using the specified raster operation.
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dw"></param>
        /// <param name="dh"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="sw"></param>
        /// <param name="sh"></param>
        /// <param name="image"></param>
        /// <param name="usage"></param>
        /// <param name="rop"></param>
        public void StretchDIBits(int dx, int dy, int dw, int dh, int sx, int sy,
                int sw, int sh, byte[] image, int usage, long rop)
        {
            bmpToSvg(image, dx, dy, dw, dh, sx, sy, sw, sh, usage, rop);
        }

        /// <summary>
        /// Writes a character string at the specified location, using the currently selected font, background color, and text color.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
        public void TextOut(int x, int y, byte[] text)
        {
            System.Xml.XmlElement elem = _doc.CreateElement("text", "http://www.w3.org/2000/svg");

            int escapement = 0;
            bool vertical = false;
            if (_dc.Font != null)
            {
                elem.SetAttribute("class", GetClassString(_dc.Font));
                if (_dc.Font.FaceName.StartsWith("@"))
                {
                    vertical = true;
                    escapement = _dc.Font.Escapement - 2700;
                }
                else
                    escapement = _dc.Font.Escapement;
            }
            elem.SetAttribute("fill", SvgObject.ToColor(_dc.TextColor));

            // style
            _buffer.Length = 0;
            int align = _dc.TextAlign;

            if ((align & ((int)Gdi.GdiEnum.TA_LEFT | (int)Gdi.GdiEnum.TA_RIGHT | (int)Gdi.GdiEnum.TA_CENTER)) == (int)Gdi.GdiEnum.TA_RIGHT)
                _buffer.Append("text-anchor: end; ");
            else if ((align & ((int)Gdi.GdiEnum.TA_LEFT | (int)Gdi.GdiEnum.TA_RIGHT | (int)Gdi.GdiEnum.TA_CENTER)) == (int)Gdi.GdiEnum.TA_CENTER)
                _buffer.Append("text-anchor: middle; ");

            if (vertical)
            {
                elem.SetAttribute("writing-mode", "tb");
                _buffer.Append("dominant-baseline: ideographic; ");
            }
            else
            {
                if ((align & ((int)Gdi.GdiEnum.TA_BOTTOM | (int)Gdi.GdiEnum.TA_TOP | (int)Gdi.GdiEnum.TA_BASELINE)) == (int)Gdi.GdiEnum.TA_BASELINE)
                    _buffer.Append("dominant-baseline: alphabetic; ");
                else
                    _buffer.Append("dominant-baseline: text-before-edge; ");
            }

            if ((align & (int)Gdi.GdiEnum.TA_RTLREADING) == (int)Gdi.GdiEnum.TA_RTLREADING)
                _buffer.Append("unicode-bidi: bidi-override; direction: rtl; ");

            if (_dc.TextSpace > 0)
                _buffer.Append("word-spacing: " + _dc.TextSpace + "; ");

            if (_buffer.Length > 0)
            {
                _buffer.Length = _buffer.Length - 1;
                elem.SetAttribute("style", _buffer.ToString());
            }

            elem.SetAttribute("stroke", "none");

            int ax = (int)_dc.ToAbsoluteX(x);
            int ay = (int)_dc.ToAbsoluteY(y);
            elem.SetAttribute("x", ax.ToString());
            elem.SetAttribute("y", ay.ToString());

            if (escapement != 0)
                elem.SetAttribute("transform", "rotate(" + (-escapement / 10.0) + ", " + ax + ", " + ay + ")");

            string str = null;
            if (_dc.Font != null)
                str = Gdi.GdiUtils.ConvertString(text, _dc.Font.Charset);
            else
                str = Gdi.GdiUtils.ConvertString(text, (int)Gdi.FontCharsetEnum.DEFAULT_CHARSET);


            if (_dc.TextCharacterExtra != 0)
            {
                _buffer.Length = 0;
                for (int i = 0; i < str.Length - 1; i++)
                {
                    if (i != 0)
                        _buffer.Append(" ");
                    _buffer.Append((int)_dc.ToRelativeX(_dc.TextCharacterExtra));
                }
                elem.SetAttribute("dx", _buffer.ToString());
            }

            if (_dc.Font != null && _dc.Font.Lang != null)
                elem.SetAttribute("xml:lang", _dc.Font.Lang);

            elem.SetAttribute("xml:space", "preserve");
            AppendText(elem, str);
            _parentNode.AppendChild(elem);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Footer()
        {
            System.Xml.XmlElement root = _doc.DocumentElement;
            if (!root.HasAttribute("width") && _dc.WindowWidth != 0)
                root.SetAttribute("width", "" + Math.Abs(_dc.WindowWidth));

            if (!root.HasAttribute("height") && _dc.WindowHeight != 0)
                root.SetAttribute("height", "" + Math.Abs(_dc.WindowHeight));

            if (_dc.WindowWidth != 0 && _dc.WindowHeight != 0)
            {
                root.SetAttribute("viewBox", "0 0 " + Math.Abs(_dc.WindowWidth) + " " + Math.Abs(_dc.WindowHeight));
                root.SetAttribute("preserveAspectRatio", "none");
            }
            root.SetAttribute("stroke-linecap", "round");
            root.SetAttribute("fill-rule", "evenodd");

            if (!_styleNode.HasChildNodes)
                root.RemoveChild(_styleNode);
            else
                _styleNode.InsertBefore(_doc.CreateTextNode("\r\n"), _styleNode.FirstChild);

            if (!_defsNode.HasChildNodes)
                root.RemoveChild(_defsNode);
        }

        /// <summary>
        /// Get class name.
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        private string getClassString(WMFConverter.Gdi.IGdiObject obj1, WMFConverter.Gdi.IGdiObject obj2)
        {
            string name1 = GetClassString(obj1);
            string name2 = GetClassString(obj2);
            if (name1 != null && name2 != null)
                return name1 + " " + name2;

            if (name1 != null)
                return name1;

            if (name2 != null)
                return name2;

            return "";
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get class name.
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private string GetClassString(WMFConverter.Gdi.IGdiObject style)
        {
            if (style == null)
                return "";

            var output = "";
            _nameMap.TryGetValue(style, out output);
            return output;
        }

        /// <summary>
        /// Append specified string to specified element.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="str"></param>
        private void AppendText(System.Xml.XmlElement elem, string str)
        {
            if (_compatible)
            {
                str = System.Text.RegularExpressions.Regex.Replace(str, "\\r\\n", "\u00A0");
                str = System.Text.RegularExpressions.Regex.Replace(str, "\\t\\r\\n", "\u00A0");
            }

            SvgFont font = _dc.Font;
            if (_replaceSymbolFont && font != null)
            {
                if ("Symbol".Equals(font.FaceName))
                {
                    int state = 0; // 0: default, 1: serif, 2: sans-serif
                    int start = 0;
                    char[] ca = str.ToCharArray();
                    for (int i = 0; i < ca.Length; i++)
                    {
                        int nstate = state;
                        switch (ca[i])
                        {
                            case '"': ca[i] = '\u2200'; nstate = 1; break;
                            case '$': ca[i] = '\u2203'; nstate = 1; break;
                            case '\'': ca[i] = '\u220D'; nstate = 1; break;
                            case '*': ca[i] = '\u2217'; nstate = 1; break;
                            case '-': ca[i] = '\u2212'; nstate = 1; break;
                            case '@': ca[i] = '\u2245'; nstate = 1; break;
                            case 'A': ca[i] = '\u0391'; nstate = 1; break;
                            case 'B': ca[i] = '\u0392'; nstate = 1; break;
                            case 'C': ca[i] = '\u03A7'; nstate = 1; break;
                            case 'D': ca[i] = '\u0394'; nstate = 1; break;
                            case 'E': ca[i] = '\u0395'; nstate = 1; break;
                            case 'F': ca[i] = '\u03A6'; nstate = 1; break;
                            case 'G': ca[i] = '\u0393'; nstate = 1; break;
                            case 'H': ca[i] = '\u0397'; nstate = 1; break;
                            case 'I': ca[i] = '\u0399'; nstate = 1; break;
                            case 'J': ca[i] = '\u03D1'; nstate = 1; break;
                            case 'K': ca[i] = '\u039A'; nstate = 1; break;
                            case 'L': ca[i] = '\u039B'; nstate = 1; break;
                            case 'M': ca[i] = '\u039C'; nstate = 1; break;
                            case 'N': ca[i] = '\u039D'; nstate = 1; break;
                            case 'O': ca[i] = '\u039F'; nstate = 1; break;
                            case 'P': ca[i] = '\u03A0'; nstate = 1; break;
                            case 'Q': ca[i] = '\u0398'; nstate = 1; break;
                            case 'R': ca[i] = '\u03A1'; nstate = 1; break;
                            case 'S': ca[i] = '\u03A3'; nstate = 1; break;
                            case 'T': ca[i] = '\u03A4'; nstate = 1; break;
                            case 'U': ca[i] = '\u03A5'; nstate = 1; break;
                            case 'V': ca[i] = '\u03C3'; nstate = 1; break;
                            case 'W': ca[i] = '\u03A9'; nstate = 1; break;
                            case 'X': ca[i] = '\u039E'; nstate = 1; break;
                            case 'Y': ca[i] = '\u03A8'; nstate = 1; break;
                            case 'Z': ca[i] = '\u0396'; nstate = 1; break;
                            case '\\': ca[i] = '\u2234'; nstate = 1; break;
                            case '^': ca[i] = '\u22A5'; nstate = 1; break;
                            case '`': ca[i] = '\uF8E5'; nstate = 1; break;
                            case 'a': ca[i] = '\u03B1'; nstate = 1; break;
                            case 'b': ca[i] = '\u03B2'; nstate = 1; break;
                            case 'c': ca[i] = '\u03C7'; nstate = 1; break;
                            case 'd': ca[i] = '\u03B4'; nstate = 1; break;
                            case 'e': ca[i] = '\u03B5'; nstate = 1; break;
                            case 'f': ca[i] = '\u03C6'; nstate = 1; break;
                            case 'g': ca[i] = '\u03B3'; nstate = 1; break;
                            case 'h': ca[i] = '\u03B7'; nstate = 1; break;
                            case 'i': ca[i] = '\u03B9'; nstate = 1; break;
                            case 'j': ca[i] = '\u03D5'; nstate = 1; break;
                            case 'k': ca[i] = '\u03BA'; nstate = 1; break;
                            case 'l': ca[i] = '\u03BB'; nstate = 1; break;
                            case 'm': ca[i] = '\u03BC'; nstate = 1; break;
                            case 'n': ca[i] = '\u03BD'; nstate = 1; break;
                            case 'o': ca[i] = '\u03BF'; nstate = 1; break;
                            case 'p': ca[i] = '\u03C0'; nstate = 1; break;
                            case 'q': ca[i] = '\u03B8'; nstate = 1; break;
                            case 'r': ca[i] = '\u03C1'; nstate = 1; break;
                            case 's': ca[i] = '\u03C3'; nstate = 1; break;
                            case 't': ca[i] = '\u03C4'; nstate = 1; break;
                            case 'u': ca[i] = '\u03C5'; nstate = 1; break;
                            case 'v': ca[i] = '\u03D6'; nstate = 1; break;
                            case 'w': ca[i] = '\u03C9'; nstate = 1; break;
                            case 'x': ca[i] = '\u03BE'; nstate = 1; break;
                            case 'y': ca[i] = '\u03C8'; nstate = 1; break;
                            case 'z': ca[i] = '\u03B6'; nstate = 1; break;
                            case '~': ca[i] = '\u223C'; nstate = 1; break;
                            case '\u00A0': ca[i] = '\u20AC'; nstate = 1; break;
                            case '\u00A1': ca[i] = '\u03D2'; nstate = 1; break;
                            case '\u00A2': ca[i] = '\u2032'; nstate = 1; break;
                            case '\u00A3': ca[i] = '\u2264'; nstate = 1; break;
                            case '\u00A4': ca[i] = '\u2044'; nstate = 1; break;
                            case '\u00A5': ca[i] = '\u221E'; nstate = 1; break;
                            case '\u00A6': ca[i] = '\u0192'; nstate = 1; break;
                            case '\u00A7': ca[i] = '\u2663'; nstate = 1; break;
                            case '\u00A8': ca[i] = '\u2666'; nstate = 1; break;
                            case '\u00A9': ca[i] = '\u2665'; nstate = 1; break;
                            case '\u00AA': ca[i] = '\u2660'; nstate = 1; break;
                            case '\u00AB': ca[i] = '\u2194'; nstate = 1; break;
                            case '\u00AC': ca[i] = '\u2190'; nstate = 1; break;
                            case '\u00AD': ca[i] = '\u2191'; nstate = 1; break;
                            case '\u00AE': ca[i] = '\u2192'; nstate = 1; break;
                            case '\u00AF': ca[i] = '\u2193'; nstate = 1; break;
                            case '\u00B2': ca[i] = '\u2033'; nstate = 1; break;
                            case '\u00B3': ca[i] = '\u2265'; nstate = 1; break;
                            case '\u00B4': ca[i] = '\u00D7'; nstate = 1; break;
                            case '\u00B5': ca[i] = '\u221D'; nstate = 1; break;
                            case '\u00B6': ca[i] = '\u2202'; nstate = 1; break;
                            case '\u00B7': ca[i] = '\u2022'; nstate = 1; break;
                            case '\u00B8': ca[i] = '\u00F7'; nstate = 1; break;
                            case '\u00B9': ca[i] = '\u2260'; nstate = 1; break;
                            case '\u00BA': ca[i] = '\u2261'; nstate = 1; break;
                            case '\u00BB': ca[i] = '\u2248'; nstate = 1; break;
                            case '\u00BC': ca[i] = '\u2026'; nstate = 1; break;
                            case '\u00BD': ca[i] = '\u23D0'; nstate = 1; break;
                            case '\u00BE': ca[i] = '\u23AF'; nstate = 1; break;
                            case '\u00BF': ca[i] = '\u21B5'; nstate = 1; break;
                            case '\u00C0': ca[i] = '\u2135'; nstate = 1; break;
                            case '\u00C1': ca[i] = '\u2111'; nstate = 1; break;
                            case '\u00C2': ca[i] = '\u211C'; nstate = 1; break;
                            case '\u00C3': ca[i] = '\u2118'; nstate = 1; break;
                            case '\u00C4': ca[i] = '\u2297'; nstate = 1; break;
                            case '\u00C5': ca[i] = '\u2295'; nstate = 1; break;
                            case '\u00C6': ca[i] = '\u2205'; nstate = 1; break;
                            case '\u00C7': ca[i] = '\u2229'; nstate = 1; break;
                            case '\u00C8': ca[i] = '\u222A'; nstate = 1; break;
                            case '\u00C9': ca[i] = '\u2283'; nstate = 1; break;
                            case '\u00CA': ca[i] = '\u2287'; nstate = 1; break;
                            case '\u00CB': ca[i] = '\u2284'; nstate = 1; break;
                            case '\u00CC': ca[i] = '\u2282'; nstate = 1; break;
                            case '\u00CD': ca[i] = '\u2286'; nstate = 1; break;
                            case '\u00CE': ca[i] = '\u2208'; nstate = 1; break;
                            case '\u00CF': ca[i] = '\u2209'; nstate = 1; break;
                            case '\u00D0': ca[i] = '\u2220'; nstate = 1; break;
                            case '\u00D1': ca[i] = '\u2207'; nstate = 1; break;
                            case '\u00D2': ca[i] = '\u00AE'; nstate = 1; break;
                            case '\u00D3': ca[i] = '\u00A9'; nstate = 1; break;
                            case '\u00D4': ca[i] = '\u2122'; nstate = 1; break;
                            case '\u00D5': ca[i] = '\u220F'; nstate = 1; break;
                            case '\u00D6': ca[i] = '\u221A'; nstate = 1; break;
                            case '\u00D7': ca[i] = '\u22C5'; nstate = 1; break;
                            case '\u00D8': ca[i] = '\u00AC'; nstate = 1; break;
                            case '\u00D9': ca[i] = '\u2227'; nstate = 1; break;
                            case '\u00DA': ca[i] = '\u2228'; nstate = 1; break;
                            case '\u00DB': ca[i] = '\u21D4'; nstate = 1; break;
                            case '\u00DC': ca[i] = '\u21D0'; nstate = 1; break;
                            case '\u00DD': ca[i] = '\u21D1'; nstate = 1; break;
                            case '\u00DE': ca[i] = '\u21D2'; nstate = 1; break;
                            case '\u00DF': ca[i] = '\u21D3'; nstate = 1; break;
                            case '\u00E0': ca[i] = '\u25CA'; nstate = 1; break;
                            case '\u00E1': ca[i] = '\u3008'; nstate = 1; break;
                            case '\u00E2': ca[i] = '\u00AE'; nstate = 2; break;
                            case '\u00E3': ca[i] = '\u00A9'; nstate = 2; break;
                            case '\u00E4': ca[i] = '\u2122'; nstate = 2; break;
                            case '\u00E5': ca[i] = '\u2211'; nstate = 1; break;
                            case '\u00E6': ca[i] = '\u239B'; nstate = 1; break;
                            case '\u00E7': ca[i] = '\u239C'; nstate = 1; break;
                            case '\u00E8': ca[i] = '\u239D'; nstate = 1; break;
                            case '\u00E9': ca[i] = '\u23A1'; nstate = 1; break;
                            case '\u00EA': ca[i] = '\u23A2'; nstate = 1; break;
                            case '\u00EB': ca[i] = '\u23A3'; nstate = 1; break;
                            case '\u00EC': ca[i] = '\u23A7'; nstate = 1; break;
                            case '\u00ED': ca[i] = '\u23A8'; nstate = 1; break;
                            case '\u00EE': ca[i] = '\u23A9'; nstate = 1; break;
                            case '\u00EF': ca[i] = '\u23AA'; nstate = 1; break;
                            case '\u00F0': ca[i] = '\uF8FF'; nstate = 1; break;
                            case '\u00F1': ca[i] = '\u3009'; nstate = 1; break;
                            case '\u00F2': ca[i] = '\u222B'; nstate = 1; break;
                            case '\u00F3': ca[i] = '\u2320'; nstate = 1; break;
                            case '\u00F4': ca[i] = '\u23AE'; nstate = 1; break;
                            case '\u00F5': ca[i] = '\u2321'; nstate = 1; break;
                            case '\u00F6': ca[i] = '\u239E'; nstate = 1; break;
                            case '\u00F7': ca[i] = '\u239F'; nstate = 1; break;
                            case '\u00F8': ca[i] = '\u23A0'; nstate = 1; break;
                            case '\u00F9': ca[i] = '\u23A4'; nstate = 1; break;
                            case '\u00FA': ca[i] = '\u23A5'; nstate = 1; break;
                            case '\u00FB': ca[i] = '\u23A6'; nstate = 1; break;
                            case '\u00FC': ca[i] = '\u23AB'; nstate = 1; break;
                            case '\u00FD': ca[i] = '\u23AC'; nstate = 1; break;
                            case '\u00FE': ca[i] = '\u23AD'; nstate = 1; break;
                            case '\u00FF': ca[i] = '\u2192'; nstate = 1; break;
                            default: nstate = 0; break;
                        }

                        if (nstate != state)
                        {
                            if (start < i)
                            {
                                System.Xml.XmlNode text = _doc.CreateTextNode(string.Join("", ca).Substring(start, i - start));
                                if (state == 0)
                                    elem.AppendChild(text);
                                else if (state == 1)
                                {
                                    System.Xml.XmlElement span = _doc.CreateElement("tspan", "http://www.w3.org/2000/svg");
                                    span.SetAttribute("font-family", "serif");
                                    span.AppendChild(text);
                                    elem.AppendChild(span);
                                }
                                else if (state == 2)
                                {
                                    System.Xml.XmlElement span = _doc.CreateElement("tspan", "http://www.w3.org/2000/svg");
                                    span.SetAttribute("font-family", "sans-serif");
                                    span.AppendChild(text);
                                    elem.AppendChild(span);
                                }
                                start = i;
                            }
                            state = nstate;
                        }
                    }

                    if (start < ca.Length)
                    {
                        System.Xml.XmlNode text = _doc.CreateTextNode(string.Join("", ca).Substring(start, ca.Length - start));
                        if (state == 0)
                            elem.AppendChild(text);
                        else if (state == 1)
                        {
                            System.Xml.XmlElement span = _doc.CreateElement("tspan", "http://www.w3.org/2000/svg");
                            span.SetAttribute("font-family", "serif");
                            span.AppendChild(text);
                            elem.AppendChild(span);
                        }
                        else if (state == 2)
                        {
                            System.Xml.XmlElement span = _doc.CreateElement("tspan", "http://www.w3.org/2000/svg");
                            span.SetAttribute("font-family", "sans-serif");
                            span.AppendChild(text);
                            elem.AppendChild(span);
                        }
                    }
                    return;
                }
            }

            elem.AppendChild(_doc.CreateTextNode(str));
        }

        /// <summary>
        /// Convert BMP to SVG.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dw"></param>
        /// <param name="dh"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="sw"></param>
        /// <param name="sh"></param>
        /// <param name="usage"></param>
        /// <param name="rop"></param>
        private void bmpToSvg(byte[] image, int dx, int dy, int dw, int dh, int sx, int sy,
               int sw, int sh, int usage, long rop)
        {
            Console.Write("bmpToSvg: not implemented.");
        }

        /// <summary>
        /// Convert Dib to BMP.
        /// </summary>
        /// <param name="dib"></param>
        /// <returns></returns>
        private byte[] dibToBmp(byte[] dib)
        {
            byte[] data = new byte[14 + dib.Length];

            /* BitmapFileHeader */
            data[0] = 0x42; // 'B'
            data[1] = 0x4d; // 'M'

            long bfSize = data.Length;
            data[2] = (byte)(bfSize & 0xff);
            data[3] = (byte)((bfSize >> 8) & 0xff);
            data[4] = (byte)((bfSize >> 16) & 0xff);
            data[5] = (byte)((bfSize >> 24) & 0xff);

            // reserved 1
            data[6] = 0x00;
            data[7] = 0x00;

            // reserved 2
            data[8] = 0x00;
            data[9] = 0x00;

            // offset
            long bfOffBits = 14;

            /* BitmapInfoHeader */
            long biSize = (dib[0] & 0xff) + ((dib[1] & 0xff) << 8)
                    + ((dib[2] & 0xff) << 16) + ((dib[3] & 0xff) << 24);
            bfOffBits += biSize;

            int biBitCount = (dib[14] & 0xff) + ((dib[15] & 0xff) << 8);

            long clrUsed = (dib[32] & 0xff) + ((dib[33] & 0xff) << 8)
                    + ((dib[34] & 0xff) << 16) + ((dib[35] & 0xff) << 24);

            switch (biBitCount)
            {
                case 1:
                    bfOffBits += (0x1L + 1) * 4;
                    break;
                case 4:
                    bfOffBits += (0xFL + 1) * 4;
                    break;
                case 8:
                    bfOffBits += (0xFFL + 1) * 4;
                    break;
                case 16:
                    bfOffBits += (clrUsed == 0L) ? 0 : (0xFFFFL + 1) * 4;
                    break;
                case 24:
                    bfOffBits += (clrUsed == 0L) ? 0 : (0xFFFFFFL + 1) * 4;
                    break;
                case 32:
                    bfOffBits += (clrUsed == 0L) ? 0 : (0xFFFFFFFFL + 1) * 4;
                    break;
            }

            data[10] = (byte)(bfOffBits & 0xff);
            data[11] = (byte)((bfOffBits >> 8) & 0xff);
            data[12] = (byte)((bfOffBits >> 16) & 0xff);
            data[13] = (byte)((bfOffBits >> 24) & 0xff);

            Array.Copy(dib, 0, data, 14, dib.Length);

            return data;
        }

        /// <summary>
        /// Init Document.
        /// </summary>
        private void init()
        {
            _dc = new SvgDc(this);
            System.Xml.XmlElement root = _doc.DocumentElement;
            root.SetAttribute("xmlns", "http://www.w3.org/2000/svg");
            root.SetAttribute("xmlns:xlink", "http://www.w3.org/1999/xlink");

            _defsNode = _doc.CreateElement("defs", "http://www.w3.org/2000/svg");
            root.AppendChild(_defsNode);


            _styleNode = _doc.CreateElement("style", "http://www.w3.org/2000/svg");
            _styleNode.SetAttribute("type", "text/css");
            root.AppendChild(_styleNode);


            _parentNode = _doc.CreateElement("g", "http://www.w3.org/2000/svg");
            _doc.DocumentElement.AppendChild(_parentNode);

            _defaultBrush = (SvgBrush)CreateBrushIndirect((int)Gdi.BrushBSEnum.BS_SOLID,
                    0x00FFFFFF, 0);
            _defaultPen = (SvgPen)CreatePenIndirect((int)Gdi.PenPSEnum.PS_SOLID, 1,
                    0x00000000);
            _defaultFont = null;

            _dc.Brush = _defaultBrush;
            _dc.Pen = _defaultPen;
            _dc.Font = _defaultFont;
        }

        #endregion
    }
}
