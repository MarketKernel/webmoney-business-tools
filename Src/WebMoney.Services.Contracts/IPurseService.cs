using System.Collections.Generic;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface IPurseService
    {
        string CreatePurse(string currency, string name);
        void AddPurse(string purse, string name);
        void RemovePurse(string purse);
        void SetMerchantKey(string purse, string key);
        void ClearMerchantKey(string purse);
        IEnumerable<IAccount> SelectAccounts(bool fresh = false, bool masterAccountsRequested = false);
    }
}