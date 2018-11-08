namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IOriginalTransfer
    {
        int PaymentId { get; }
        string SourcePurse { get; }
        string TargetPurse { get; }
        decimal Amount { get; }
        string Description { get; }
        byte ProtectionPeriod { get; }
        string ProtectionCode { get; }
        long? InvoiceId { get; }
        bool Force { get; }
    }
}
