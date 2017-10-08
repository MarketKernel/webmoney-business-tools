namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IPaymentConfirmation
    {
        string TargetPurse { get; }
        long InvoiceId { get; }
        string ConfirmationCode { get; }
    }
}
