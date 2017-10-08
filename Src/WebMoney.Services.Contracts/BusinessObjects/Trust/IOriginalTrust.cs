namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IOriginalTrust
    {
        long MasterIdentifier { get; }
        string Purse { get; }
        bool InvoiceAllowed { get; }
        bool TransferAllowed { get; }
        bool BalanceAllowed { get; }
        bool HistoryAllowed { get; }
        decimal DayLimit { get; }
        decimal WeekLimit { get; }
        decimal MonthLimit { get; }
    }
}
