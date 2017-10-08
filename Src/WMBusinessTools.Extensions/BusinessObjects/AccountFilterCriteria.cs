using WebMoney.Services.Contracts.BasicTypes;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class AccountFilterCriteria
    {
        public string Currency { get; set; }
        public bool HasMoney { get; set; }
        public CurrencyCapabilities CurrencyCapabilities { get; set; }
    }
}