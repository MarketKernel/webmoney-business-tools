using System;
using System.Collections.Generic;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface ITransferService
    {
        long CreateTransfer(IOriginalTransfer originalTransfer);
        void Moneyback(long transferId, decimal amount);
        void FinishProtection(long transferId, string protectionCode);
        void RejectProtection(long transferId);
        void RedeemPaymer(string purse, string number, string key);
        IEnumerable<ITransfer> SelectTransfers(string purse, DateTime fromTime, DateTime toTime, bool fresh = false);
        decimal CalculateCommission(decimal amount, string currency);
    }
}
