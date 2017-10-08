using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using WebMoney.Services.Contracts.Properties;

namespace WebMoney.Services.Contracts.Exceptions
{
    [Serializable, ComVisible(true)]
    public class DuplicateIdentifierException : BusinessException
    {
        public override string Caption => Resources.DuplicateIdentifierException_Caption_Same_identifier_already_registered;

        public DuplicateIdentifierException()
        {
        }

        public DuplicateIdentifierException(string message)
            : base(message)
        {
        }

        public DuplicateIdentifierException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecuritySafeCritical]
        protected DuplicateIdentifierException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
