using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace ExtensibilityAssistant
{
    [Serializable, ComVisible(true)]
    public class ExtensionNotFoundException : ExtensionException
    {
        public ExtensionNotFoundException()
        {   
        }

        public ExtensionNotFoundException(string message)
            : base(message)
        {
        }

        public ExtensionNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecurityCritical]
        protected ExtensionNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
