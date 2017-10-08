using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace ExtensibilityAssistant
{
    [Serializable, ComVisible(true)]
    public class WrongExtensionTypeException : ExtensionException
    {
        public WrongExtensionTypeException()
        {
        }

        public WrongExtensionTypeException(string message)
            : base(message)
        {
        }

        public WrongExtensionTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecurityCritical]
        protected WrongExtensionTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
