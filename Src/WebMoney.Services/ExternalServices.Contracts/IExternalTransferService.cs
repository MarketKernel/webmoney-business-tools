using System;
using System.Collections.Generic;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.ExternalServices.Contracts
{
    public interface IExternalTransferService
    {
        long CreateTransfer(IOriginalTransfer originalTransfer);
        IReadOnlyCollection<ITransfer> SelectTransfers(string purse, DateTime fromTime, DateTime toTime);
    }
}
