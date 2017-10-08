using System;
using System.ComponentModel.DataAnnotations;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class ExpressPayment : IExpressPayment
    {
        public int TransferPrimaryId { get; }
        public int InvoicePrimaryId { get; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal Amount { get; }

        public DateTime TransferCreateTime { get; }
        public string SourcePurse { get; }
        public long SourceIdentifier { get; }

        public ExpressPayment(int transferPrimaryId, int invoicePrimaryId, decimal amount,
            DateTime transferCreateTime, string sourcePurse, long sourceIdentifier)
        {
            TransferPrimaryId = transferPrimaryId;
            InvoicePrimaryId = invoicePrimaryId;
            Amount = amount;
            TransferCreateTime = transferCreateTime;
            SourcePurse = sourcePurse ?? throw new ArgumentNullException(nameof(sourcePurse));
            SourceIdentifier = sourceIdentifier;
        }
    }
}
