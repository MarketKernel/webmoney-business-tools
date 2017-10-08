using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    [Table("Account")]
    internal sealed class Account : IAccount
    {
        [Key, MaxLength(13)]
        public string Number { get; internal set; }

        [Required, Index]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long Identifier { get; internal set; }

        [ForeignKey("Identifier")]
        public IdentifierSummary IdentifierSummary { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; internal set; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal? Amount { get; set; }

        public long? LastIncomingTransferPrimaryId { get; set; }
        public long? LastOutgoingTransferPrimaryId { get; set; }
        public bool? InvoiceAllowed { get; set; }
        public bool? TransferAllowed { get; set; }
        public bool? BalanceAllowed { get; set; }
        public bool? HistoryAllowed { get; set; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal? DayLimit { get; set; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal? WeekLimit { get; set; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal? MonthLimit { get; set; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal? DayTotalAmount { get; set; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal? WeekTotalAmount { get; set; }

        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal? MonthTotalAmount { get; set; }

        public DateTime? LastTransferTime { get; set; }
        public long? StoreIdentifier { get; set; }

        public string MerchantKey { get; set; }

        [NotMapped]
        public bool HasMerchantKey => null != MerchantKey;

        public bool IsManuallyAdded { get; set; }

        internal Account()
        {
        }

        public Account(string purse, long identifier, string name)
        {
            Number = purse ?? throw new ArgumentNullException(nameof(purse));
            Identifier = identifier;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
