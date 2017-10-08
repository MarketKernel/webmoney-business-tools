using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    [Table("Trust")]
    internal class Trust : ITrust
    {
        [Key, Column(Order = 0)]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long MasterIdentifier { get; set; }

        [Key, MaxLength(13), Column(Order = 1)]
        public string Purse { get; set; }

        [Required]
        public bool InvoiceAllowed { get; set; }

        [Required]
        public bool TransferAllowed { get; set; }

        [Required]
        public bool BalanceAllowed { get; set; }

        [Required]
        public bool HistoryAllowed { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal DayLimit { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal WeekLimit { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal MonthLimit { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal DayTotalAmount { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal WeekTotalAmount { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal MonthTotalAmount { get; set; }

        [Required]
        public DateTime LastTransferTime { get; set; }

        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long? StoreIdentifier { get; set; }

        internal Trust()
        {
        }

        public Trust(long masterIdentifier, bool invoiceAllowed, bool transferAllowed, bool balanceAllowed,
            bool historyAllowed, string purse, decimal dayLimit, decimal weekLimit, decimal monthLimit,
            decimal dayTotalAmount, decimal weekTotalAmount, decimal monthTotalAmount, DateTime lastTransferTime)
        {
            MasterIdentifier = masterIdentifier;
            InvoiceAllowed = invoiceAllowed;
            TransferAllowed = transferAllowed;
            BalanceAllowed = balanceAllowed;
            HistoryAllowed = historyAllowed;
            Purse = purse ?? throw new ArgumentNullException(nameof(purse));
            DayLimit = dayLimit;
            WeekLimit = weekLimit;
            MonthLimit = monthLimit;
            DayTotalAmount = dayTotalAmount;
            WeekTotalAmount = weekTotalAmount;
            MonthTotalAmount = monthTotalAmount;
            LastTransferTime = lastTransferTime;
        }
    }
}