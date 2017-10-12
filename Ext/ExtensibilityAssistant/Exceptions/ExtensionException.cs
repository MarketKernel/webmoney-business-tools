using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace ExtensibilityAssistant
{
    [Serializable, ComVisible(true)]
    public class ExtensionException : Exception
    {
        public ExtensionException()
        {
        }

        public ExtensionException(string message)
            : base(message)
        {
        }

        public ExtensionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecurityCritical]
        protected ExtensionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
