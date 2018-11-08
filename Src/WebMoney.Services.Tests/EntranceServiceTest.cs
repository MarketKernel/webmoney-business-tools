using System.Linq;
using System.Security;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Tests.Properties;

namespace WebMoney.Services.Tests
{
    [TestClass]
    public class EntranceServiceTest
    {
        private readonly IEntranceService _entranceService;

        public EntranceServiceTest()
        {
            var unityContainer = new UnityContainer();
            new ConfigurationService().RegisterServices(unityContainer);

            _entranceService = unityContainer.Resolve<IEntranceService>();
        }

        [TestMethod]
        public void TestMethod1()
        {
            foreach (var registration in _entranceService.SelectRegistrations())
            {
                _entranceService.RemoveRegistration(registration.Identifier);
            }

            Assert.AreEqual(0, _entranceService.SelectRegistrations().Count());

            SecureString securePassword = new SecureString();
            securePassword.AppendChar('П');
            securePassword.AppendChar('р');
            securePassword.AppendChar('о');
            securePassword.AppendChar('в');
            securePassword.AppendChar('е');
            securePassword.AppendChar('р');
            securePassword.AppendChar('к');
            securePassword.AppendChar('и');

            var decrypted = _entranceService.DecryptKeeperKey(Resources.key_227583964705_15_01_11, 227583964705L, "1");

            var authenticationSettings = new AuthenticationSettings
            {
                AuthenticationMethod = AuthenticationMethod.KeeperClassic,
                Identifier = 227583964705L,
                KeeperKey = decrypted
            };

            _entranceService.Register(authenticationSettings, securePassword);

            Assert.AreEqual(1, _entranceService.SelectRegistrations().Count());
        }
    }
}
