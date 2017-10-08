using System;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class ExtendedIdentifier : IExtendedIdentifier
    {
        public ExtendedIdentifierType Type { get; }
        public string Value { get; }

        public ExtendedIdentifier(ExtendedIdentifierType type, string value)
        {
            Type = type;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
