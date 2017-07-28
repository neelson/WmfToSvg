using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Wmf
{
    /// <summary>
    /// Windows Metafile - Represents WMF Object base
    /// </summary>
    public class WmfObject : Gdi.IGdiObject
    {
        #region Local Variables

        public int _id;

        #endregion

        #region Properties

        /// <summary>
        /// Object Id.
        /// </summary>
        public int Id
        {
            get
            {
                return _id;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id"></param>
        public WmfObject(int id)
        {
            _id = id;
        }

        #endregion
       
    }
}
