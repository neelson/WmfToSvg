using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Wmf
{
    /// <summary>
    /// Windows Metafile - Represents WMF Palette object.
    /// </summary>
    public class WmfPalette: WmfObject, Gdi.IGdiPalette
    {
        #region Local Variables

        private int _version;
        private int[] _entries;

        #endregion

        #region Properties

        /// <summary>
        /// Object version.
        /// </summary>
        public int Version
        {
            get
            {
                return _version;
            }
        }

        /// <summary>
        /// Object entries.
        /// </summary>
        public int[] Entries
        {
            get
            {
                return _entries;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <param name="entries"></param>
        public WmfPalette(int id, int version, int[] entries)
            : base(id)
        {
            _version = version;
            _entries = entries;
        }

        #endregion

    }
}
