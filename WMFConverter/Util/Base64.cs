using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Util
{
    /// <summary>
    /// Base 64 encode
    /// </summary>
    public class Base64
    {
        #region Local Variables

        private static char[] ENCODE_DATA = {
		    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
		    'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
		    'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
		    'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f',
		    'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
		    'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
		    'w', 'x', 'y', 'z', '0', '1', '2', '3',
		    '4', '5', '6', '7', '8', '9', '+', '/' 
	    };

        #endregion

        #region Public Methods

        /// <summary>
        /// Encode byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
	    public static string Encode(byte[] data) 
        {
		    char[] buffer = null;

		    if (data.Length % 3 == 0) 
			    buffer = new char[data.Length / 3 * 4];
		    else 
			    buffer = new char[(data.Length / 3 + 1) * 4];
		    

		    int buf = 0;
		    for (int i = 0; i < data.Length; i++) 
            {
			    switch (i % 3) 
                {
				    case 0 :
					    buffer[i / 3 * 4] = ENCODE_DATA[(data[i] & 0xFC) >> 2];
					    buf = (data[i] & 0x03) << 4;
					    if (i + 1 == data.Length) 
                        {
						    buffer[i / 3 * 4 + 1] = ENCODE_DATA[buf];
						    buffer[i / 3 * 4 + 2] = '=';
						    buffer[i / 3 * 4 + 3] = '=';
					    }
					    break;
				    case 1 :
					    buf += (data[i] & 0xF0) >> 4;
					    buffer[i / 3 * 4 + 1] = ENCODE_DATA[buf];
					    buf = (data[i] & 0x0F) << 2;
					    if (i + 1 == data.Length) 
                        {
						    buffer[i / 3 * 4 + 2] = ENCODE_DATA[buf];
						    buffer[i / 3 * 4 + 3] = '=';
					    }
					    break;
				    case 2 :
					    buf += (data[i] & 0xC0) >> 6;
					    buffer[i / 3 * 4 + 2] = ENCODE_DATA[buf];
					    buffer[i / 3 * 4 + 3] = ENCODE_DATA[data[i] & 0x3F];
					    break;
			    }
		    }

		    return new string(buffer);
        }

        #endregion
    }
}
