using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class TransferContext : PurseContext
    {
        public ITransfer Transfer { get; }

        public TransferContext(TransferContext origin)
            : base(origin)
        {
            Transfer = origin.Transfer;
        }

        public TransferContext(PurseContext baseContext, ITransfer transfer)
            : base(baseContext)
        {
            Transfer = transfer ?? throw new ArgumentNullException(nameof(transfer));
        }
    }
}
