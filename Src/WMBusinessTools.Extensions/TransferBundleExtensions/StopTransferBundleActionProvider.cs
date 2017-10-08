using System;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;

namespace WMBusinessTools.Extensions
{
    public sealed class StopTransferBundleActionProvider : ITransferBundleActionProvider
    {
        public bool CheckCompatibility(TransferBundleContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (TransferBundleState.Pended == context.TransferBundle.State ||
                TransferBundleState.Processed == context.TransferBundle.State)
                return true;

            return false;
        }

        public void RunAction(TransferBundleContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var transferBundleService = context.UnityContainer.Resolve<ITransferBundleService>();
            transferBundleService.AbortProcessingAsync(context.TransferBundle.Id);
        }
    }
}