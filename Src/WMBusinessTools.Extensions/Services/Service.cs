using System;
using Unity;

namespace WMBusinessTools.Extensions.Services
{
    internal abstract class Service
    {
        protected IUnityContainer Container { get; }

        protected Service(IUnityContainer container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }
    }
}