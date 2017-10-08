using System;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class ErrorContext
    {
        public enum ErrorLevel
        {
            Error = 0,
            Warning = 1
        }

        public string Caption { get; }
        public string Message { get; }
        public string Details { get; set; }
        public ErrorLevel Level { get; }

        public ErrorContext(string caption, string message, ErrorLevel errorLevel = ErrorLevel.Error)
        {
            Caption = caption ?? throw new ArgumentNullException(nameof(caption));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Level = errorLevel;
        }
    }
}
