using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class OriginalTrust : IOriginalTrust
    {
        public long MasterIdentifier { get; }
        public string Purse { get; }
        public bool InvoiceAllowed { get; set; }
        public bool TransferAllowed { get; set; }
        public bool BalanceAllowed { get; set; }
        public bool HistoryAllowed { get; set; }
        public decimal DayLimit { get; set; }
        public decimal WeekLimit { get; set; }
        public decimal MonthLimit { get; set; }

        public OriginalTrust(long masterIdentifier, string purse)
        {
            MasterIdentifier = masterIdentifier;
            Purse = purse ?? throw new ArgumentNullException(nameof(purse));
        }
    }
}