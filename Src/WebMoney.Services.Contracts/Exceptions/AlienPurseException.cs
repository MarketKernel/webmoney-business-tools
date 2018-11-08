using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using WebMoney.Services.Contracts.Properties;

namespace WebMoney.Services.Contracts.Exceptions
{
    [Serializable, ComVisible(true)]
    public class AlienPurseException : BusinessException
    {
        public override string Caption => Resources.AlienPurseException_Caption_Purse_does_not_match_WMID;

        public AlienPurseException()
            : base(Resources.AlienPurseException_Caption_Purse_does_not_match_WMID)
        {
        }

        public AlienPurseException(string message)
            : base(message)
        {
        }

        public AlienPurseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecuritySafeCritical]
        protected AlienPurseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
