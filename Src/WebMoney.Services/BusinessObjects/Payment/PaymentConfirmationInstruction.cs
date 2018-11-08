using System;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class PaymentConfirmationInstruction : IPaymentConfirmationInstruction
    {
        public long InvoiceId { get; }
        public ConfirmationType ConfirmationType { get; }
        public string PublicMessage { get; }

        public PaymentConfirmationInstruction(long invoiceId, ConfirmationType confirmationType,
            string publicMessage)
        {
            InvoiceId = invoiceId;
            ConfirmationType = confirmationType;
            PublicMessage = publicMessage ?? throw new ArgumentNullException(nameof(publicMessage));
        }
    }
}
