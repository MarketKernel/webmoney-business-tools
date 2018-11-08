using WebMoney.Services.Contracts;
using WebMoney.XmlInterfaces;

namespace WebMoney.Services.Utils
{
    internal static class AuthenticationServiceExtensions
    {
        public static Initializer ObtainInitializer(this IAuthenticationService authenticationService)
        {
            return ((AuthenticationService) authenticationService).Initializer;
        }
    }
}
