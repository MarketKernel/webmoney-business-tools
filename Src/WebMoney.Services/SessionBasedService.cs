﻿using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services
{
    public abstract class SessionBasedService
    {
        [Dependency]
        public ISession Session { get; set; }

        [Dependency]
        public IUnityContainer Container { get; set; }
    }
}
