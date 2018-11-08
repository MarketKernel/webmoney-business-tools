using System;
using System.ComponentModel.DataAnnotations;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class ExpressPayment : IExpressPayment
    {
        public long TransferId { get; }
        public long InvoiceId { get; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal Amount { get; }

        public DateTime TransferCreateTime { get; }
        public string SourcePurse { get; }
        public long SourceIdentifier { get; }

        public ExpressPayment(long transferId, long invoiceId, decimal amount,
            DateTime transferCreateTime, string sourcePurse, long sourceIdentifier)
        {
            TransferId = transferId;
            InvoiceId = invoiceId;
            Amount = amount;
            TransferCreateTime = transferCreateTime;
            SourcePurse = sourcePurse ?? throw new ArgumentNullException(nameof(sourcePurse));
            SourceIdentifier = sourceIdentifier;
        }
    }
}
