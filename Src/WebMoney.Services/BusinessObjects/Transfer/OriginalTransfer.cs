using System;
using System.ComponentModel.DataAnnotations;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    public sealed class OriginalTransfer : IOriginalTransfer
    {
        public int TransferId { get; }
        public string SourcePurse { get; }
        public string TargetPurse { get; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal Amount { get; }

        public string Description { get; }
        public byte ProtectionPeriod { get; set; }
        public string ProtectionCode { get; set; }
        public long? InvoiceId { get; set; }
        public bool Force { get; set; }

        public OriginalTransfer(int transferId, string sourcePurse, string targetPurse, decimal amount,
            string description)
        {
            TransferId = transferId;
            SourcePurse = sourcePurse ?? throw new ArgumentNullException(nameof(sourcePurse));
            TargetPurse = targetPurse ?? throw new ArgumentNullException(nameof(targetPurse));
            Amount = amount;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}
