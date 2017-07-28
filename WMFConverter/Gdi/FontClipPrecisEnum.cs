using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Gdi
{
    /// <summary>
    /// Clip Precis Enum
    /// </summary>
    public enum FontClipPrecisEnum
    {
        CLIP_DEFAULT_PRECIS = 0,
        CLIP_CHARACTER_PRECIS = 1,
        CLIP_STROKE_PRECIS = 2,
        CLIP_MASK = 15,
        CLIP_LH_ANGLES = 16,
        CLIP_TT_ALWAYS = 32,
        CLIP_EMBEDDED = 128,
    }
}
