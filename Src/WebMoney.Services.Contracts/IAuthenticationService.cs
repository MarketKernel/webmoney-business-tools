using System.Net;
using System.Security.Cryptography.X509Certificates;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface IAuthenticationService
    {
        long MasterIdentifier { get; }

        AuthenticationMethod AuthenticationMethod { get; }

        bool HasPassword { get; }

        bool HasConnectionSettings { get; }

        // Password
        void SetPassword(string password, string freshPassword);

        // Keeper
        void SetKeeperKey(byte[] keeperKey);

        string Sign(string message);

        // Certificate
        X509Certificate2 GetCertificate();

        void SetCertificate(string certificateThumbprint);

        //RequestNumber
        IRequestNumberSettings GetRequestNumberSettings();

        void SetRequestNumberSettings(IRequestNumberSettings requestNumberSettings);
        long GetRequestNumber(RequestNumberGenerationMethod method, long increment);

        // Connection
        IConnectionSettings GetConnectionSettings();
        void SetConnectionSettings(IConnectionSettings connectionSettings);

        // Proxy
        WebProxy GetProxy();

        IProxySettings GeProxySettings();
        void SetProxySettings(IProxySettings proxySettings);
    }
}
