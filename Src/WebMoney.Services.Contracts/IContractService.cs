using System;
using System.Collections.Generic;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface IContractService
    {
        int CreateContract(string name, string text, IEnumerable<long> authorizedIdentifiers = null);
        void RefreshContract(int id);
        IReadOnlyCollection<IContract> SelectContracts(DateTime fromTime, DateTime toTime, bool fresh = false);
    }
}
