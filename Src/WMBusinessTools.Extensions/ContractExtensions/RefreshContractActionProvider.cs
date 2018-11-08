using System;
using System.Threading;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;

namespace WMBusinessTools.Extensions
{
    public sealed class RefreshContractActionProvider : IContractActionProvider
    {
        private SynchronizationContext _synchronizationContex;

        public bool CheckCompatibility(ContractContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (context.Contract.IsPublic)
                return true;

            switch (context.Contract.State)
            {
                case ContractState.Created:
                case ContractState.Signed:
                    return true;
                default:
                    return false;
            }
        }

        public void RunAction(ContractContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            _synchronizationContex = SynchronizationContext.Current;

            ThreadPool.QueueUserWorkItem(state =>
            {
                Exception exception = null;

                try
                {
                    var contractService = context.UnityContainer.Resolve<IContractService>();
                    contractService.RefreshContract(context.Contract.Id);
                }
                catch (Exception e)
                {
                    exception = e;
                }

                _synchronizationContex.Post(o =>
                {
                    if (null != exception)
                    {
                        var errorContext =
                            new ErrorContext(exception.GetType().Name, exception.Message)
                            {
                                Details = exception.ToString()
                            };

                        context.ExtensionManager.GetErrorFormProvider().GetForm(errorContext).Show();
                    }

                    EventBroker.OnContractChanged();
                }, null);
            });
        }
    }
}