using System;
using System.Globalization;
using System.Threading;
using log4net;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;

namespace WebMoney.Services
{
    internal sealed class TransferBundleProcessor : SessionBasedService, ITransferBundleProcessor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransferBundleProcessor));

        private int _isBusy;
        private volatile bool _cancellationPending;

        public bool IsBusy => _isBusy != 0;

        public void RunAsync()
        {
            var isBusy = Interlocked.Exchange(ref _isBusy, 1);

            if (isBusy != 0)
                return;

            var currentCulture = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
            var currentUICulture = (CultureInfo) Thread.CurrentThread.CurrentUICulture.Clone();

            ThreadPool.QueueUserWorkItem(state =>
            {

                Thread.CurrentThread.CurrentCulture = currentCulture;
                Thread.CurrentThread.CurrentUICulture = currentUICulture;

                while (true)
                {
                    if (_cancellationPending)
                    {
                        Interlocked.Exchange(ref _isBusy, 0);
                        _cancellationPending = false;

                        break;
                    }

                    try
                    {
                        var transferBundleService = Container.Resolve<ITransferBundleService>();

                        var preparedTransfer = transferBundleService.TryObtainTransferForProcessing();

                        if (null == preparedTransfer)
                        {
                            _cancellationPending = true;
                            continue;
                        }

                        transferBundleService.ProcessPreparedTransfer();
                    }
                    catch (Exception exception)
                    {
                        Logger.Error(exception.Message, exception);
                    }
                }
            });
        }

        public void CancelAsync()
        {
            if (_isBusy != 1)
                return;

            _cancellationPending = true;
        }
    }
}