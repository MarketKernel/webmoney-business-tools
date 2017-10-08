using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IAuthenticationSettings
    {
        AuthenticationMethod AuthenticationMethod { get; }
        long Identifier { get; }
        string CertificateThumbprint { get; }
        IRequestNumberSettings RequestNumberSettings { get; }
        IConnectionSettings ConnectionSettings { get; }
        IProxySettings ProxySettings { get; }

        byte[] GetKeeperKey();
    }
}
