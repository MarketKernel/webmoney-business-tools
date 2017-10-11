using System;
using System.ComponentModel.DataAnnotations;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class IncomingInvoice : IIncomingInvoice
    {
        public long PrimaryId { get; }
        public long SecondaryId { get; }
        public int OrderId { get; }

        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long TargetIdentifier { get; }

        public string TargetPurse { get; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal Amount { get; }

        public string Description { get; }
        public string Address { get; set; }
        public byte? ProtectionPeriod { get; set; }
        public byte ExpirationPeriod { get; }
        public InvoiceState State { get; }
        public long? TransferPrimaryId { get; set; }
        public DateTime CreationTime { get; }
        public DateTime UpdateTime { get; }

        public IncomingInvoice(long primaryId, long secondaryId, int orderId, long targetIdentifier,
            string targetPurse, decimal amount, string description, byte expirationPeriod, InvoiceState state, DateTime creationTime,
            DateTime updateTime)
        {
            PrimaryId = primaryId;
            SecondaryId = secondaryId;
            OrderId = orderId;
            TargetIdentifier = targetIdentifier;
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
