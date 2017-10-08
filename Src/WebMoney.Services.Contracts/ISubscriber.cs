using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface ISubscriber : IDisposable
    {
        bool IsDisposed { get; }
        void Notify(INotification notification);
    }
}
