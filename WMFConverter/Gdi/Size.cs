using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Gdi
{
    /// <summary>
    /// Represents a size object.
    /// </summary>
    public class Size
    {
        #region Properties

        /// <summary>
        /// Width of the object.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of the object.
        /// </summary>
        public int Height { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        #endregion 

        #region Public Methods

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + Height;
            result = prime * result + Width;
            return result;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (typeof(Size) != obj.GetType())
                return false;
            Size other = (Size)obj;
            if (Height != other.Height)
                return false;
            if (Width != other.Width)
                return false;
            return true;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Size [width=" + Width + ", height=" + Height + "]";
        }

        #endregion
    }
}
