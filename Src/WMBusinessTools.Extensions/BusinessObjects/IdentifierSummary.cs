using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class IdentifierSummary : IIdentifierSummary
    {
        public long Identifier { get; }
        public string IdentifierAlias { get; }
        public bool IsMaster { get; }

        public IdentifierSummary(long identifier, string identifierAlias)
        {
            Identifier = identifier;
            IdentifierAlias = identifierAlias;
            IsMaster = false;
        }
    }
}
