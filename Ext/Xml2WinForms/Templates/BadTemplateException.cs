using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Xml2WinForms.Templates
{
    [Serializable, ComVisible(true)]
    public class BadTemplateException : Exception
    {
        public BadTemplateException()
        {
        }

        public BadTemplateException(string message)
            : base(message)
        {
        }

        public BadTemplateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected BadTemplateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}