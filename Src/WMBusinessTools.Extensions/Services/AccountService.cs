using System.Linq;
using Unity;
using WebMoney.Services.Contracts;

namespace WMBusinessTools.Extensions.Services
{
    internal sealed class AccountService : Service
    {
        public AccountService(IUnityContainer container)
            : base(container)
        {
        }

        public bool CheckingAccountExists()
        {
            return Container.Resolve<IPurseService>().SelectAccounts().Any();
        }
    }
}
