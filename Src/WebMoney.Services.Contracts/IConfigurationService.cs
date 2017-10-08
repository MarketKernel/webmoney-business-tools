using Microsoft.Practices.Unity;

namespace WebMoney.Services.Contracts
{
    public interface IConfigurationService
    {
        void RegisterServices(IUnityContainer unityContainer);
    }
}
