using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class Account : IAccount
    {
        public string Number { get; }

        public string Name { get; set; }

        public decimal? Amount { get; set; }

        public long? LastIncomingTransferId => throw new NotImplementedException();

        public long? LastOutgoingTransferId => throw new NotImplementedException();

        public bool? InvoiceAllowed => throw new NotImplementedException();

        public bool? TransferAllowed => throw new NotImplementedException();

        public bool? BalanceAllowed => throw new NotImplementedException();

        public bool? HistoryAllowed => throw new NotImplementedException();

        public decimal? DayLimit => throw new NotImplementedException();

        public decimal? WeekLimit => throw new NotImplementedException();

        public decimal? MonthLimit => throw new NotImplementedException();

        public decimal? DayTotalAmount => throw new NotImplementedException();

        public decimal? WeekTotalAmount => throw new NotImplementedException();

        public decimal? MonthTotalAmount => throw new NotImplementedException();

        public DateTime? LastTransferTime => throw new NotImplementedException();

        public long? StoreIdentifier => throw new NotImplementedException();

        public string MerchantKey => throw new NotImplementedException();

        public string SecretKeyX20 => throw new NotImplementedException();

        public bool HasMerchantKey => throw new NotImplementedException();

        public bool HasSecretKeyX20 => throw new NotImplementedException();

        public bool IsManuallyAdded => throw new NotImplementedException();

        public Account(string number, string name, decimal? amount)
        {
            Number = number ?? throw new ArgumentNullException(nameof(name));
            Name = name;
            Amount = amount;
        }
    }
}