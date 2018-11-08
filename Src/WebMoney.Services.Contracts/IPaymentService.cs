using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface IPaymentService
    {
        IPaymentConfirmationInstruction RequestPayment(IOriginalExpressPayment originalExpressPayment);
        IExpressPayment ConfirmPayment(IPaymentConfirmation confirmation);
        string CreatePaymentLink(IPaymentLinkRequest paymentLinkRequest);
        IMerchantPayment FindPayment(string purse, long number, PaymentNumberKind numberKind);
    }
}
