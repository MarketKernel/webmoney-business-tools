using System.Collections.Generic;
using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts
{
    public interface ICurrencyService
    {
        string ObtainCurrencyByAccountNumber(string accountNumber);
        bool CheckCapabilities(string currency, CurrencyCapabilities capabilitiesToCheck);
        string AddPrefix(string currency);
        string RemovePrefix(string currency);
        IEnumerable<string> SelectCurrencies(CurrencyCapabilities currencyCapabilities);
    }
}
