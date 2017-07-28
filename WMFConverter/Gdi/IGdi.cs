using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Gdi
{
    /// <summary>
    /// Graphics Device Interface - Represents graphics elements
    /// </summary>
    public interface IGdi
    {
        /// <summary>
        /// Create header document.
        /// </summary>
        /// <param name="vsx"></param>
        /// <param name="vsy"></param>
        /// <param name="vex"></param>
        /// <param name="vey"></param>
        /// <param name="dpi"></param>
        void PlaceableHeader(int vsx, int vsy, int vex, int vey, int dpi);

        /// <summary>
        /// Initialize Header document.
        /// </summary>
        void Header();

        /// <summary>
        /// Element is put inside a shape element and defines how an attribute of an element changes over the animation. 
        /// The attribute will change from the initial value to the end value in the duration specified.
        /// </summary>
        /// <param name="palette"></param>
        /// <param name="startIndex"></param>
        /// <param name="entries"></param>
        void AnimatePalette(IGdiPalette palette, int startIndex, int[] entries);

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
        void Arc(int sxr, int syr, int exr, int eyr,
               int sxa, int sya, int exa, int eya);

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
        void BitBlt(byte[] image, int dx, int dy, int dw, int dh, int sx, int sy, long rop);

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
        void Chord(int sxr, int syr, int exr, int eyr,
                 int sxa, int sya, int exa, int eya);

        /// <summary>
        /// Creates a logical brush that has the specified style, color, and pattern.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="color"></param>
        /// <param name="hatch"></param>
        /// <returns></returns>
        IGdiBrush CreateBrushIndirect(int style, int color, int hatch);

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
        IGdiFont CreateFontIndirect(int height, int width, int escapement,
                         int orientation, int weight,
                         bool italic, bool underline, bool strikeout,
                         int charset, int outPrecision, int clipPrecision,
                         int quality, int pitchAndFamily, byte[] faceName);

        /// <summary>
        /// Creates a logical palette.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="palEntry"></param>
        /// <returns></returns>
        IGdiPalette CreatePalette(int version, int[] palEntry);

        /// <summary>
        /// Creates a logical brush with the specified bitmap pattern.
        /// The bitmap can be a DIB section bitmap, which is created by the CreateDIBSection function, or it can be a device-dependent bitmap.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        IGdiPatternBrush CreatePatternBrush(byte[] image);

        /// <summary>
        /// Creates a logical cosmetic pen that has the style, width, and color specified in a structure.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        IGdiPen CreatePenIndirect(int style, int width, int color);

        /// <summary>
        /// Creates a rectangular region.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        IGdiRegion CreateRectRgn(int left, int top, int right, int bottom);

        /// <summary>
        /// Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the object. 
        /// After the object is deleted, the specified handle is no longer valid.
        /// </summary>
        /// <param name="obj"></param>
        void DeleteObject(IGdiObject obj);

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
        void DibBitBlt(byte[] image, int dx, int dy, int dw, int dh,
               int sx, int sy, long rop);

        /// <summary>
        /// Create Dib Pattern Brush object instance.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="usage"></param>
        /// <returns></returns>
        IGdiPatternBrush DibCreatePatternBrush(byte[] image, int usage);

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
        void DibStretchBlt(byte[] image, int dx, int dy, int dw, int dh,
               int sx, int sy, int sw, int sh, long rop);

        /// <summary>
        /// Draws an ellipse. 
        /// The center of the ellipse is the center of the specified bounding rectangle.
        /// The ellipse is outlined by using the current pen and is filled by using the current brush.
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        void Ellipse(int sx, int sy, int ex, int ey);

        /// <summary>
        /// Enables an application to access the system-defined device capabilities that are not available through GDI. 
        /// Escape calls made by an application are translated and sent to the driver.
        /// </summary>
        /// <param name="data"></param>
        void Escape(byte[] data);

        /// <summary>
        ///  Creates a new clipping region that consists of the existing clipping region minus the specified rectangle.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        int ExcludeClipRect(int left, int top, int right, int bottom);

        /// <summary>
        /// Fills an area of the display surface with the current brush.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <param name="type"></param>
        void ExtFloodFill(int x, int y, int color, int type);

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
        void ExtTextOut(int x, int y, int options, int[] rect, byte[] text, int[] lpdx);

        /// <summary>
        /// Fills a region by using the specified brush.
        /// </summary>
        /// <param name="rgn"></param>
        /// <param name="brush"></param>
        void FillRgn(IGdiRegion rgn, IGdiBrush brush);

        /// <summary>
        /// Fills an area of the display surface with the current brush. 
        /// The area is assumed to be bounded as specified by the crFill parameter.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        void FloodFill(int x, int y, int color);

        /// <summary>
        /// Draws a border around the specified region by using the specified brush.
        /// </summary>
        /// <param name="rgn"></param>
        /// <param name="brush"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        void FrameRgn(IGdiRegion rgn, IGdiBrush brush, int w, int h);

        /// <summary>
        /// Creates a new clipping region from the intersection of the current clipping region and the specified rectangle.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        void IntersectClipRect(int left, int top, int right, int bottom);

        /// <summary>
        /// Inverts the colors in the specified region.
        /// </summary>
        /// <param name="rgn"></param>
        void InvertRgn(IGdiRegion rgn);

        /// <summary>
        /// Draws a line from the current position up to, but not including, the specified point.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        void LineTo(int ex, int ey);

        /// <summary>
        /// Updates the current position to the specified point and optionally returns the previous position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        void MoveToEx(int x, int y, Point old);

        /// <summary>
        /// Moves the clipping region of a device context by the specified offsets.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void OffsetClipRgn(int x, int y);

        /// <summary>
        /// Modifies the viewport origin for a device context using the specified horizontal and vertical offsets.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="point"></param>
        void OffsetViewportOrgEx(int x, int y, Point point);

        /// <summary>
        /// Modifies the window origin for a device context using the specified horizontal and vertical offsets.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="point"></param>
        void OffsetWindowOrgEx(int x, int y, Point point);

        /// <summary>
        /// Paints the specified region by using the brush currently selected into the device context.
        /// </summary>
        /// <param name="rgn"></param>
        void PaintRgn(IGdiRegion rgn);

        /// <summary>
        /// Paints the specified rectangle using the brush that is currently selected into the specified device context.
        /// The brush color and the surface color or colors are combined by using the specified raster operation.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rop"></param>
        void PatBlt(int x, int y, int width, int height, long rop);

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
        void Pie(int sx, int sy, int ex, int ey, int sxr, int syr, int exr, int eyr);

        /// <summary>
        /// Draws a polygon consisting of two or more vertices connected by straight lines. 
        /// The polygon is outlined by using the current pen and filled by using the current brush and polygon fill mode.
        /// </summary>
        /// <param name="points"></param>
        void Polygon(Point[] points);

        /// <summary>
        /// Element is an SVG basic shape that creates straight lines connecting several points. 
        /// Typically a polyline is used to create open shapes as the last point doesn't have to be connected to the first point. 
        /// </summary>
        /// <param name="points"></param>
        void Polyline(Point[] points);

        /// <summary>
        ///  Draws a series of closed polygons. 
        ///  Each polygon is outlined by using the current pen and filled by using the current brush and polygon fill mode. 
        ///  The polygons drawn by this function can overlap.
        /// </summary>
        /// <param name="points"></param>
        void PolyPolygon(Point[][] points);

        /// <summary>
        /// Maps palette entries from the current logical palette to the system palette.
        /// </summary>
        void RealizePalette();

        /// <summary>
        /// Restores a device context (DC) to the specified state. 
        /// The DC is restored by popping state information off a stack created by earlier calls to the SaveDC function.
        /// </summary>
        /// <param name="savedDC"></param>
        void RestoreDC(int savedDC);

        /// <summary>
        /// Draws a rectangle. 
        /// The rectangle is outlined by using the current pen and filled by using the current brush.
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        void Rectangle(int sx, int sy, int ex, int ey);

        /// <summary>
        /// Increases or decreases the size of a logical palette based on the specified value.
        /// </summary>
        /// <param name="palette"></param>
        void ResizePalette(IGdiPalette palette);

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
        void RoundRect(int sx, int sy, int ex, int ey, int rw, int rh);

        /// <summary>
        /// Save device context (DC).
        /// </summary>
        void SeveDC();

        /// <summary>
        /// Modifies the viewport for a device context using the ratios formed by the specified multiplicands and divisors.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="xd"></param>
        /// <param name="y"></param>
        /// <param name="yd"></param>
        /// <param name="old"></param>
        void ScaleViewportExtEx(int x, int xd, int y, int yd, Size old);

        /// <summary>
        /// Modifies the window for a device context using the ratios formed by the specified multiplicands and divisors.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="xd"></param>
        /// <param name="y"></param>
        /// <param name="yd"></param>
        /// <param name="old"></param>
        void ScaleWindowExtEx(int x, int xd, int y, int yd, Size old);

        /// <summary>
        /// Selects a region as the current clipping region for the specified device context.
        /// </summary>
        /// <param name="rgn"></param>
        void SelectClipRgn(IGdiRegion rgn);

        /// <summary>
        /// Selects an object into the specified device context (DC). The new object replaces the previous object of the same type.
        /// </summary>
        /// <param name="obj"></param>
        void SelectObject(IGdiObject obj);

        /// <summary>
        /// Selects the specified logical palette into a device context.
        /// </summary>
        /// <param name="palette"></param>
        /// <param name="mode"></param>
        void SelectPalette(IGdiPalette palette, bool mode);

        /// <summary>
        /// Sets the current background color to the specified color value, or to the nearest physical color if the device cannot represent the specified color value.
        /// </summary>
        /// <param name="color"></param>
        void SetBkColor(int color);

        /// <summary>
        /// Sets the background mix mode of the specified device context. 
        /// The background mix mode is used with text, hatched brushes, and pen styles that are not solid lines.
        /// </summary>
        /// <param name="mode"></param>
        void SetBkMode(int mode);

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
        void SetDIBitsToDevice(int dx, int dy, int dw, int dh, int sx, int sy,
                       int startscan, int scanlines, byte[] image, int colorUse);

        /// <summary>
        /// Changes the layout of a device context (DC).
        /// </summary>
        /// <param name="layout"></param>
        void SetLayout(long layout);

        /// <summary>
        /// Sets the mapping mode of the specified device context. 
        /// The mapping mode defines the unit of measure used to transform page-space units into device-space units, and also defines the orientation of the device's x and y axes.
        /// </summary>
        /// <param name="mode"></param>
        void SetMapMode(int mode);

        /// <summary>
        /// Alters the algorithm the font mapper uses when it maps logical fonts to physical fonts.
        /// </summary>
        /// <param name="flags"></param>
        void SetMapperFlags(long flags);

        /// <summary>
        /// Sets RGB (red, green, blue) color values and flags in a range of entries in a logical palette.
        /// </summary>
        /// <param name="palette"></param>
        /// <param name="startIndex"></param>
        /// <param name="entries"></param>
        void SetPaletteEntries(IGdiPalette palette, int startIndex, int[] entries);

        /// <summary>
        /// Sets the pixel at the specified coordinates to the specified color.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        void SetPixel(int x, int y, int color);

        /// <summary>
        /// Sets the polygon fill mode for functions that fill polygons.
        /// </summary>
        /// <param name="mode"></param>
        void SetPolyFillMode(int mode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        void SetRelAbs(int mode);

        /// <summary>
        /// Sets the current foreground mix mode. 
        /// GDI uses the foreground mix mode to combine pens and interiors of filled objects with the colors already on the screen. 
        /// The foreground mix mode defines how colors from the brush or pen and the colors in the existing image are to be combined.
        /// </summary>
        /// <param name="mode"></param>
        void SetROP2(int mode);

        /// <summary>
        /// Sets the bitmap stretching mode in the specified device context.
        /// </summary>
        /// <param name="mode"></param>
        void SetStretchBltMode(int mode);
        
        /// <summary>
        /// Sets the text-alignment flags for the specified device context.
        /// </summary>
        /// <param name="align"></param>
        void SetTextAlign(int align);

        /// <summary>
        /// Sets the intercharacter spacing. 
        /// Intercharacter spacing is added to each character, including break characters, when the system writes a line of text.
        /// </summary>
        /// <param name="extra"></param>
        void SetTextCharacterExtra(int extra);

        /// <summary>
        /// Sets the text color for the specified device context to the specified color.
        /// </summary>
        /// <param name="color"></param>
        void SetTextColor(int color);

        /// <summary>
        /// Specifies the amount of space the system should add to the break characters in a string of text. 
        /// The space is added when an application calls the TextOut or ExtTextOut functions.
        /// </summary>
        /// <param name="breakExtra"></param>
        /// <param name="breakCount"></param>
        void SetTextJustification(int breakExtra, int breakCount);

        /// <summary>
        /// Sets the horizontal and vertical extents of the viewport for a device context by using the specified values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        void SetViewportExtEx(int x, int y, Size old);

        /// <summary>
        /// Specifies which device point maps to the window origin (0,0).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        void SetViewportOrgEx(int x, int y, Point old);

        /// <summary>
        /// Sets the horizontal and vertical extents of the window for a device context by using the specified values.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="old"></param>
        void SetWindowExtEx(int width, int height, Size old);

        /// <summary>
        /// Specifies which window point maps to the viewport origin (0,0).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="old"></param>
        void SetWindowOrgEx(int x, int y, Point old);

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
        void StretchBlt(byte[] image, int dx, int dy, int dw, int dh,
               int sx, int sy, int sw, int sh, long rop);

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
        void StretchDIBits(int dx, int dy, int dw, int dh,
                       int sx, int sy, int sw, int sh,
                       byte[] image, int usage, long rop);

        /// <summary>
        /// Writes a character string at the specified location, using the currently selected font, background color, and text color.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
        void TextOut(int x, int y, byte[] text);

        /// <summary>
        /// 
        /// </summary>
        void Footer();
    }
}
