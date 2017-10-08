using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class PreparedTransferContext : TransferBundleContext
    {
        public IPreparedTransfer PreparedTransfer { get; }

        public PreparedTransferContext(PreparedTransferContext origin)
            : base(origin)
        {
            PreparedTransfer = origin.PreparedTransfer;
        }

        public PreparedTransferContext(TransferBundleContext baseContext, IPreparedTransfer preparedTransfer)
            : base(baseContext, baseContext.TransferBundle)
        {
            PreparedTransfer = preparedTransfer ?? throw new ArgumentNullException(nameof(preparedTransfer));
        }
    }
}
