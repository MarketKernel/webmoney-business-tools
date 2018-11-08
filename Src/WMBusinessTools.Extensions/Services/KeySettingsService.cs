using System.Collections.Generic;
using System.Linq;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.Services
{
    internal sealed class KeySettingsService : Service
    {
        public KeySettingsService(IUnityContainer container) : base(container)
        {
        }

        internal ICollection<ILightCertificate> SelectCertificates()
        {
            var entranceService = Container.Resolve<IEntranceService>();
            return entranceService.SelectCertificates().ToList();

        }
    }
}
