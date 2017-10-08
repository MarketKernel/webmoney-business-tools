namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IOriginalOutgoingInvoice
    {
        int OrderId { get; }
        long ClientIdentifier { get; }
        string StorePurse { get; }
        decimal Amount { get; }
        string Description { get; }
        string Address { get; }
        byte? ProtectionPeriod { get; }
        byte ExpirationPeriod { get; }
        bool Force { get; }
        int? ShopId { get; }
    }
}
