using System;
using System.Collections.Generic;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface ITransferService
    {
        long CreateTransfer(IOriginalTransfer originalTransfer);
        void Moneyback(long transferPrimaryId, decimal amount);
        void FinishProtection(long transferPrimaryId, string protectionCode);
        void RejectProtection(long transferPrimaryId);
        void RedeemPaymer(string purse, string number, string key);
        IReadOnlyCollection<ITransfer> SelectTransfers(string purse, DateTime fromTime, DateTime toTime, bool fresh = false);
        decimal CalculateCommission(decimal amount, string currency);
    }
}
