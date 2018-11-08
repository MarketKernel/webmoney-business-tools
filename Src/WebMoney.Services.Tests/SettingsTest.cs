using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebMoney.Services.Tests
{
    [TestClass]
    public class SettingsTest
    {
        [TestMethod]
        public void CloneTest()
        {
            SettingsService settingsService = new SettingsService(1);

            var settings1 = settingsService.GetSettings();
            var settings2 = settingsService.GetSettings();

            settings1.OrderId = 1;
            settings2.OrderId = 2;

            Assert.AreEqual(1, settings1.OrderId);
            Assert.AreEqual(2, settings2.OrderId);

            settings1.TransferSettings.BalanceVisibility = true;
            settings2.TransferSettings.BalanceVisibility = false;

            Assert.AreEqual(true, settings1.TransferSettings.BalanceVisibility);
            Assert.AreEqual(false, settings2.TransferSettings.BalanceVisibility);
        }
    }
}
