using Microsoft.Practices.Unity;
using WebMoney.Services;

namespace WMBusinessTools.Agent
{
    class Program
    {
        static void Main(string[] args)
        {
            var unityContainer = new UnityContainer();

            var configurationService = new ConfigurationService();
            configurationService.RegisterServices(unityContainer);
        }
    }
}
