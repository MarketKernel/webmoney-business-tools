using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class OriginalTransfer : IOriginalTransfer
    {
        public int PaymentId { get; }
        public string SourcePurse { get; }
        public string TargetPurse { get; }
        public decimal Amount { get; }
        public string Description { get; }
        public byte ProtectionPeriod { get; set; }
        public string ProtectionCode { get; set; }
        public long? InvoiceId { get; set; }
        public bool Force { get; set; }

        public OriginalTransfer(int paymentId, string sourcePurse, string targetPurse, decimal amount,
            string description)
        {
            PaymentId = paymentId;
            SourcePurse = sourcePurse ?? throw new ArgumentNullException(nameof(sourcePurse));
            TargetPurse = targetPurse ?? throw new ArgumentNullException(nameof(targetPurse));
            Amount = amount;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}
