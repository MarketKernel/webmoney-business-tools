using System;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services
{
    internal sealed class TransferBundleNotification : ITransferBundleNotification
    {
        public string NotificationType => nameof(ITransferBundleNotification);

        public ITransferBundle TransferBundle { get; }


        public TransferBundleNotification(TransferBundle transferBundle)
        {
            TransferBundle = transferBundle ?? throw new ArgumentNullException(nameof(transferBundle));
        }
    }
}
