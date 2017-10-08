using System;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class PaymentConfirmationInstruction : IPaymentConfirmationInstruction
    {
        public long PrimaryInvoiceId { get; }
        public ConfirmationType ConfirmationType { get; }
        public string PublicMessage { get; }

        public PaymentConfirmationInstruction(long primaryInvoiceId, ConfirmationType confirmationType,
            string publicMessage)
        {
            PrimaryInvoiceId = primaryInvoiceId;
            ConfirmationType = confirmationType;
            PublicMessage = publicMessage ?? throw new ArgumentNullException(nameof(publicMessage));
        }
    }
}
