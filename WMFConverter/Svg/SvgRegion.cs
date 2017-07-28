using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Svg
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SvgRegion : SvgObject, Gdi.IGdiRegion
    {
        #region Constructors

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="gdi"></param>
        public SvgRegion(SvgGdi gdi)
            : base(gdi)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create element - abstract method.
        /// </summary>
        /// <returns></returns>
        public abstract System.Xml.XmlElement CreateElement();

        #endregion
    }
}
