using System;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class PaymentConfirmationReport : IPaymentConfirmationReport
    {
        public IExpressPayment Payment { get; }

        public string PublicMessage { get; }
        public SmsState SmsState { get; }

        public PaymentConfirmationReport(ExpressPayment payment, string publicMessage, SmsState smsState)
        {
            Payment = payment ?? throw new ArgumentNullException(nameof(payment));
            PublicMessage = publicMessage ?? throw new ArgumentNullException(nameof(publicMessage));
            SmsState = smsState;
        }
    }
}
