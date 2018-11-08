using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using WebMoney.Services.Contracts.Properties;

namespace WebMoney.Services.Contracts.Exceptions
{
    [Serializable, ComVisible(true)]
    public class ExternalServiceException : BusinessException
    {
        public override string Caption => Resources.ExternalBusinessException_Caption_Request_rejected_by_WebMoney;

        public ExternalServiceException()
            : base(Resources.ExternalBusinessException_Caption_Request_rejected_by_WebMoney)
        {
        }

        public ExternalServiceException(string message)
            : base(message)
        {
        }

        public ExternalServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecuritySafeCritical]
        protected ExternalServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}