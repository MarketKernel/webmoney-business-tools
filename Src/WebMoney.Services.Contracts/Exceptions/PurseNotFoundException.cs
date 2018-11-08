using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using WebMoney.Services.Contracts.Properties;

namespace WebMoney.Services.Contracts.Exceptions
{
    [Serializable, ComVisible(true)]
    public class PurseNotFoundException : BusinessException
    {
        public override string Caption => Resources.PurseNotFoundException_Caption_Purse_not_found;

        public PurseNotFoundException()
            : base(Resources.PurseNotFoundException_Caption_Purse_not_found)
        {
        }

        public PurseNotFoundException(string message)
            : base(message)
        {
        }

        public PurseNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecuritySafeCritical]
        protected PurseNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
