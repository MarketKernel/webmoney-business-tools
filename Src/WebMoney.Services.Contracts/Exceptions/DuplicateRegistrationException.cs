using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using WebMoney.Services.Contracts.Properties;

namespace WebMoney.Services.Contracts.Exceptions
{
    [Serializable, ComVisible(true)]
    public class DuplicateRegistrationException : BusinessException
    {
        public override string Caption => Resources.DuplicateRegistrationException_Caption_WMID_already_registered;

        public DuplicateRegistrationException()
        {
        }

        public DuplicateRegistrationException(string message)
            : base(message)
        {
        }

        [SecuritySafeCritical]
        public DuplicateRegistrationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DuplicateRegistrationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
