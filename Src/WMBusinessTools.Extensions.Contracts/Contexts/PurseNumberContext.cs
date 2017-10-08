using System;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class PurseNumberContext : SessionContext
    {
        public string PurseNumber { get; }

        public PurseNumberContext(PurseNumberContext origin)
            : base(origin)
        {
            PurseNumber = origin.PurseNumber;
        }

        public PurseNumberContext(SessionContext baseContext, string purseNumber)
            : base(baseContext)
        {
            PurseNumber = purseNumber ?? throw new ArgumentNullException(nameof(purseNumber));
        }
    }
}
