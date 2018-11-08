using System;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;

namespace WMBusinessTools.Extensions
{
    public sealed class StartTransferBundleActionProvider : ITransferBundleActionProvider
    {
        public bool CheckCompatibility(TransferBundleContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (TransferBundleState.Registered == context.TransferBundle.State)
                return true;

            return false;
        }

        public void RunAction(TransferBundleContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var transferBundleService = context.UnityContainer.Resolve<ITransferBundleService>();
            transferBundleService.ProcessBundleAsync(context.TransferBundle.Id);
        }
    }
}
