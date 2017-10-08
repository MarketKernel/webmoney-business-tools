using System;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class IdentifierContext : SessionContext
    {
        public string IdentifierValue { get; }

        public IdentifierContext(IdentifierContext origin)
            : base(origin)
        {
            IdentifierValue = origin.IdentifierValue;
        }

        public IdentifierContext(SessionContext baseContext, string identifierValue)
            : base(baseContext)
        {
            IdentifierValue = identifierValue ?? throw new ArgumentNullException(nameof(identifierValue));
        }
    }
}
