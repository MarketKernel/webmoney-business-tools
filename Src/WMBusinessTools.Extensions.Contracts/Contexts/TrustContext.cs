using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class TrustContext : SessionContext
    {
        public ITrust Trust { get; }

        public TrustContext(TrustContext origin)
            : base(origin)
        {
            Trust = origin.Trust;
        }

        public TrustContext(SessionContext baseContext, ITrust trust)
            : base(baseContext)
        {
            Trust = trust ?? throw new ArgumentNullException(nameof(trust));
        }
    }
}
