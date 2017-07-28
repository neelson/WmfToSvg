using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.IO
{
    /// <summary>
    /// Read input stream of wmf file.
    /// </summary>
    public class DataInput
    {
        #region Local Variables

        private System.IO.Stream _inputStream;
        private bool _isLittleEndian = BitConverter.IsLittleEndian;
	
	    private byte[] _buf = new byte[4];
	    private int _count = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the count of bytes read
        /// </summary>
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// Create a DataInput instance using native order.
        /// </summary>
        /// <param name="inputStream"></param>
	    public DataInput(System.IO.Stream inputStream) 
        {
            _isLittleEndian = BitConverter.IsLittleEndian;
		    _inputStream = inputStream;
	    }

	    /// <summary>
	    /// Default contructor.
        /// Create a DataInput instance receving the order.
	    /// </summary>
	    /// <param name="inputStream"></param>
	    /// <param name="endian"></param>
	    public DataInput(System.IO.Stream inputStream, bool endian) 
        {
		    _inputStream = inputStream;
            _isLittleEndian = endian;
	    }

        #endregion 

        #region Public Methods

        /// <summary>
        /// Reads the next one byte of this input stream as a signed 8-bit integer.
	    /// </summary>
	    /// <returns></returns>
	    public int ReadByte() 
        {
		    if (_inputStream.Read(_buf, 0, 1) == 1) 
            {
			    _count += 1;
			    return (0xff & _buf[0]);
		    }
		    throw new System.IO.EndOfStreamException();
	    }

	   /// <summary>
        /// Reads the next two bytes of this input stream as a signed 16-bit integer.
	   /// </summary>
	   /// <returns></returns>
	    public int ReadInt16()  
        {
		    if (_inputStream.Read(_buf, 0, 2) == 2) 
            {
			    short value = 0;
			    if (_isLittleEndian==false) 
                {
				    value |= (short)(0xff & _buf[1]);
				    value |= (short)((0xff & _buf[0]) << 8);
			    } 
                else 
                { 
				    value |= (short)(0xff & _buf[0]);
				    value |= (short)((0xff & _buf[1]) << 8);
			    }
			    _count += 2;
			    return value;
		    }
		    throw new System.IO.EndOfStreamException();
	    }

        /// <summary>
        /// Reads the next four bytes of this input stream as a signed 32-bit integer.
        /// </summary>
        /// <returns></returns>
	    public int ReadInt32() 
        {
		    if (_inputStream.Read(_buf, 0, 4) == 4) 
            {
			    int value = 0;
			    if (_isLittleEndian==false) 
                {
				    value |= (0xff & _buf[3]);
				    value |= (0xff & _buf[2]) << 8;
				    value |= (0xff & _buf[1]) << 16;
				    value |= (0xff & _buf[0]) << 24;
			    } 
                else 
                {
				    value |= (0xff & _buf[0]);
				    value |= (0xff & _buf[1]) << 8;
				    value |= (0xff & _buf[2]) << 16;
				    value |= (0xff & _buf[3]) << 24;
			    }
			    _count += 4;
			    return value;
		    }
		    throw new System.IO.EndOfStreamException();
	    }

	    /// <summary>
        /// Reads the next two bytes of this input stream as a unsigned 16-bit integer.
	    /// </summary>
	    /// <returns></returns>
	    public int ReadUint16() 
        {
		    if (_inputStream.Read(_buf, 0, 2) == 2) 
            {
			    int value = 0;
			    if (_isLittleEndian==false) 
                {
				    value |= (0xff & _buf[1]);
				    value |= (0xff & _buf[0]) << 8;
			    } else {
				    value |= (0xff & _buf[0]);
				    value |= (0xff & _buf[1]) << 8;
			    }
			    _count += 2;
			    return value;
		    }
		    throw new System.IO.EndOfStreamException();
	    }
	    
        /// <summary>
        /// Reads the next four bytes of this input stream as a unsigned 32-bit integer.
        /// </summary>
        /// <returns></returns>
	    public long ReadUint32()  
        {
		    if (_inputStream.Read(_buf, 0, 4) == 4) 
            {
			    long value = 0;
			    if (_isLittleEndian ==false) 
                {
				    value |= (0xff & _buf[3]);
				    value |= (0xff & _buf[2]) << 8;
				    value |= (0xff & _buf[1]) << 16;
				    value |= (0xff & _buf[0]) << 24;
			    } 
                else 
                {
				    value |= (0xff & _buf[0]);
				    value |= (0xff & _buf[1]) << 8;
				    value |= (0xff & _buf[2]) << 16;
				    value |= (0xff & _buf[3]) << 24;
			    }
			    _count += 4;
			    return value;
		    }
		    throw new System.IO.EndOfStreamException();
	    }

        /// <summary>
        /// Reads the next n bytes.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
	    public byte[] ReadBytes(int n) 
        {
		    byte[] array = new byte[n];
		    if (_inputStream.Read(array,0,array.Length) == n) 
            {
			    _count += n;
			    return array;
		    }
		    throw new System.IO.EndOfStreamException();
	    }

	
        /// <summary>
        /// Close the input stream.
        /// </summary>
	    public void Close() 
        {
		    try 
            {
			    _inputStream.Close();
		    } 
            catch(System.IO.IOException ex) 
            {
                Console.Write(ex.Message);
		    }
        }

        #endregion 
    }
}
