using System.Collections.Generic;
using System.Security;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface IEntranceService
    {
        byte[] DecryptKeeperKey(byte[] encrypted, long id, string password);
        IEnumerable<ILightCertificate> SelectCertificates();
        string SuggestConnectionString(long identifier);
        void Connect(string connectionString, string providerInvariantName);
        bool CheckRegistration(long identifier);
        void Register(IAuthenticationSettings authenticationSettings, SecureString password);
        void RemoveRegistration(long identifier);
        IReadOnlyCollection<IRegistration> SelectRegistrations();
        ISession CreateSession(long identifier, SecureString password = null);
        void Handshake(ISession session);
    }
}
