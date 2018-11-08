using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PartialTrustInstaller.Utils
{
    internal static class WebClientExtensions
    {
        public static async Task DownloadFileAsyncTask(this WebClient webClient, Uri address, string fileName)
        {
            var synchronizationContext = SynchronizationContext.Current;

            bool cancelled = false;
            Exception exception = null;

            var autoResetEvent = new AutoResetEvent(false);

            webClient.DownloadFileCompleted += (o, args) =>
            {
                if (args.Cancelled)
                    cancelled = true;

                if (null != args.Error)
                    exception = args.Error;

                autoResetEvent.Set();
            };

            await Task.Factory.StartNew(() =>
            {
                synchronizationContext.Post(
                    state => { webClient.DownloadFileAsync(address, fileName); },
                    null);

                autoResetEvent.WaitOne();

                if (cancelled)
                    throw new TaskCanceledException();

                if (null != exception)
                    throw exception;
            });
        }
    }
}
