using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.DataAccess.EF;
using WebMoney.Services.ExternalServices.Contracts;
using WebMoney.Services.Tests.FakeServices;

namespace WebMoney.Services.Tests
{
    [TestClass]
    public class ContractServiceTest
    {
        private readonly UnityContainer _unityContainer;

        public ContractServiceTest()
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IContractService, ContractService>();
            unityContainer.RegisterType<IExternalContractService, ExternalContractService>();

            ISession session = new Session(1, new FakeServices.AuthenticationService(), new SettingsService(1));
            unityContainer.RegisterInstance(session);

            _unityContainer = unityContainer;

            //DbConfiguration.SetConfiguration(new DataConfiguration());

            //using (var wmContext = new DataContext(session.AuthenticationService.GetConnectionSettings()))
            //{
            //    wmContext.Database.Delete();
            //}
        }

        [TestMethod]
        public void CreateContractTest()
        {
            var contractService = _unityContainer.Resolve<IContractService>();

            var contracts = contractService.SelectContracts(DateTime.Now.AddYears(-1), DateTime.UtcNow).ToList();

            Assert.AreEqual(0, contracts.Count);
            contractService.CreateContract("Test contract", "Текст контракта");

            contracts = contractService.SelectContracts(DateTime.Now.AddYears(-1), DateTime.UtcNow).ToList();
            var contract = contracts[0];

            Assert.AreEqual(1, contracts.Count);

            Assert.AreEqual(0, contract.Signatures.Count());

            contractService.RefreshContract(1);

            contracts = contractService.SelectContracts(DateTime.Now.AddYears(-1), DateTime.UtcNow).ToList();
            contract = contracts[0];

            Assert.AreEqual(2, contract.Signatures.Count());
        }
    }
}
