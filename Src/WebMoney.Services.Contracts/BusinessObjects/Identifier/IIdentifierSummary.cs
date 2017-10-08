namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IIdentifierSummary
    {
        long Identifier { get; }
        string IdentifierAlias { get; }
        bool IsMaster { get; }
    }
}
