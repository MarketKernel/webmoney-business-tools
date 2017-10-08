using System;

namespace Xml2WinForms
{
    public sealed class InspectionReport
    {
        public string Message { get; }

        public bool Handled { get; }

        public InspectionReport(string message, bool handled = false)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Handled = handled;
        }
    }
}