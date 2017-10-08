using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services
{
    internal sealed class PreparedTransferNotification : IPreparedTransferNotification
    {
        public string NotificationType => nameof(IPreparedTransferNotification);

        public IPreparedTransfer PreparedTransfer { get; }

        public PreparedTransferNotification(IPreparedTransfer preparedTransfer)
        {
            PreparedTransfer = preparedTransfer ?? throw new ArgumentNullException(nameof(preparedTransfer));
        }
    }
}
