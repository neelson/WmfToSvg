using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Wmf
{
    /// <summary>
    /// Windows Metafile - Represents WMF Font object.
    /// </summary>
    public class WmfFont : WmfObject, Gdi.IGdiFont
    {
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

        #endregion

        #region Properties

        /// <summary>
        /// Object Height.
        /// </summary>
        public int Height
        {
            get
            {
                return _height;
            }
        }

        /// <summary>
        /// Object Width.
        /// </summary>
        public int Width
        {
            get
            {
                return _width;
            }
        }

        /// <summary>
        /// Object Escapement.
        /// </summary>
        public int Escapement
        {
            get
            {
                return _escapement;
            }
        }

        /// <summary>
        /// Object Orientation.
        /// </summary>
        public int Orientation
        {
            get
            {
                return _orientation;
            }
        }

        /// <summary>
        /// Object Weight.
        /// </summary>
        public int Weight
        {
            get
            {
                return _weight;
            }
        }

        /// <summary>
        /// Defines whether the font is italic.
        /// </summary>
        public bool IsItalic
        {
            get
            {
                return _italic;
            }
        }

        /// <summary>
        /// Defines whether the font is underlined.
        /// </summary>
        public bool IsUnderlined
        {
            get
            {
                return _underline;
            }
        }

        /// <summary>
        /// Defines whether the font is striked.
        /// </summary>
        public bool IsStrikedOut
        {
            get
            {
                return _strikeout;
            }
        }

        /// <summary>
        /// Defines the font charset.
        /// </summary>
        public int Charset
        {
            get
            {
                return _charset;
            }
        }

        /// <summary>
        /// Defines the font out precision.
        /// </summary>
        public int OutPrecision
        {
            get
            {
                return _outPrecision;
            }
        }

        /// <summary>
        /// Defines the clip precision.
        /// </summary>
        public int ClipPrecision
        {
            get
            {
                return _clipPrecision;
            }
        }

        /// <summary>
        /// Object quality.
        /// </summary>
        public int Quality
        {
            get
            {
                return _quality;
            }
        }

        /// <summary>
        /// Defines pitch and famility font.
        /// </summary>
        public int PitchAndFamily
        {
            get
            {
                return _pitchAndFamily;
            }
        }

        /// <summary>
        /// Defines face name rules.
        /// </summary>
        public string FaceName
        {
            get
            {
                return _faceName;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="id"></param>
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
        public WmfFont(int id,
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
            : base (id)
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
        }

        #endregion

       
    }
}
