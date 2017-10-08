using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class PurseContext : SessionContext
    {
        public IAccount Account { get; }

        public PurseContext(PurseContext origin)
            : base(origin)
        {
            Account = origin.Account;
        }

        public PurseContext(SessionContext baseContext, IAccount account)
            : base(baseContext)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
        }
    }
}
