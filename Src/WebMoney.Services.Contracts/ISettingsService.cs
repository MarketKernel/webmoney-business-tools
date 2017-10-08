using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface ISettingsService
    {
        int AllocateTransferId();
        int AllocateOrderId();
        ISettings GetSettings();
        void SetSettings(ISettings settings);
        void Save();
    }
}