using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Tests
{
    [TestClass]
    public class TempTest
    {
        private class OriginalOutgoingInvoice : IOriginalOutgoingInvoice
        {
            public int OrderId { get; set; }
            public long ClientIdentifier { get; set; }
            public string StorePurse { get; set; }
            public decimal Amount { get; set; }
            public string Description { get; set; }
            public string Address { get; set; }
            public byte? ProtectionPeriod { get; set; }
            public byte ExpirationPeriod { get; set; }
            public bool Force { get; set; }
            public int? ShopId { get; set; }
        }

        [TestMethod]
        public void Test()
        {
            var unityContainer = new UnityContainer();

            var configurationService = new ConfigurationService();
            configurationService.RegisterServices(unityContainer);

            var entranceService = unityContainer.Resolve<IEntranceService>();

            var session = entranceService.CreateSession(301095414760L);
            unityContainer.RegisterInstance(session);

            //var invoiceService = unityContainer.Resolve<IInvoiceService>();

            //for (int i = 0; i < 100; i++)
            //{
            //    invoiceService.CreateOutgoingInvoice(new OriginalOutgoingInvoice()
            //    {
            //        ClientIdentifier = 301095414760L,
            //        OrderId = i,
            //        StorePurse = "R870745686562",
            //        Amount = 122222M + (i/100M),
            //        Description = "Test WM-API #" + i
            //    });
            //}
            var transferService = unityContainer.Resolve<ITransferService>();

            transferService.CreateTransfer(new OriginalTransfer(1, "U313185739663", "U391803842412", 0.01M, "test WM-API"));

            //for (int i = 1; i < 105; i++)
            //{
            //    transferService.CreateTransfer(new OriginalTransfer(400 + i, "U313185739663", "U391803842412", i / 100M,
            //        "test WM-API" + i));
            //}
        }
    }
}
