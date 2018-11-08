using System;
using System.Collections.Generic;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services
{
    public sealed class EventBroker : IEventBroker
    {
        private static readonly object Anchor = new object();

        private readonly Dictionary<string, List<WeakReference>> _register;

        public EventBroker()
        {
            _register = new Dictionary<string, List<WeakReference>>();
        }

        public void Subscribe(ISubscriber subscriber, string notificationType)
        {
            if (null == subscriber)
                throw new ArgumentNullException(nameof(subscriber));

            if (null == notificationType)
                throw new ArgumentNullException(nameof(notificationType));

            lock (Anchor)
            {
                if (!_register.ContainsKey(notificationType))
                    _register.Add(notificationType, new List<WeakReference>());

                var references = _register[notificationType];
                references.Add(new WeakReference(subscriber));
            }
        }

        public void Notify(INotification notification)
        {
            if (null == notification)
                throw new ArgumentNullException(nameof(notification));

            var aliveSubscribers = new List<ISubscriber>();

            lock (Anchor)
            {
                if (!_register.TryGetValue(notification.NotificationType, out var references))
                    return;

                var deadReferences = new List<WeakReference>();

                foreach (var reference in references)
                {
                    if (!reference.IsAlive)
                    {
                        deadReferences.Add(reference);
                        continue;
                    }

                    var subscriber = reference.Target as ISubscriber;

                    if (null == subscriber)
                    {
                        deadReferences.Add(reference);
                        continue;
                    }

                    if (subscriber.IsDisposed)
                    {
                        deadReferences.Add(reference);
                        continue;
                    }

                    aliveSubscribers.Add(subscriber);
                }

                foreach (var deadReference in deadReferences)
                {
                    references.Remove(deadReference);
                }
            }

            foreach (var subscriber in aliveSubscribers)
            {
                subscriber.Notify(notification);
            }
        }
    }
}