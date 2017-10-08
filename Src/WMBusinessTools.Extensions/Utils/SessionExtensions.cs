using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.Utils
{
    internal static class SessionExtensions
    {
        public static bool IsMaster(this ISession session)
        {
            return session.CurrentIdentifier == session.AuthenticationService.MasterIdentifier;
        }
    }
}