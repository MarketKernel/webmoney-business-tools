using System;
using System.ComponentModel.DataAnnotations;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class OutgoingInvoice : IOutgoingInvoice
    {
        public long PrimaryId { get; }
        public long SecondaryId { get; }
        public int OrderId { get; }

        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long ClientIdentifier { get; }

        public string SourcePurse { get; set; }
        public string TargetPurse { get; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal Amount { get; }

        public string Description { get; }
        public string Address { get; set; }
        public byte ExpirationPeriod { get; }
        public byte ProtectionPeriod { get; set; }
        public InvoiceState State { get; }
        public long? TransferId { get; set; }
        public DateTime CreationTime { get; }
        public DateTime UpdateTime { get; }

        public OutgoingInvoice(long primaryId, long secondaryId, int orderId, long clientIdentifier,
            string targetPurse, decimal amount, string description, byte expirationPeriod, InvoiceState state, DateTime creationTime,
            DateTime updateTime)
        {
            PrimaryId = primaryId;
            SecondaryId = secondaryId;
            OrderId = orderId;
            ClientIdentifier = clientIdentifier;
            TargetPurse = targetPurse ?? throw new ArgumentNullException(nameof(targetPurse));
            Amount = amount;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            ExpirationPeriod = expirationPeriod;
            State = state;
            CreationTime = creationTime;
            UpdateTime = updateTime;
        }
    }
}
