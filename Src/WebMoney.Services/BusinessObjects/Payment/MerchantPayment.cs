using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class MerchantPayment : IMerchantPayment
    {
        public long SystemTransferId { get; }
        public long SystemInvoiceId { get; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal Amount { get; }

        public DateTime CreationTime { get; }
        public string Description { get; }

        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long SourceIdentifier { get; }

        public string SourcePurse { get; }
        public bool IsCapitaller { get; set; }
        public byte IsEnum { get; set; }
        public IPAddress IPAddress { get; set; }
        public string TelepatPhone { get; set; }
        public TelepatMethod? TelepatMethod { get; set; }
        public string PaymerNumber { get; set; }
        public string PaymerEmail { get; set; }
        public PaymerType? PaymerType { get; set; }
        public string CashierNumber { get; set; }
        public DateTime? CashierDate { get; set; }
        public decimal? CashierAmount { get; set; }
        public byte? InvoicingMethod { get; set; }

        public MerchantPayment(long systemTransferId, long systemInvoiceId, decimal amount, DateTime creationTime,
            string description, long sourceIdentifier, string sourcePurse)
        {
            SystemTransferId = systemTransferId;
            SystemInvoiceId = systemInvoiceId;
            Amount = amount;
            CreationTime = creationTime;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            SourceIdentifier = sourceIdentifier;
            SourcePurse = sourcePurse ?? throw new ArgumentNullException(nameof(sourcePurse));
        }
    }
}
