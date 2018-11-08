using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using WebMoney.Services.Contracts.Properties;

namespace WebMoney.Services.Contracts.Exceptions
{
    [Serializable, ComVisible(true)]
    public class WrongFileFormatException : BusinessException
    {
        public override string Caption => Resources.WrongFileFormatException_Caption_Wrong_file_format;

        public WrongFileFormatException()
            : base(Resources.WrongFileFormatException_Caption_Wrong_file_format)
        {
        }

        public WrongFileFormatException(string message)
            : base(message)
        {
        }

        public WrongFileFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecuritySafeCritical]
        protected WrongFileFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}