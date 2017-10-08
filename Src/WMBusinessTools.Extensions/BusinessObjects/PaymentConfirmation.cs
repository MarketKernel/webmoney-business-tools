using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class PaymentConfirmation : IPaymentConfirmation
    {
        public string TargetPurse { get; }
        public long InvoiceId { get; }
        public string ConfirmationCode { get; set; }

        public PaymentConfirmation(string targetPurse, long invoiceId)
        {
            TargetPurse = targetPurse ?? throw new ArgumentNullException(nameof(targetPurse));
            InvoiceId = invoiceId;
        }
    }
}
