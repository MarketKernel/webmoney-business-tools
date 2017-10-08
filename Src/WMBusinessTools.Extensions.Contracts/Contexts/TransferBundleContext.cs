using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class TransferBundleContext : SessionContext
    {
        public ITransferBundle TransferBundle { get; }

        public TransferBundleContext(TransferBundleContext origin)
            : base(origin)
        {
            TransferBundle = origin.TransferBundle;
        }

        public TransferBundleContext(SessionContext baseContext, ITransferBundle transferBundle)
            : base(baseContext)
        {
            TransferBundle = transferBundle ?? throw new ArgumentNullException(nameof(transferBundle));
        }
    }
}
