using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Wmf
{
    /// <summary>
    /// Windows Metafile - Represents WMF Region.
    /// </summary>
    public class WmfRegion : WmfObject,Gdi.IGdiRegion
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id"></param>
        public WmfRegion(int id)
            :base (id)
        {
        }

        #endregion
    }
}
