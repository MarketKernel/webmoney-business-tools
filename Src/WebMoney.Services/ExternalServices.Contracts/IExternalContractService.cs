﻿using System.Collections.Generic;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.ExternalServices.Contracts
{
    public interface IExternalContractService
    {
        int CreateContract(string name, string text, IEnumerable<long> authorizedIdentifiers);
        IEnumerable<IContractSignature> SelectContractSignatures(int contractId);
    }
}
