using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Gdi
{
    /// <summary>
    /// Represents a point (x,y).
    /// </summary>
    public class Point
    {
        #region Properties

        /// <summary>
        /// Point X
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Point Y
        /// </summary>
        public int Y { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
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
            result = prime * result + X;
            result = prime * result + Y;
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
            if (typeof(Point) != obj.GetType())
                return false;
            Point other = (Point)obj;
            if (X != other.X)
                return false;
            if (Y != other.Y)
                return false;
            return true;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Point [x=" + X + ", y=" + Y + "]";
        }

        #endregion
    }
}
