using Unity;

namespace WebMoney.Services.Contracts
{
    public interface IConfigurationService
    {
        string InstallationReference { get; set; }
        void RegisterServices(IUnityContainer unityContainer);
    }
}
