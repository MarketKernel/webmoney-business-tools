using System.Collections.Generic;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.ExternalServices.Contracts;

namespace WebMoney.Services.Tests.FakeServices
{
    internal sealed class ExternalContractService : IExternalContractService
    {
        public int CreateContract(string name, string text, IEnumerable<long> authorizedIdentifiers)
        {
            return 1;
        }

        IEnumerable<IContractSignature> IExternalContractService.SelectContractSignatures(int contractId)
        {
            return new List<IContractSignature>
            {
               new ContractSignature(1),
                new ContractSignature(2),
            };
        }
    }
}
