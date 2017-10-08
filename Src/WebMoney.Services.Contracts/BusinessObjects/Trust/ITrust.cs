using System;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface ITrust
    {
        long MasterIdentifier { get; }
        bool InvoiceAllowed { get; }
        bool TransferAllowed { get; }
        bool BalanceAllowed { get; }
        bool HistoryAllowed { get; }
        string Purse { get; }
        decimal DayLimit { get; }
        decimal WeekLimit { get; }
        decimal MonthLimit { get; }
        decimal DayTotalAmount { get; }
        decimal WeekTotalAmount { get; }
        decimal MonthTotalAmount { get; }
        DateTime LastTransferTime { get; }
        long? StoreIdentifier { get; }
    }
}