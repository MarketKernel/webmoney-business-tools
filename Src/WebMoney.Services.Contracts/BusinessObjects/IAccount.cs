using System;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IAccount
    {
        string Number { get; }
        string Name { get; }
        decimal? Amount { get; }
        long? LastIncomingTransferId { get; }
        long? LastOutgoingTransferId { get; }
        bool? InvoiceAllowed { get; }
        bool? TransferAllowed { get; }
        bool? BalanceAllowed { get; }
        bool? HistoryAllowed { get; }
        decimal? DayLimit { get; }
        decimal? WeekLimit { get; }
        decimal? MonthLimit { get; }
        decimal? DayTotalAmount { get; }
        decimal? WeekTotalAmount { get; }
        decimal? MonthTotalAmount { get; }
        DateTime? LastTransferTime { get; }
        long? StoreIdentifier { get; }

        string MerchantKey { get; }
        string SecretKeyX20 { get; }
        bool HasMerchantKey { get; }
        bool HasSecretKeyX20 { get; }

        bool IsManuallyAdded { get; }
    }
}
