using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IPaymentConfirmationReport
    {
        IExpressPayment Payment { get; }
        string PublicMessage { get; }
        SmsState SmsState { get; }
    }
}
