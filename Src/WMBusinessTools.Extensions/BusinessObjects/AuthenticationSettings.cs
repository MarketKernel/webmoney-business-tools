using System;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class AuthenticationSettings : IAuthenticationSettings
    {
        private readonly byte[] _keeperKey;

        public AuthenticationMethod AuthenticationMethod { get; }
        public long Identifier { get; }
        public string CertificateThumbprint { get; }
        public IRequestNumberSettings RequestNumberSettings => null;
        public IConnectionSettings ConnectionSettings { get; set; }
        public IProxySettings ProxySettings => null;

        public AuthenticationSettings(long identifier, byte[] keeperKey)
        {
            Identifier = identifier;
            _keeperKey = keeperKey ?? throw new ArgumentNullException(nameof(keeperKey));
            AuthenticationMethod = AuthenticationMethod.KeeperClassic;
        }

        public AuthenticationSettings(long identifier, string certificateThumbprint)
        {
            Identifier = identifier;
            CertificateThumbprint = certificateThumbprint ??
                                    throw new ArgumentNullException(nameof(certificateThumbprint));
            AuthenticationMethod = AuthenticationMethod.KeeperLight;
        }

        public byte[] GetKeeperKey()
        {
            return _keeperKey;
        }
    }
}
