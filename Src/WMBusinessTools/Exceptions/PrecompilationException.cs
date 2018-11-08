using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using WMBusinessTools.Properties;

namespace WMBusinessTools
{
    [Serializable, ComVisible(true)]
    public class PrecompilationException : InvalidOperationException
    {
        public int ErrorNumber { get; set; }

        public PrecompilationException()
            : base(Resources.PrecompilationException_PrecompilationException_Precompilation_error_)
        {
            ErrorNumber = -1;
        }

        public PrecompilationException(int errorNumber)
            : this(Resources.PrecompilationException_PrecompilationException_Precompilation_error_, errorNumber)
        {
            ErrorNumber = errorNumber;
        }

        public PrecompilationException(string message, int errorNumber)
            : base(message)
        {
            ErrorNumber = errorNumber;
        }

        public PrecompilationException(string message, int errorNumber, Exception innerException)
            : base(message, innerException)
        {
            ErrorNumber = errorNumber;
        }

        [SecuritySafeCritical]
        protected PrecompilationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ErrorNumber = info.GetInt32("ErrorNumber");
        }

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            base.GetObjectData(info, context);
            info.AddValue("ErrorNumber", ErrorNumber, typeof(int));
        }
    }
}
