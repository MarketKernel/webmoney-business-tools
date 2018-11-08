using System;
using Unity;
using WebMoney.Services.ExternalServices.Contracts;

namespace WebMoney.Services.ExternalServices
{
    internal static class Configurator
    {
        public static void ConfigureUnityContainer(IUnityContainer unityContainer)
        {
            if (null == unityContainer)
                throw new ArgumentNullException(nameof(unityContainer));

            unityContainer.RegisterType<IExternalContractService, ExternalContractService>();
            unityContainer.RegisterType<IExternalPurseService, ExternalPurseService>();
            unityContainer.RegisterType<IExternalTransferService, ExternalTransferService>();
            unityContainer.RegisterType<IExternalTrustService, ExternalTrustService>();
        }
    }
}
