using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Wmf
{
    /// <summary>
    /// Windows Metafile - Represents WMF of GDI object.
    /// </summary>
    public class WmfGdi : Gdi.IGdi
    {
        #region Local Variables

        private byte[] _placeableHeader;
        private byte[] _header;

        private List<WMFConverter.Gdi.IGdiObject> _objects = new List<WMFConverter.Gdi.IGdiObject>();
        private List<byte[]> _records = new List<byte[]>();

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WmfGdi()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes the document in the stream.
        /// </summary>
        /// <param name="output"></param>
        public void Write(System.IO.Stream output)
        {
		    Footer();
		    if (_placeableHeader != null) 
                output.Write(_placeableHeader,0,_placeableHeader.Length);
		    if (_header != null) 
                output.Write(_header,0,_placeableHeader.Length);

            foreach(var item in _records)
                output.Write(item,0,_placeableHeader.Length);
            output.Flush();
	    }

        /// <summary>
        /// Defines header document.
        /// </summary>
        /// <param name="vsx"></param>
        /// <param name="vsy"></param>
        /// <param name="vex"></param>
        /// <param name="vey"></param>
        /// <param name="dpi"></param>
        public void PlaceableHeader(int vsx, int vsy, int vex, int vey, int dpi)
        {
            byte[] record = new byte[22];
            int pos = 0;
            pos = SetUint32(record, pos, 0x9AC6CDD7);
            pos = SetInt16(record, pos, 0x0000);
            pos = SetInt16(record, pos, vsx);
            pos = SetInt16(record, pos, vsy);
            pos = SetInt16(record, pos, vex);
            pos = SetInt16(record, pos, vey);
            pos = SetUint16(record, pos, dpi);
            pos = SetUint32(record, pos, 0x00000000);

            int checksum = 0;
            for (int i = 0; i < record.Length - 2; i += 2)
                checksum ^= (0xFF & record[i]) | ((0xFF & record[i + 1]) << 8);

            pos = SetUint16(record, pos, checksum);
            _placeableHeader = record;
        }

        /// <summary>
        /// Defines header document.
        /// </summary>
	    public void Header() 
        {
		    byte[] record = new byte[18];
		    int pos = 0;
		    pos = SetUint16(record, pos, 0x0001);
		    pos = SetUint16(record, pos, 0x0009);
		    pos = SetUint16(record, pos, 0x0300);
		    pos = SetUint32(record, pos, 0x0000); // dummy size
		    pos = SetUint16(record, pos, 0x0000); // dummy noObjects
		    pos = SetUint32(record, pos, 0x0000); // dummy maxRecords
		    pos = SetUint16(record, pos, 0x0000);
		    _header = record;
	    }

        /// <summary>
        /// Replaces entries in the specified logical palette.
        /// </summary>
        /// <param name="palette"></param>
        /// <param name="startIndex"></param>
        /// <param name="entries"></param>
	    public void AnimatePalette(WMFConverter.Gdi.IGdiPalette palette, int startIndex, int[] entries) 
        {
		    byte[] record = new byte[22];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
		    pos = SetUint16(record, pos, (int)WmfConstants.RECORD_ANIMATE_PALETTE);
		    pos = SetUint16(record, pos, entries.Length);
		    pos = SetUint16(record, pos, startIndex);
		    pos = SetUint16(record, pos, ((WmfPalette)palette).Id);
		    for (int i = 0; i < entries.Length; i++) 
			    pos = SetInt32(record, pos, entries[i]);

		    _records.Add(record);
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
	    public void Arc(int sxr, int syr, int exr, int eyr, int sxa, int sya, int exa, int eya)
        {
		    byte[] record = new byte[22];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_ARC);
		    pos = SetInt16(record, pos, eya);
		    pos = SetInt16(record, pos, exa);
		    pos = SetInt16(record, pos, sya);
		    pos = SetInt16(record, pos, sxa);
		    pos = SetInt16(record, pos, eyr);
		    pos = SetInt16(record, pos, exr);
		    pos = SetInt16(record, pos, syr);
		    pos = SetInt16(record, pos, sxr);
		    _records.Add(record);
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
	    public void BitBlt(byte[] image, int dx, int dy, int dw, int dh, int sx, int sy, long rop) 
        {
		    byte[] record = new byte[22 + (image.Length + image.Length%2)];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_BIT_BLT);
		    pos = SetUint32(record, pos, rop);
		    pos = SetInt16(record, pos, sy);
		    pos = SetInt16(record, pos, sx);
		    pos = SetInt16(record, pos, dw);
		    pos = SetInt16(record, pos, dh);
		    pos = SetInt16(record, pos, dy);
		    pos = SetInt16(record, pos, dx);
		    pos = SetBytes(record, pos, image);
		    if (image.Length%2 == 1) 
                pos = SetByte(record, pos, 0);
		    _records.Add(record);
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
	    public void Chord(int sxr, int syr, int exr, int eyr, int sxa, int sya, int exa, int eya) 
        {
		    byte[] record = new byte[22];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_CHORD);
		    pos = SetInt16(record, pos, eya);
		    pos = SetInt16(record, pos, exa);
		    pos = SetInt16(record, pos, sya);
		    pos = SetInt16(record, pos, sxa);
		    pos = SetInt16(record, pos, eyr);
		    pos = SetInt16(record, pos, exr);
		    pos = SetInt16(record, pos, syr);
		    pos = SetInt16(record, pos, sxr);
		    _records.Add(record);
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
		    byte[] record = new byte[14];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_CREATE_BRUSH_INDIRECT);
		    pos = SetUint16(record, pos, style);
		    pos = SetInt32(record, pos, color);
		    pos = SetUint16(record, pos, hatch);
		    _records.Add(record);

		    WmfBrush brush = new WmfBrush(_objects.Count(), style, color, hatch);
		    _objects.Add(brush);
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

		    byte[] record = new byte[24 + (faceName.Length + faceName.Length%2)];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_CREATE_FONT_INDIRECT);
		    pos = SetInt16(record, pos, height);
		    pos = SetInt16(record, pos, width);
		    pos = SetInt16(record, pos, escapement);
		    pos = SetInt16(record, pos, orientation);
		    pos = SetInt16(record, pos, weight);
		    pos = SetByte(record, pos, (italic) ? 0x01 : 0x00);
		    pos = SetByte(record, pos, (underline) ? 0x01 : 0x00);
		    pos = SetByte(record, pos, (strikeout) ? 0x01 : 0x00);
		    pos = SetByte(record, pos, charset);
		    pos = SetByte(record, pos, outPrecision);
		    pos = SetByte(record, pos, clipPrecision);
		    pos = SetByte(record, pos, quality);
		    pos = SetByte(record, pos, pitchAndFamily);
		    pos = SetBytes(record, pos, faceName);
		    if (faceName.Length%2 == 1) 
                pos = SetByte(record, pos, 0);
		    _records.Add(record);

		    WmfFont font = new WmfFont(_objects.Count(), height, width, escapement,
				    orientation, weight, italic, underline, strikeout, charset, outPrecision,
				    clipPrecision, quality, pitchAndFamily, faceName);
		    _objects.Add(font);
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
		    byte[] record = new byte[10 + entries.Length * 4];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_CREATE_PALETTE);
		    pos = SetUint16(record, pos, version);
		    pos = SetUint16(record, pos, entries.Length);
		    for (int i = 0; i < entries.Length; i++) 
			    pos = SetInt32(record, pos, entries[i]);
		    
		    _records.Add(record);

		    WMFConverter.Gdi.IGdiPalette palette = new WmfPalette(_objects.Count(), version, entries);
		    _objects.Add(palette);
		    return palette;
	    }

        /// <summary>
        /// Creates a logical brush with the specified bitmap pattern.
        /// The bitmap can be a DIB section bitmap, which is created by the CreateDIBSection function, or it can be a device-dependent bitmap.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
	    public WMFConverter.Gdi.IGdiPatternBrush CreatePatternBrush(byte[] image) 
        {
		    byte[] record = new byte[6 + (image.Length + image.Length%2)];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_CREATE_PATTERN_BRUSH);
		    pos = SetBytes(record, pos, image);
		    if (image.Length%2 == 1) pos = SetByte(record, pos, 0);
		    _records.Add(record);

		    WMFConverter.Gdi.IGdiPatternBrush brush = new WmfPatternBrush(_objects.Count(), image);
		    _objects.Add(brush);
		    return brush;
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
		    byte[] record = new byte[16];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_CREATE_PEN_INDIRECT);
		    pos = SetUint16(record, pos, style);
		    pos = SetInt16(record, pos, width);
		    pos = SetInt16(record, pos, 0);
		    pos = SetInt32(record, pos, color);
		    _records.Add(record);

		    WmfPen pen = new WmfPen(_objects.Count(), style, width, color);
		    _objects.Add(pen);
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
		    byte[] record = new byte[14];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_CREATE_RECT_RGN);
		    pos = SetInt16(record, pos, bottom);
		    pos = SetInt16(record, pos, right);
		    pos = SetInt16(record, pos, top);
		    pos = SetInt16(record, pos, left);
		    _records.Add(record);

		    WmfRectRegion rgn = new WmfRectRegion(_objects.Count(), left, top, right, bottom);
		    _objects.Add(rgn);
		    return rgn;
	    }

        /// <summary>
        /// Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the object. 
        /// After the object is deleted, the specified handle is no longer valid.
        /// </summary>
        /// <param name="obj"></param>
	    public void DeleteObject(WMFConverter.Gdi.IGdiObject obj) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_DELETE_OBJECT);
            pos = SetUint16(record, pos, ((WmfObject)obj).Id);
		    _records.Add(record);
            _objects.Remove((WmfObject)obj);
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
	    public void DibBitBlt(byte[] image, int dx, int dy, int dw, int dh, int sx, int sy, long rop) 
        {
		    byte[] record = new byte[22 + (image.Length + image.Length%2)];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_DIB_BIT_BLT);
		    pos = SetUint32(record, pos, rop);
		    pos = SetInt16(record, pos, sy);
		    pos = SetInt16(record, pos, sx);
		    pos = SetInt16(record, pos, dw);
		    pos = SetInt16(record, pos, dh);
		    pos = SetInt16(record, pos, dy);
		    pos = SetInt16(record, pos, dx);
		    pos = SetBytes(record, pos, image);
		    if (image.Length%2 == 1) 
                pos = SetByte(record, pos, 0);
		    _records.Add(record);
	    }

        /// <summary>
        /// Create Dib Pattern Brush object instance.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="usage"></param>
        /// <returns></returns>
	    public WMFConverter.Gdi.IGdiPatternBrush DibCreatePatternBrush(byte[] image, int usage) 
        {
		    byte[] record = new byte[10 + (image.Length + image.Length%2)];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_DIB_CREATE_PATTERN_BRUSH);
		    pos = SetInt32(record, pos, usage);
		    pos = SetBytes(record, pos, image);
		    if (image.Length%2 == 1) 
                pos = SetByte(record, pos, 0);
		    _records.Add(record);

		    // TODO usage
		    WMFConverter.Gdi.IGdiPatternBrush brush = new WmfPatternBrush(_objects.Count(), image);
		    _objects.Add(brush);
		    return brush;
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
		    byte[] record = new byte[26 + (image.Length + image.Length%2)];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_DIB_STRETCH_BLT);
		    pos = SetUint32(record, pos, rop);
		    pos = SetInt16(record, pos, sh);
		    pos = SetInt16(record, pos, sw);
		    pos = SetInt16(record, pos, sy);
		    pos = SetInt16(record, pos, sx);
		    pos = SetInt16(record, pos, dw);
		    pos = SetInt16(record, pos, dh);
		    pos = SetInt16(record, pos, dy);
		    pos = SetInt16(record, pos, dx);
		    pos = SetBytes(record, pos, image);
		    if (image.Length%2 == 1) 
                pos = SetByte(record, pos, 0);
		    _records.Add(record);
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
		    byte[] record = new byte[14];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_ELLIPSE);
		    pos = SetInt16(record, pos, ey);
		    pos = SetInt16(record, pos, ex);
		    pos = SetInt16(record, pos, sy);
		    pos = SetInt16(record, pos, sx);
		    _records.Add(record);
	    }

        /// <summary>
        /// Enables an application to access the system-defined device capabilities that are not available through GDI. 
        /// Escape calls made by an application are translated and sent to the driver.
        /// </summary>
        /// <param name="data"></param>
	    public void Escape(byte[] data) 
        {
		    byte[] record = new byte[10 + (data.Length + data.Length%2)];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_ESCAPE);
		    pos = SetBytes(record, pos, data);
		    if (data.Length%2 == 1)
                pos = SetByte(record, pos, 0);
		    _records.Add(record);
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
		    byte[] record = new byte[14];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_EXCLUDE_CLIP_RECT);
		    pos = SetInt16(record, pos, bottom);
		    pos = SetInt16(record, pos, right);
		    pos = SetInt16(record, pos, top);
		    pos = SetInt16(record, pos, left);
		    _records.Add(record);

		    // TODO
		    return (int)WMFConverter.Gdi.RegionEnum.COMPLEXREGION;
	    }

        /// <summary>
        /// Fills an area of the display surface with the current brush.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <param name="type"></param>
	    public void ExtFloodFill(int x, int y, int color, int type) 
        {
		    byte[] record = new byte[16];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_EXT_FLOOD_FILL);
		    pos = SetUint16(record, pos, type);
		    pos = SetInt32(record, pos, color);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    _records.Add(record);
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
        /// <param name="lpdx"></param>
	    public void ExtTextOut(int x, int y, int options, int[] rect, byte[] text, int[] lpdx) 
        {
		    if (rect != null && rect.Length != 4) 
			    throw new ArgumentException("rect must be 4 length.");
		    
		    byte[] record = new byte[14 + ((rect != null) ? 8 : 0) + (text.Length + text.Length%2) + (lpdx.Length * 2)];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_EXT_TEXT_OUT);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    pos = SetInt16(record, pos, text.Length);
		    pos = SetInt16(record, pos, options);
		    if (rect != null) 
            {
			    pos = SetInt16(record, pos, rect[0]);
			    pos = SetInt16(record, pos, rect[1]);
			    pos = SetInt16(record, pos, rect[2]);
			    pos = SetInt16(record, pos, rect[3]);
		    }
		    pos = SetBytes(record, pos, text);
		    if (text.Length%2 == 1) pos = SetByte(record, pos, 0);
		    for (int i = 0; i < lpdx.Length; i++) 
			    pos = SetInt16(record, pos, lpdx[i]);
		    
		    _records.Add(record);
	    }

        /// <summary>
        /// Fills a region by using the specified brush.
        /// </summary>
        /// <param name="rgn"></param>
        /// <param name="brush"></param>
	    public void FillRgn(WMFConverter.Gdi.IGdiRegion rgn, WMFConverter.Gdi.IGdiBrush brush) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_FLOOD_FILL);
            pos = SetUint16(record, pos, ((WmfBrush)brush).Id);
            pos = SetUint16(record, pos, ((WmfRegion)rgn).Id);
		    _records.Add(record);
	    }

        /// <summary>
        /// Fills an area of the display surface with the current brush. 
        /// The area is assumed to be bounded as specified by the crFill parameter.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
	    public void FloodFill(int x, int y, int color) 
        {
		    byte[] record = new byte[16];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_FLOOD_FILL);
		    pos = SetInt32(record, pos, color);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    _records.Add(record);
	    }

        /// <summary>
        /// Draws a border around the specified region by using the specified brush.
        /// </summary>
        /// <param name="rgn"></param>
        /// <param name="brush"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
	    public void FrameRgn(WMFConverter.Gdi.IGdiRegion rgn, WMFConverter.Gdi.IGdiBrush brush, int w, int h) 
        {
		    byte[] record = new byte[14];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_FRAME_RGN);
		    pos = SetInt16(record, pos, h);
		    pos = SetInt16(record, pos, w);
            pos = SetUint16(record, pos, ((WmfBrush)brush).Id);
            pos = SetUint16(record, pos, ((WmfRegion)rgn).Id);
		    _records.Add(record);
	    }

        /// <summary>
        /// Creates a new clipping region from the intersection of the current clipping region and the specified rectangle.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
	    public void IntersectClipRect(int left, int top, int right, int bottom) 
        {
		    byte[] record = new byte[16];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_INTERSECT_CLIP_RECT);
		    pos = SetInt16(record, pos, bottom);
		    pos = SetInt16(record, pos, right);
		    pos = SetInt16(record, pos, top);
		    pos = SetInt16(record, pos, left);
		    _records.Add(record);
	    }

        /// <summary>
        /// Inverts the colors in the specified region.
        /// </summary>
        /// <param name="rgn"></param>
	    public void InvertRgn(WMFConverter.Gdi.IGdiRegion rgn) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_INVERT_RGN);
            pos = SetUint16(record, pos, ((WmfRegion)rgn).Id);
		    _records.Add(record);
	    }

        /// <summary>
        /// Draws a line from the current position up to, but not including, the specified point.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
	    public void LineTo(int ex, int ey) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_LINE_TO);
		    pos = SetInt16(record, pos, ey);
		    pos = SetInt16(record, pos, ex);
		    _records.Add(record);
	    }

        /// <summary>
        /// Updates the current position to the specified point and optionally returns the previous position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
	    public void MoveToEx(int x, int y, WMFConverter.Gdi.Point old) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_MOVE_TO_EX);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    //TODO old
		    _records.Add(record);
	    }

        /// <summary>
        /// Moves the clipping region of a device context by the specified offsets.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
	    public void OffsetClipRgn(int x, int y) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_OFFSET_CLIP_RGN);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    _records.Add(record);
	    }

        /// <summary>
        /// Modifies the viewport origin for a device context using the specified horizontal and vertical offsets.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="point"></param>
	    public void OffsetViewportOrgEx(int x, int y, WMFConverter.Gdi.Point point) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_OFFSET_VIEWPORT_ORG_EX);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    // TODO
		    _records.Add(record);
	    }

        /// <summary>
        /// Modifies the window origin for a device context using the specified horizontal and vertical offsets.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="point"></param>
	    public void OffsetWindowOrgEx(int x, int y, WMFConverter.Gdi.Point point) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_OFFSET_WINDOW_ORG_EX);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    // TODO
		    _records.Add(record);
	    }

        /// <summary>
        /// Paints the specified region by using the brush currently selected into the device context.
        /// </summary>
        /// <param name="rgn"></param>
	    public void PaintRgn(WMFConverter.Gdi.IGdiRegion rgn) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_PAINT_RGN);
            pos = SetUint16(record, pos, ((WmfRegion)rgn).Id);
		    _records.Add(record);
	    }

        /// <summary>
        /// Paints the specified rectangle using the brush that is currently selected into the specified device context.
        /// The brush color and the surface color or colors are combined by using the specified raster operation.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rop"></param>
	    public void PatBlt(int x, int y, int width, int height, long rop) 
        {
		    byte[] record = new byte[18];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_PAT_BLT);
		    pos = SetUint32(record, pos, rop);
		    pos = SetInt16(record, pos, height);
		    pos = SetInt16(record, pos, width);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    _records.Add(record);
	    }

        /// <summary>
        /// Draws a pie-shaped wedge bounded by the intersection of an ellipse and two radials. 
        /// The pie is outlined by using the current pen and filled by using the current brush.
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        /// <param name="sxr"></param>
        /// <param name="syr"></param>
        /// <param name="exr"></param>
        /// <param name="eyr"></param>
	    public void Pie(int sx, int sy, int ex, int ey, int sxr, int syr, int exr, int eyr) 
        {
		    byte[] record = new byte[22];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_PIE);
		    pos = SetInt16(record, pos, eyr);
		    pos = SetInt16(record, pos, exr);
		    pos = SetInt16(record, pos, syr);
		    pos = SetInt16(record, pos, sxr);
		    pos = SetInt16(record, pos, ey);
		    pos = SetInt16(record, pos, ex);
		    pos = SetInt16(record, pos, sy);
		    pos = SetInt16(record, pos, sx);
		    _records.Add(record);
	    }

        /// <summary>
        /// Draws a polygon consisting of two or more vertices connected by straight lines. 
        /// The polygon is outlined by using the current pen and filled by using the current brush and polygon fill mode.
        /// </summary>
        /// <param name="points"></param>
	    public void Polygon(WMFConverter.Gdi.Point[] points) 
        {
		    byte[] record = new byte[8 + points.Length * 4];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_POLYGON);
		    pos = SetInt16(record, pos, points.Length);
		    for (int i = 0; i < points.Length; i++) 
            {
			    pos = SetInt16(record, pos, points[i].X);
			    pos = SetInt16(record, pos, points[i].Y);
		    }
		    _records.Add(record);
	    }

        /// <summary>
        /// Element is an SVG basic shape that creates straight lines connecting several points. 
        /// Typically a polyline is used to create open shapes as the last point doesn't have to be connected to the first point. 
        /// </summary>
        /// <param name="points"></param>
	    public void Polyline(WMFConverter.Gdi.Point[] points) 
        {
		    byte[] record = new byte[8 + points.Length * 4];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_POLYLINE);
		    pos = SetInt16(record, pos, points.Length);
		    for (int i = 0; i < points.Length; i++) 
            {
			    pos = SetInt16(record, pos, points[i].X);
			    pos = SetInt16(record, pos, points[i].Y);
		    }
		    _records.Add(record);
	    }

        /// <summary>
        ///  Draws a series of closed polygons. 
        ///  Each polygon is outlined by using the current pen and filled by using the current brush and polygon fill mode. 
        ///  The polygons drawn by this function can overlap.
        /// </summary>
        /// <param name="points"></param>
	    public void PolyPolygon(WMFConverter.Gdi.Point[][] points) 
        {
		    int length = 8;
		    for (int i = 0; i < points.Length; i++) 
			    length += 2 + points[i].Length * 4;
		    
		    byte[] record = new byte[length];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_POLYLINE);
		    pos = SetInt16(record, pos, points.Length);
		    for (int i = 0; i < points.Length; i++) 
			    pos = SetInt16(record, pos, points[i].Length);
		    
		    for (int i = 0; i < points.Length; i++) 
            {
			    for (int j = 0; j < points[i].Length; j++) 
                {
				    pos = SetInt16(record, pos, points[i][j].X);
				    pos = SetInt16(record, pos, points[i][j].Y);
			    }
		    }
		    _records.Add(record);
	    }

        /// <summary>
        /// Maps palette entries from the current logical palette to the system palette.
        /// </summary>
	    public void RealizePalette() 
        {
		    byte[] record = new byte[6];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_REALIZE_PALETTE);
		    _records.Add(record);
	    }

        /// <summary>
        /// Restores a device context (DC) to the specified state. 
        /// The DC is restored by popping state information off a stack created by earlier calls to the SaveDC function.
        /// </summary>
        /// <param name="savedDC"></param>
	    public void RestoreDC(int savedDC) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_RESTORE_DC);
		    pos = SetInt16(record, pos, savedDC);
		    _records.Add(record);
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
		    byte[] record = new byte[14];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_RECTANGLE);
		    pos = SetInt16(record, pos, ey);
		    pos = SetInt16(record, pos, ex);
		    pos = SetInt16(record, pos, sy);
		    pos = SetInt16(record, pos, sx);
		    _records.Add(record);
	    }

        /// <summary>
        /// Increases or decreases the size of a logical palette based on the specified value.
        /// </summary>
        /// <param name="palette"></param>
	    public void ResizePalette(WMFConverter.Gdi.IGdiPalette palette) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_REALIZE_PALETTE);
            pos = SetUint16(record, pos, ((WmfPalette)palette).Id);
		    _records.Add(record);
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
		    byte[] record = new byte[18];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_ROUND_RECT);
		    pos = SetInt16(record, pos, rh);
		    pos = SetInt16(record, pos, rw);
		    pos = SetInt16(record, pos, ey);
		    pos = SetInt16(record, pos, ex);
		    pos = SetInt16(record, pos, sy);
		    pos = SetInt16(record, pos, sx);
		    _records.Add(record);
	    }

        /// <summary>
        /// Save device context (DC).
        /// </summary>
	    public void SeveDC() 
        {
		    byte[] record = new byte[6];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SAVE_DC);
		    _records.Add(record);
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
		    byte[] record = new byte[14];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SCALE_VIEWPORT_EXT_EX);
		    pos = SetInt16(record, pos, yd);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, xd);
		    pos = SetInt16(record, pos, x);
		    // TODO
		    _records.Add(record);
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
		    byte[] record = new byte[14];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SCALE_WINDOW_EXT_EX);
		    pos = SetInt16(record, pos, yd);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, xd);
		    pos = SetInt16(record, pos, x);
		    // TODO
		    _records.Add(record);
	    }

        /// <summary>
        /// Selects a region as the current clipping region for the specified device context.
        /// </summary>
        /// <param name="rgn"></param>
	    public void SelectClipRgn(WMFConverter.Gdi.IGdiRegion rgn) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SELECT_CLIP_RGN);
            pos = SetUint16(record, pos, ((WmfRegion)rgn).Id);
		    _records.Add(record);
	    }

        /// <summary>
        /// Selects an object into the specified device context (DC). The new object replaces the previous object of the same type.
        /// </summary>
        /// <param name="obj"></param>
	    public void SelectObject(WMFConverter.Gdi.IGdiObject obj) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SELECT_OBJECT);
            pos = SetUint16(record, pos, ((WmfObject)obj).Id);
		    _records.Add(record);
	    }

        /// <summary>
        /// Selects the specified logical palette into a device context.
        /// </summary>
        /// <param name="palette"></param>
        /// <param name="mode"></param>
	    public void SelectPalette(WMFConverter.Gdi.IGdiPalette palette, bool mode) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SELECT_PALETTE);
		    pos = SetInt16(record, pos, mode ? 1 : 0);
            pos = SetUint16(record, pos, ((WmfPalette)palette).Id);
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets the current background color to the specified color value, or to the nearest physical color if the device cannot represent the specified color value.
        /// </summary>
        /// <param name="color"></param>
	    public void SetBkColor(int color) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_BK_COLOR);
		    pos = SetInt32(record, pos, color);
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets the background mix mode of the specified device context. 
        /// The background mix mode is used with text, hatched brushes, and pen styles that are not solid lines.
        /// </summary>
        /// <param name="mode"></param>
	    public void SetBkMode(int mode) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_BK_MODE);
		    pos = SetInt16(record, pos, mode);
		    _records.Add(record);
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
		    byte[] record = new byte[24 + (image.Length + image.Length%2)];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_DIBITS_TO_DEVICE);
		    pos = SetUint16(record, pos, colorUse);
		    pos = SetUint16(record, pos, scanlines);
		    pos = SetUint16(record, pos, startscan);
		    pos = SetInt16(record, pos, sy);
		    pos = SetInt16(record, pos, sx);
		    pos = SetInt16(record, pos, dw);
		    pos = SetInt16(record, pos, dh);
		    pos = SetInt16(record, pos, dy);
		    pos = SetInt16(record, pos, dx);
		    pos = SetBytes(record, pos, image);
		    if (image.Length%2 == 1) 
                pos = SetByte(record, pos, 0);
		    _records.Add(record);
	    }

        /// <summary>
        /// Changes the layout of a device context (DC).
        /// </summary>
        /// <param name="layout"></param>
	    public void SetLayout(long layout) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_LAYOUT);
		    pos = SetUint32(record, pos, layout);
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets the mapping mode of the specified device context. 
        /// The mapping mode defines the unit of measure used to transform page-space units into device-space units, and also defines the orientation of the device's x and y axes.
        /// </summary>
        /// <param name="mode"></param>
	    public void SetMapMode(int mode) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_MAP_MODE);
		    pos = SetInt16(record, pos, mode);
		    _records.Add(record);
	    }

        /// <summary>
        /// Alters the algorithm the font mapper uses when it maps logical fonts to physical fonts.
        /// </summary>
        /// <param name="flags"></param>
	    public void SetMapperFlags(long flags) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_MAPPER_FLAGS);
		    pos = SetUint32(record, pos, flags);
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets RGB (red, green, blue) color values and flags in a range of entries in a logical palette.
        /// </summary>
        /// <param name="palette"></param>
        /// <param name="startIndex"></param>
        /// <param name="entries"></param>
	    public void SetPaletteEntries(WMFConverter.Gdi.IGdiPalette palette, int startIndex, int[] entries) 
        {
		    byte[] record = new byte[6 + entries.Length * 4];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_PALETTE_ENTRIES);
            pos = SetUint16(record, pos, ((WmfPalette)palette).Id);
		    pos = SetUint16(record, pos, entries.Length);
		    pos = SetUint16(record, pos, startIndex);
		    for (int i = 0; i < entries.Length; i++) 
			    pos = SetInt32(record, pos, entries[i]);
		    
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets the pixel at the specified coordinates to the specified color.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
	    public void SetPixel(int x, int y, int color) 
        {
		    byte[] record = new byte[14];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_PIXEL);
		    pos = SetInt32(record, pos, color);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets the polygon fill mode for functions that fill polygons.
        /// </summary>
        /// <param name="mode"></param>
	    public void SetPolyFillMode(int mode) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_POLY_FILL_MODE);
		    pos = SetInt16(record, pos, mode);
		    _records.Add(record);
	    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
	    public void SetRelAbs(int mode) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_REL_ABS);
		    pos = SetInt16(record, pos, mode);
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets the current foreground mix mode. 
        /// GDI uses the foreground mix mode to combine pens and interiors of filled objects with the colors already on the screen. 
        /// The foreground mix mode defines how colors from the brush or pen and the colors in the existing image are to be combined.
        /// </summary>
        /// <param name="mode"></param>
	    public void SetROP2(int mode) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_ROP2);
		    pos = SetInt16(record, pos, mode);
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets the bitmap stretching mode in the specified device context.
        /// </summary>
        /// <param name="mode"></param>
	    public void SetStretchBltMode(int mode) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_STRETCH_BLT_MODE);
		    pos = SetInt16(record, pos, mode);
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets the text-alignment flags for the specified device context.
        /// </summary>
        /// <param name="align"></param>
	    public void SetTextAlign(int align) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_TEXT_ALIGN);
		    pos = SetInt16(record, pos, align);
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets the intercharacter spacing. 
        /// Intercharacter spacing is added to each character, including break characters, when the system writes a line of text.
        /// </summary>
        /// <param name="extra"></param>
	    public void SetTextCharacterExtra(int extra) 
        {
		    byte[] record = new byte[8];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_TEXT_CHARACTER_EXTRA);
		    pos = SetInt16(record, pos, extra);
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets the text color for the specified device context to the specified color.
        /// </summary>
        /// <param name="color"></param>
	    public void SetTextColor(int color) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_TEXT_COLOR);
		    pos = SetInt32(record, pos, color);
		    _records.Add(record);
	    }

        /// <summary>
        /// Specifies the amount of space the system should add to the break characters in a string of text. 
        /// The space is added when an application calls the TextOut or ExtTextOut functions.
        /// </summary>
        /// <param name="breakExtra"></param>
        /// <param name="breakCount"></param>
	    public void SetTextJustification(int breakExtra, int breakCount) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_TEXT_COLOR);
		    pos = SetInt16(record, pos, breakCount);
		    pos = SetInt16(record, pos, breakExtra);
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets the horizontal and vertical extents of the viewport for a device context by using the specified values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
	    public void SetViewportExtEx(int x, int y, WMFConverter.Gdi.Size old) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_VIEWPORT_EXT_EX);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    _records.Add(record);
	    }

        /// <summary>
        /// Specifies which device point maps to the window origin (0,0).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
	    public void SetViewportOrgEx(int x, int y, WMFConverter.Gdi.Point old) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_VIEWPORT_ORG_EX);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    _records.Add(record);
	    }

        /// <summary>
        /// Sets the horizontal and vertical extents of the window for a device context by using the specified values.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="old"></param>
	    public void SetWindowExtEx(int width, int height, WMFConverter.Gdi.Size old) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_WINDOW_EXT_EX);
		    pos = SetInt16(record, pos, height);
		    pos = SetInt16(record, pos, width);
		    _records.Add(record);
	    }

        /// <summary>
        /// Specifies which window point maps to the viewport origin (0,0).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
	    public void SetWindowOrgEx(int x, int y, WMFConverter.Gdi.Point old) 
        {
		    byte[] record = new byte[10];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_SET_WINDOW_ORG_EX);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    _records.Add(record);
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
	    public void StretchBlt(byte[] image, int dx, int dy, int dw, int dh,
			    int sx, int sy, int sw, int sh, long rop) 
        {
		    byte[] record = new byte[26 + (image.Length + image.Length%2)];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_STRETCH_BLT);
		    pos = SetUint32(record, pos, rop);
		    pos = SetInt16(record, pos, sh);
		    pos = SetInt16(record, pos, sw);
		    pos = SetInt16(record, pos, sy);
		    pos = SetInt16(record, pos, sx);
		    pos = SetInt16(record, pos, dw);
		    pos = SetInt16(record, pos, dh);
		    pos = SetInt16(record, pos, dy);
		    pos = SetInt16(record, pos, dx);
		    pos = SetBytes(record, pos, image);
		    if (image.Length%2 == 1) 
                pos = SetByte(record, pos, 0);
		    _records.Add(record);
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
		    byte[] record = new byte[26 + (image.Length + image.Length%2)];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_STRETCH_DIBITS);
		    pos = SetUint32(record, pos, rop);
		    pos = SetUint16(record, pos, usage);
		    pos = SetInt16(record, pos, sh);
		    pos = SetInt16(record, pos, sw);
		    pos = SetInt16(record, pos, sy);
		    pos = SetInt16(record, pos, sx);
		    pos = SetInt16(record, pos, dw);
		    pos = SetInt16(record, pos, dh);
		    pos = SetInt16(record, pos, dy);
		    pos = SetInt16(record, pos, dx);
		    pos = SetBytes(record, pos, image);
		    if (image.Length%2 == 1) 
                pos = SetByte(record, pos, 0);
		    _records.Add(record);
	    }

        /// <summary>
        /// Writes a character string at the specified location, using the currently selected font, background color, and text color.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
	    public void TextOut(int x, int y, byte[] text) 
        {
		    byte[] record = new byte[10 + text.Length + text.Length%2];
		    int pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
            pos = SetUint16(record, pos, (int)WmfConstants.RECORD_TEXT_OUT);
		    pos = SetInt16(record, pos, text.Length);
		    pos = SetBytes(record, pos, text);
		    if (text.Length%2 == 1) pos = SetByte(record, pos, 0);
		    pos = SetInt16(record, pos, y);
		    pos = SetInt16(record, pos, x);
		    _records.Add(record);
	    }

        /// <summary>
        /// 
        /// </summary>
	    public void Footer() 
        {
		    int pos = 0;
		    if (_header != null) 
            {
			    long size = _header.Length;
			    long maxRecordSize = 0;
                foreach(var item in _records)
                {
                    size += item.Length;
                    if (item.Length > maxRecordSize) 
                        maxRecordSize = item.Length;
                }
			    pos = SetUint32(_header, 6, size/2);
			    pos = SetUint16(_header, pos, _objects.Count());
			    pos = SetUint32(_header, pos, maxRecordSize / 2);
		    }

		    byte[] record = new byte[6];
		    pos = 0;
		    pos = SetUint32(record, pos, record.Length/2);
		    pos = SetUint16(record, pos, 0x0000);
		    _records.Add(record);
	    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private int SetByte(byte[] output, int pos, int value) 
        {
		    output[pos] = (byte)(0xFF & value);
		    return pos + 1;
	    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="pos"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private int SetBytes(byte[] output, int pos, byte[] data) 
        {
            Array.Copy(data, 0, output, pos, data.Length);
            return pos + data.Length;
        }

        /// <summary>
        /// Sets two bytes as a signed 16-bit integer.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        /// <returns></returns>
	    private int SetInt16(byte[] output, int pos, int value) 
        {
		    output[pos] = (byte)(0xFF & value);
		    output[pos+1] = (byte)(0xFF & (value >> 8));
		    return pos + 2;
	    }

        /// <summary>
        /// Sets four bytes as a signed 32-bit integer.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        /// <returns></returns>
	    private int SetInt32(byte[] output, int pos, int value) 
        {
		    output[pos] = (byte)(0xFF & value);
		    output[pos+1] = (byte)(0xFF & (value >> 8));
		    output[pos+2] = (byte)(0xFF & (value >> 16));
		    output[pos+3] = (byte)(0xFF & (value >> 24));
		    return pos + 4;
	    }

        /// <summary>
        /// Sets two bytes as a unsigned 16-bit integer.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        /// <returns></returns>
	    private int SetUint16(byte[] output, int pos, int value) 
        {
		    output[pos] = (byte)(0xFF & value);
		    output[pos+1] = (byte)(0xFF & (value >> 8));
		    return pos + 2;
	    }

        /// <summary>
        /// Sets four bytes as a unsigned 32-bit integer.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        /// <returns></returns>
	    private int SetUint32(byte[] output, int pos, long value) 
        {
		    output[pos] = (byte)(0xFF & value);
		    output[pos+1] = (byte)(0xFF & (value >> 8));
		    output[pos+2] = (byte)(0xFF & (value >> 16));
            output[pos + 3] = (byte)(0xFF & (value >> 24));
		    return pos + 4;
        }

        #endregion
    }
}
