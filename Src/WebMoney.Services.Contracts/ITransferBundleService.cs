using System;
using System.Collections.Generic;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface ITransferBundleService
    {
        int RegisterBundle(IEnumerable<IOriginalTransfer> transfers, string name);
        IEnumerable<ITransferBundle> SelectBundles(DateTime fromTime, DateTime toTime);
        ITransferBundle ObtainBundle(int bundleId, bool includeTransfers);
        IPreparedTransfer ObtainPreparedTransfer(int id);

        void ProcessBundleAsync(int bundleId);
        void AbortProcessingAsync(int bundleId);
        IPreparedTransfer TryObtainTransferForProcessing();
        void ProcessPreparedTransfer();
    }
}
