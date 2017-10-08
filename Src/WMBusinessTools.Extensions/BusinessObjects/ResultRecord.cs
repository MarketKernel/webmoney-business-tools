using System;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class ResultRecord
    {
        public string Name { get; }
        public string Value { get; }

        public ResultRecord(string name, string value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
