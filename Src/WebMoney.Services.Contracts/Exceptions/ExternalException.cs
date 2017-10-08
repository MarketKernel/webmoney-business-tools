using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using WebMoney.Services.Contracts.Properties;

namespace WebMoney.Services.Contracts.Exceptions
{
    [Serializable, ComVisible(true)]
    public class ExternalException : BusinessException
    {
        public override string Caption => Resources.ExternalBusinessException_Caption_Request_rejected_by_WebMoney;

        public ExternalException()
        {
        }

        public ExternalException(string message)
            : base(message)
        {
        }

        public ExternalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecuritySafeCritical]
        protected ExternalException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}