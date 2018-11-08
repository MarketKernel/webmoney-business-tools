using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IPaymentConfirmationInstruction
    {
        long InvoiceId { get; }
        ConfirmationType ConfirmationType { get; }
        string PublicMessage { get; }
    }
}
