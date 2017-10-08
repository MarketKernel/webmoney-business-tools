using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace WebMoney.Services.Contracts.Exceptions
{
    [Serializable, ComVisible(true)]
    public abstract class BusinessException : Exception
    {
        public abstract string Caption { get; }

        protected BusinessException()
        {
        }

        protected BusinessException(string message)
            : base(message)
        {
        }

        protected BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecuritySafeCritical]
        protected BusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
