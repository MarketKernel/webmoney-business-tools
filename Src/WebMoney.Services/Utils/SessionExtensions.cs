using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Utils
{
    internal static class SessionExtensions
    {
        public static bool IsMaster(this ISession session)
        {
            return session.CurrentIdentifier == session.AuthenticationService.MasterIdentifier;
        }
    }
}