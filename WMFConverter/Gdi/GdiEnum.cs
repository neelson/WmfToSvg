using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Gdi
{
    /// <summary>
    /// Graphics Device Interface Enum
    /// </summary>
    public enum GdiEnum
    {
        OPAQUE = 2,
        TRANSPARENT = 1,

        TA_BASELINE = 24,
        TA_BOTTOM = 8,
        TA_TOP = 0,
        TA_CENTER = 6,
        TA_LEFT = 0,
        TA_RIGHT = 2,
        TA_NOUPDATECP = 0,
        TA_RTLREADING = 256,
        TA_UPDATECP = 1,
        VTA_BASELINE = 24,
        VTA_CENTER = 6,

        ETO_CLIPPED = 4,
        ETO_NUMERICSLOCAL = 1024,
        ETO_NUMERICSLATIN = 2048,
        ETO_GLYPH_INDEX = 16,
        ETO_OPAQUE = 2,
        ETO_PDY = 8192,
        ETO_RTLREADING = 128,
        ETO_IGNORELANGUAGE = 4096,

        MM_ANISOTROPIC = 8,
        MM_HIENGLISH = 5,
        MM_HIMETRIC = 3,
        MM_ISOTROPIC = 7,
        MM_LOENGLISH = 4,
        MM_LOMETRIC = 2,
        MM_TEXT = 1,
        MM_TWIPS = 6,

        STRETCH_ANDSCANS = 2,
        STRETCH_DELETESCANS = 3,
        STRETCH_HALFTONE = 4,
        STRETCH_ORSCANS = 2,
        BLACKONWHITE = 2,
        COLORONCOLOR = 3,
        HALFTONE = 4,
        WHITEONBLACK = 2,

        ALTERNATE = 1,
        WINDING = 2,

        R2_BLACK = 1,
        R2_COPYPEN = 13,
        R2_MASKNOTPEN = 3,
        R2_MASKPEN = 9,
        R2_MASKPENNOT = 5,
        R2_MERGENOTPEN = 12,
        R2_MERGEPEN = 15,
        R2_MERGEPENNOT = 14,
        R2_NOP = 11,
        R2_NOT = 6,
        R2_NOTCOPYPEN = 4,
        R2_NOTMASKPEN = 8,
        R2_NOTMERGEPEN = 2,
        R2_NOTXORPEN = 10,
        R2_WHITE = 16,
        R2_XORPEN = 7,

        BLACKNESS = 66,
        DSTINVERT = 5570569,
        MERGECOPY = 12583114,
        MERGEPAINT = 12255782,
        NOTSRCCOPY = 3342344,
        NOTSRCERASE = 1114278,
        PATCOPY = 15728673,
        PATINVERT = 5898313,
        PATPAINT = 16452105,
        SRCAND = 8913094,
        SRCCOPY = 13369376,
        SRCERASE = 4457256,
        SRCINVERT = 6684742,
        SRCPAINT = 15597702,
        WHITENESS = 16711778,

        DIB_RGB_COLORS = 0,
        DIB_PAL_COLORS = 1,

        LAYOUT_BITMAPORIENTATIONPRESERVED = 8,
        LAYOUT_RTL = 1,

        ABSOLUTE = 1,
        RELATIVE = 2,

        ASPECT_FILTERING = 1,
    }
}
