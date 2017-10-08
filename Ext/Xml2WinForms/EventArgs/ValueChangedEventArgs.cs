using System;

namespace Xml2WinForms
{
    public sealed class ValueChangedEventArgs : EventArgs
    {
        public string ColumnName { get; }

        internal ValueChangedEventArgs(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
