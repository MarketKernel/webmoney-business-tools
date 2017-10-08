using WMBusinessTools.Extensions.Contracts.Contexts;

namespace WMBusinessTools.Extensions.DisplayHelpers.Origins
{
    internal sealed class AccountListScreenOrigin : ListScreenOrigin
    {
        public SessionContext Context { get;}

        public AccountListScreenOrigin(SessionContext context, string extensionId)
            : base(context.ExtensionManager, extensionId)
        {
            Context = context;
        }
    }
}
