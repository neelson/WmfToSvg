using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Wmf
{
    /// <summary>
    /// Windows Metafile - Represents WMF Pattern Brush object.
    /// </summary>
    public class WmfPatternBrush : WmfObject, Gdi.IGdiPatternBrush
    {
        #region Local Variables

        private byte[] _image;

        #endregion 

        #region Properties

        /// <summary>
        /// Object pattern.
        /// </summary>
        public byte[] Pattern
        {
            get
            {
                return _image;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="image"></param>
        public WmfPatternBrush(int id, byte[] image)
            : base (id)
        {
            _image = image;
        }

        #endregion 

       
    }
}
