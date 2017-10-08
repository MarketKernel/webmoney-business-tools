using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using WebMoney.Services.Contracts.Properties;

namespace WebMoney.Services.Contracts.Exceptions
{
    [Serializable, ComVisible(true)]
    public class WrongPasswordException : BusinessException
    {
        public override string Caption => Resources.WrongPasswordException_Caption_Wrong_password;

        public WrongPasswordException()
        {
        }

        public WrongPasswordException(string message)
            : base(message)
        {
        }

        [SecuritySafeCritical]
        public WrongPasswordException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected WrongPasswordException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
