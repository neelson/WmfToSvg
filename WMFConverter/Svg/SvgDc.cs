using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Svg
{
    /// <summary>
    /// Scalable Vector Graphics - Represents SVG document.
    /// </summary>
    public class SvgDc : ICloneable
    {

        #region Local Variables

        private SvgGdi _gdi;

        private int _dpi = 1440;

        // window
        private int _wx = 0;
        private int _wy = 0;
        private int _ww = 0;
        private int _wh = 0;

        // window offset
        private int _wox = 0;
        private int _woy = 0;

        // window scale
        private double _wsx = 1.0;
        private double _wsy = 1.0;

        // mapping scale
        private double _mx = 1.0;
        private double _my = 1.0;

        // viewport
        private int _vx = 0;
        private int _vy = 0;
        private int _vw = 0;
        private int _vh = 0;

        // viewport offset
        private int _vox = 0;
        private int _voy = 0;

        // viewport scale
        private double _vsx = 1.0;
        private double _vsy = 1.0;

        // current location
        private int _cx = 0;
        private int _cy = 0;

        // clip offset
        private int _cox = 0;
        private int _coy = 0;

        private int _mapMode = (int)Gdi.GdiEnum.MM_TEXT;
        private int _bkColor = 0x00FFFFFF;
        private int _bkMode = (int)Gdi.GdiEnum.OPAQUE;
        private int _textColor = 0x00000000;
        private int _textSpace = 0;
        private int _textAlign = (int)Gdi.GdiEnum.TA_TOP | (int)Gdi.GdiEnum.TA_LEFT;
        private int _textDx = 0;
        private int _polyFillMode = (int)Gdi.GdiEnum.ALTERNATE;
        private int _relAbsMode = 0;
        private int _rop2Mode = (int)Gdi.GdiEnum.R2_COPYPEN;
        private int _stretchBltMode = (int)Gdi.GdiEnum.STRETCH_ANDSCANS;
        private long _layout = 0;
        private long _mapperFlags = 0;

        private SvgBrush _brush = null;
        private SvgFont _font = null;
        private SvgPen _pen = null;

        private System.Xml.XmlElement _mask = null;

        #endregion 

        #region Properties

        /// <summary>
        /// Background color.
        /// </summary>
        public int BkColor
        {
            get
            {
                return _bkColor;
            }
            set
            {
                _bkColor = value;
            }
        }

        /// <summary>
        /// Background mode.
        /// </summary>
        public int BkMode
        {
            get
            {
                return _bkMode;
            }
            set
            {
                _bkColor = value;
            }
        }

        /// <summary>
        /// Text color.
        /// </summary>
        public int TextColor
        {
            get
            {
                return _textColor;
            }
            set
            {
                _textColor = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PolyFillMode
        {
            get
            {
                return _polyFillMode;

            }
            set
            {
                _polyFillMode = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int RelAbs
        {
            get
            {
                return _relAbsMode;
            }
            set
            {
                _relAbsMode = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ROP2
        {
            get
            {
                return _rop2Mode;
            }
            set
            {
                _rop2Mode = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int StretchBltMode
        {
            get
            {
                return _stretchBltMode;
            }
            set
            {
                _stretchBltMode = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TextSpace
        {
            get
            {
                return _textSpace;
            }
            set
            {
                _textSpace = value;
            }
        }

        /// <summary>
        /// Text align.
        /// </summary>
        public int TextAlign
        {
            get
            {
                return _textAlign;
            }
            set
            {
                _textAlign = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TextCharacterExtra
        {
            get
            {
                return _textDx;
            }
            set
            {
                _textDx = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long Layout
        {
            get
            {
                return _layout;
            }
            set
            {
                _layout = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long MapperFlags
        {
            get
            {
                return _mapperFlags;
            }
            set
            {
                _mapperFlags = value;
            }
        }

        /// <summary>
        /// Brush instance.
        /// </summary>
        public SvgBrush Brush
        {
            get
            {
                return _brush;
            }
            set
            {
                _brush = value;
            }
        }

        /// <summary>
        /// Font instance.
        /// </summary>
        public SvgFont Font
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
            }
        }

        /// <summary>
        /// Pen instance.
        /// </summary>
        public SvgPen Pen
        {
            get
            {
                return _pen;
            }
            set
            {
                _pen = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Xml.XmlElement Mask
        {
            get
            {
                return _mask;
            }
            set
            {
                _mask = value;
            }
        }

        /// <summary>
        /// Current X Point.
        /// </summary>
        public int CurrentX
        {
            get
            {
                return _cx;
            }
        }

        /// <summary>
        /// Current Y Point.
        /// </summary>
        public int CurrentY
        {
            get
            {
                return _cy;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OffsetClipX
        {
            get
            {
                return _cox;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OffsetClipY
        {
            get
            {
                return _coy;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MapMode
        {
            get
            {
                return _mapMode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Dpi
        {
            get
            {
                return _dpi;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int WindowX
        {
            get
            {
                return _wx;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int WindowY
        {
            get
            {
                return _wy;
            }
        }

        /// <summary>
        /// Represents window width.
        /// </summary>
        public int WindowWidth
        {
            get
            {
                return _ww;
            }
        }

        /// <summary>
        /// Represents window height.
        /// </summary>
        public int WindowHeight
        {
            get
            {
                return _wh;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="gdi"></param>
        public SvgDc(SvgGdi gdi)
        {
            _gdi = gdi;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Specifies which window point maps to the viewport origin (0,0).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        public void SetWindowOrgEx(int x, int y, WMFConverter.Gdi.Point old)
        {
            if (old != null)
            {
                old.X = _wx;
                old.Y = _wy;
            }
            _wx = x;
            _wy = y;
        }

        /// <summary>
        /// Sets the horizontal and vertical extents of the window for a device context by using the specified values.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="old"></param>
        public void SetWindowExtEx(int width, int height, WMFConverter.Gdi.Size old)
        {
            if (old != null)
            {
                old.Width = _ww;
                old.Height = _wh;
            }
            _ww = width;
            _wh = height;
        }

        /// <summary>
        /// Modifies the window origin for a device context using the specified horizontal and vertical offsets.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        public void OffSetWindowOrgEx(int x, int y, WMFConverter.Gdi.Point old)
        {
            if (old != null)
            {
                old.X = _wox;
                old.Y = _woy;
            }
            _wox += x;
            _woy += y;
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
            // TODO
            _wsx = (_wsx * x) / xd;
            _wsy = (_wsy * y) / yd;
        }

        /// <summary>
        /// Specifies which device point maps to the window origin (0,0).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        public void SetViewportOrgEx(int x, int y, WMFConverter.Gdi.Point old)
        {
            if (old != null)
            {
                old.X = _vx;
                old.Y = _vy;
            }
            _vx = x;
            _vy = y;
        }

        /// <summary>
        /// Sets the horizontal and vertical extents of the viewport for a device context by using the specified values.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="old"></param>
        public void SetViewportExtEx(int width, int height, WMFConverter.Gdi.Size old)
        {
            if (old != null)
            {
                old.Width = _vw;
                old.Height = _vh;
            }
            _vw = width;
            _vh = height;
        }

        /// <summary>
        /// Modifies the viewport origin for a device context using the specified horizontal and vertical offsets.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        public void OffSetViewportOrgEx(int x, int y, WMFConverter.Gdi.Point old)
        {
            if (old != null)
            {
                old.X = _vox;
                old.Y = _voy;
            }
            _vox = x;
            _voy = y;
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
            // TODO
            _vsx = (_vsx * x) / xd;
            _vsy = (_vsy * y) / yd;
        }

        /// <summary>
        /// Moves the clipping region of a device context by the specified offsets.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void OffSetClipRgn(int x, int y)
        {
            _cox = x;
            _coy = y;
        }

        /// <summary>
        /// Sets the mapping mode of the specified device context. 
        /// The mapping mode defines the unit of measure used to transform page-space units into device-space units, and also defines the orientation of the device's x and y axes.
        /// </summary>
        /// <param name="mode"></param>
        public void SetMapMode(int mode)
        {
            _mapMode = mode;
            switch (mode)
            {
                case (int)Gdi.GdiEnum.MM_HIENGLISH:
                    _mx = 0.09;
                    _my = -0.09;
                    break;
                case (int)Gdi.GdiEnum.MM_LOENGLISH:
                    _mx = 0.9;
                    _my = -0.9;
                    break;
                case (int)Gdi.GdiEnum.MM_HIMETRIC:
                    _mx = 0.03543307;
                    _my = -0.03543307;
                    break;
                case (int)Gdi.GdiEnum.MM_LOMETRIC:
                    _mx = 0.3543307;
                    _my = -0.3543307;
                    break;
                case (int)Gdi.GdiEnum.MM_TWIPS:
                    _mx = 0.0625;
                    _my = -0.0625;
                    break;
                default:
                    _mx = 1.0;
                    _my = 1.0;
                    break;
            }
        }

        /// <summary>
        /// Updates the current position to the specified point and optionally returns the previous position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        public void MoveToEx(int x, int y, WMFConverter.Gdi.Point old)
        {
            if (old != null)
            {
                old.X = _cx;
                old.Y = _cy;
            }
            _cx = x;
            _cy = y;
        }

        /// <summary>
        /// Return the absolute X position from x point.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double ToAbsoluteX(double x)
        {
            // TODO Handle Viewport
            return ((_ww >= 0) ? 1 : -1) * (_mx * x - (_wx + _wox)) / _wsx;
        }

        /// <summary>
        /// Return the absolute Y position from y point.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public double ToAbsoluteY(double y)
        {
            // TODO Handle Viewport
            return ((_wh >= 0) ? 1 : -1) * (_my * y - (_wy + _woy)) / _wsy;
        }

        /// <summary>
        /// Return the relative X position from x point.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double ToRelativeX(double x)
        {
            // TODO Handle Viewport
            return ((_ww >= 0) ? 1 : -1) * (_mx * x) / _wsx;
        }

        /// <summary>
        /// Return the relative Y position from y point.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public double ToRelativeY(double y)
        {
            // TODO Handle Viewport
            return ((_wh >= 0) ? 1 : -1) * (_my * y) / _wsy;
        }

        /// <summary>
        /// Define Dpi value
        /// </summary>
        /// <param name="dpi"></param>
        public void SetDpi(int dpi)
        {
            _dpi = (dpi > 0) ? dpi : 1440;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rop"></param>
        /// <returns></returns>
        public string GetRopFilter(long rop)
        {
            string name = null;
            System.Xml.XmlDocument doc = _gdi.Document;
            
            if (rop == (int)Gdi.GdiEnum.BLACKNESS)
            {
                name = "BLACKNESS_FILTER";
                System.Xml.XmlElement filter = doc.GetElementById(name);
                if (filter == null)
                {
                    filter = _gdi.Document.CreateElement("filter");
                    filter.SetAttribute("id", name);
                    //filter.setIdAttribute("id", true);

                    System.Xml.XmlElement feColorMatrix = doc.CreateElement("feColorMatrix");
                    feColorMatrix.SetAttribute("type", "matrix");
                    feColorMatrix.SetAttribute("in", "SourceGraphic");
                    feColorMatrix.SetAttribute("values", "0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0");
                    filter.AppendChild(feColorMatrix);

                    _gdi.DefsElement.AppendChild(filter);
                }
            }
            else if (rop == (int)Gdi.GdiEnum.NOTSRCERASE)
            {
                name = "NOTSRCERASE_FILTER";
                System.Xml.XmlElement filter = doc.GetElementById(name);
                if (filter == null)
                {
                    filter = _gdi.Document.CreateElement("filter");
                    filter.SetAttribute("id", name);
                    //filter.setIdAttribute("id", true);

                    System.Xml.XmlElement feComposite = doc.CreateElement("feComposite");
                    feComposite.SetAttribute("in", "SourceGraphic");
                    feComposite.SetAttribute("in2", "BackgroundImage");
                    feComposite.SetAttribute("operator", "arithmetic");
                    feComposite.SetAttribute("k1", "1");
                    feComposite.SetAttribute("result", "result0");
                    filter.AppendChild(feComposite);

                    System.Xml.XmlElement feColorMatrix = doc.CreateElement("feColorMatrix");
                    feColorMatrix.SetAttribute("in", "result0");
                    feColorMatrix.SetAttribute("values", "-1 0 0 0 1 0 -1 0 0 1 0 0 -1 0 1 0 0 0 1 0");
                    filter.AppendChild(feColorMatrix);

                    _gdi.DefsElement.AppendChild(filter);
                }
            }
            else if (rop == (int)Gdi.GdiEnum.NOTSRCCOPY)
            {
                name = "NOTSRCCOPY_FILTER";
                System.Xml.XmlElement filter = doc.GetElementById(name);
                if (filter == null)
                {
                    filter = _gdi.Document.CreateElement("filter");
                    filter.SetAttribute("id", name);
                    //filter.setIdAttribute("id", true);

                    System.Xml.XmlElement feColorMatrix = doc.CreateElement("feColorMatrix");
                    feColorMatrix.SetAttribute("type", "matrix");
                    feColorMatrix.SetAttribute("in", "SourceGraphic");
                    feColorMatrix.SetAttribute("values", "-1 0 0 0 1 0 -1 0 0 1 0 0 -1 0 1 0 0 0 1 0");
                    filter.AppendChild(feColorMatrix);

                    _gdi.DefsElement.AppendChild(filter);
                }
            }
            else if (rop == (int)Gdi.GdiEnum.SRCERASE)
            {
                name = "SRCERASE_FILTER";
                System.Xml.XmlElement filter = doc.GetElementById(name);
                if (filter == null)
                {
                    filter = _gdi.Document.CreateElement("filter");
                    filter.SetAttribute("id", name);
                    //filter.setIdAttribute("id", true);

                    System.Xml.XmlElement feColorMatrix = doc.CreateElement("feColorMatrix");
                    feColorMatrix.SetAttribute("type", "matrix");
                    feColorMatrix.SetAttribute("in", "BackgroundImage");
                    feColorMatrix.SetAttribute("values", "-1 0 0 0 1 0 -1 0 0 1 0 0 -1 0 1 0 0 0 1 0");
                    feColorMatrix.SetAttribute("result", "result0");
                    filter.AppendChild(feColorMatrix);

                    System.Xml.XmlElement feComposite = doc.CreateElement("feComposite");
                    feComposite.SetAttribute("in", "SourceGraphic");
                    feComposite.SetAttribute("in2", "result0");
                    feComposite.SetAttribute("operator", "arithmetic");
                    feComposite.SetAttribute("k2", "1");
                    feComposite.SetAttribute("k3", "1");
                    filter.AppendChild(feComposite);

                    _gdi.DefsElement.AppendChild(filter);
                }
            }
            else if (rop == (int)Gdi.GdiEnum.PATINVERT)
            {
                // TODO
            }
            else if (rop == (int)Gdi.GdiEnum.SRCINVERT)
            {
                // TODO
            }
            else if (rop == (int)Gdi.GdiEnum.DSTINVERT)
            {
                name = "DSTINVERT_FILTER";
                System.Xml.XmlElement filter = doc.GetElementById(name);
                if (filter == null)
                {
                    filter = _gdi.Document.CreateElement("filter");
                    filter.SetAttribute("id", name);
                    //filter.SetIdAttribute("id", true);

                    System.Xml.XmlElement feColorMatrix = doc.CreateElement("feColorMatrix");
                    feColorMatrix.SetAttribute("type", "matrix");
                    feColorMatrix.SetAttribute("in", "BackgroundImage");
                    feColorMatrix.SetAttribute("values", "-1 0 0 0 1 0 -1 0 0 1 0 0 -1 0 1 0 0 0 1 0");
                    filter.AppendChild(feColorMatrix);

                    _gdi.DefsElement.AppendChild(filter);
                }
            }
            else if (rop == (int)Gdi.GdiEnum.SRCAND)
            {
                name = "SRCAND_FILTER";
                System.Xml.XmlElement filter = doc.GetElementById(name);
                if (filter == null)
                {
                    filter = _gdi.Document.CreateElement("filter");
                    filter.SetAttribute("id", name);
                    //filter.setIdAttribute("id", true);

                    System.Xml.XmlElement feComposite = doc.CreateElement("feComposite");
                    feComposite.SetAttribute("in", "SourceGraphic");
                    feComposite.SetAttribute("in2", "BackgroundImage");
                    feComposite.SetAttribute("operator", "arithmetic");
                    feComposite.SetAttribute("k1", "1");
                    filter.AppendChild(feComposite);

                    _gdi.DefsElement.AppendChild(filter);
                }
            }
            else if (rop == (int)Gdi.GdiEnum.MERGEPAINT)
            {
                name = "MERGEPAINT_FILTER";
                System.Xml.XmlElement filter = doc.GetElementById(name);
                if (filter == null)
                {
                    filter = _gdi.Document.CreateElement("filter");
                    filter.SetAttribute("id", name);
                    //filter.setIdAttribute("id", true);

                    System.Xml.XmlElement feColorMatrix = doc.CreateElement("feColorMatrix");
                    feColorMatrix.SetAttribute("type", "matrix");
                    feColorMatrix.SetAttribute("in", "SourceGraphic");
                    feColorMatrix.SetAttribute("values", "-1 0 0 0 1 0 -1 0 0 1 0 0 -1 0 1 0 0 0 1 0");
                    feColorMatrix.SetAttribute("result", "result0");
                    filter.AppendChild(feColorMatrix);

                    System.Xml.XmlElement feComposite = doc.CreateElement("feComposite");
                    feComposite.SetAttribute("in", "result0");
                    feComposite.SetAttribute("in2", "BackgroundImage");
                    feComposite.SetAttribute("operator", "arithmetic");
                    feComposite.SetAttribute("k1", "1");
                    filter.AppendChild(feComposite);

                    _gdi.DefsElement.AppendChild(filter);
                }
            }
            else if (rop == (int)Gdi.GdiEnum.MERGECOPY)
            {
                // TODO
            }
            else if (rop == (int)Gdi.GdiEnum.SRCPAINT)
            {
                name = "SRCPAINT_FILTER";
                System.Xml.XmlElement filter = doc.GetElementById(name);
                if (filter == null)
                {
                    filter = _gdi.Document.CreateElement("filter");
                    filter.SetAttribute("id", name);
                    //filter.setIdAttribute("id", true);

                    System.Xml.XmlElement feComposite = doc.CreateElement("feComposite");
                    feComposite.SetAttribute("in", "SourceGraphic");
                    feComposite.SetAttribute("in2", "BackgroundImage");
                    feComposite.SetAttribute("operator", "arithmetic");
                    feComposite.SetAttribute("k2", "1");
                    feComposite.SetAttribute("k3", "1");
                    filter.AppendChild(feComposite);

                    _gdi.DefsElement.AppendChild(filter);
                }
            }
            else if (rop == (int)Gdi.GdiEnum.PATCOPY)
            {
                // TODO
            }
            else if (rop == (int)Gdi.GdiEnum.PATPAINT)
            {
                // TODO
            }
            else if (rop == (int)Gdi.GdiEnum.WHITENESS)
            {
                name = "WHITENESS_FILTER";
                System.Xml.XmlElement filter = doc.GetElementById(name);
                if (filter == null)
                {
                    filter = _gdi.Document.CreateElement("filter");
                    filter.SetAttribute("id", name);
                    //filter.SetIdAttribute("id", true);

                    System.Xml.XmlElement feColorMatrix = doc.CreateElement("feColorMatrix");
                    feColorMatrix.SetAttribute("type", "matrix");
                    feColorMatrix.SetAttribute("in", "SourceGraphic");
                    feColorMatrix.SetAttribute("values", "1 0 0 0 1 0 1 0 0 1 0 0 1 0 1 0 0 0 1 0");
                    filter.AppendChild(feColorMatrix);

                    _gdi.DefsElement.AppendChild(filter);
                }
            }

            if (name != null)
            {
                if (!doc.DocumentElement.HasAttribute("enable-background"))
                {
                    doc.DocumentElement.SetAttribute("enable-background", "new");
                }
                return "url(#" + name + ")";
            }
            return null;
        }

        /// <summary>
        /// Clone this object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SvgDc [gdi=" + _gdi + ", dpi=" + _dpi + ", wx=" + _wx + ", wy="
                    + _wy + ", ww=" + _ww + ", wh=" + _wh + ", wox=" + _wox + ", woy="
                    + _woy + ", wsx=" + _wsx + ", wsy=" + _wsy + ", mx=" + _mx
                    + ", my=" + _my + ", vx=" + _vx + ", vy=" + _vy + ", vw=" + _vw
                    + ", vh=" + _vh + ", vox=" + _vox + ", voy=" + _voy + ", vsx="
                    + _vsx + ", vsy=" + _vsy + ", cx=" + _cx + ", cy=" + _cy
                    + ", mapMode=" + _mapMode + ", bkColor=" + _bkColor + ", bkMode="
                    + _bkMode + ", textColor=" + _textColor + ", textSpace="
                    + _textSpace + ", textAlign=" + _textAlign + ", textDx=" + _textDx
                    + ", polyFillMode=" + _polyFillMode + ", relAbsMode="
                    + _relAbsMode + ", rop2Mode=" + _rop2Mode + ", stretchBltMode="
                    + _stretchBltMode + ", brush=" + _brush + ", font=" + _font
                    + ", pen=" + _pen + "]";
        }

        #endregion
    }
}
