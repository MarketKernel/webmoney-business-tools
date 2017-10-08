using System;

namespace WMBusinessTools.Extensions.Contracts
{
    public sealed class DataChangedEventArgs : EventArgs
    {
        public bool FreshDataRequired { get; set; }
    }
}
