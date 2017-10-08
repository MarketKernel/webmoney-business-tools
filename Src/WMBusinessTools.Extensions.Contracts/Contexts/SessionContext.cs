using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class SessionContext : EntranceContext
    {
        public ISession Session { get; }

        public SessionContext(SessionContext origin)
            : base(origin)
        {
            Session = origin.Session;
        }

        public SessionContext(EntranceContext baseContext, ISession session)
            : base(baseContext)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
        }
    }
}