using System;

namespace Xml2WinForms
{
    public sealed class CommandEventArgs : EventArgs
    {
        public string Command { get; set; }
        public object Argument { get; set; }
    }
}
