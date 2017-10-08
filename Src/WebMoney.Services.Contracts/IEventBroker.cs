using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface IEventBroker
    {
        void Subscribe(ISubscriber subscriber, string notificationType);
        void Notify(INotification notification);
    }
}
