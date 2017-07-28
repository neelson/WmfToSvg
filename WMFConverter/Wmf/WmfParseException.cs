using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Wmf
{
    public class WmfParseException : Exception
    {
        public WmfParseException()
            :base()
        {

        }

        public WmfParseException(String message)
            : base(message)
        {

        }

        public WmfParseException(String message, Exception t)
            : base(message,t)
        {

        }

    }
}
