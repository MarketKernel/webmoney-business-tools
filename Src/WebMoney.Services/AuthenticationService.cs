using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WebMoney.Cryptography;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.Utils;
using WebMoney.XmlInterfaces;

namespace WebMoney.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private static readonly object Anchor = new object();

        private readonly AuthenticationSettings _authenticationSettings;

        private WebProxy _webProxy;
        private Signer _signer;
        private X509Certificate2 _certificate;

        private SecureString _password;

        internal Initializer Initializer { get; }

        public long MasterIdentifier => _authenticationSettings.Identifier;

        public AuthenticationMethod AuthenticationMethod => _authenticationSettings.AuthenticationMethod;

        public bool HasPassword => null != _password;

        public bool HasConnectionSettings => null != _authenticationSettings.ConnectionSettings;

        public AuthenticationService(long masterIdentifier, SecureString password)
        {
            string filePath = SettingsUtility.GetAuthenticationSettingsFilePath(masterIdentifier);

            if (!File.Exists(filePath))
                throw new KeyNotFoundException($"identifier={masterIdentifier}");

            _authenticationSettings = AuthenticationSettingsExtensions.Load(filePath, password);
            Initializer = new ExtendedInitializer(this);

            _password = password;
        }

        public void SetPassword(string password, string freshPassword)
        {
            lock (Anchor)
            {
                if (null != _password && _password.Length > 0)
                {
                    if (string.IsNullOrEmpty(password))
                        throw new WrongPasswordException();

                    var passwordBytes = Encoding.UTF8.GetBytes(password);

                    if (!_password.ToByteArray().SequenceEqual(passwordBytes))
                        throw new WrongPasswordException();
                }

                bool changed = false;

                if (string.IsNullOrEmpty(freshPassword))
                {
                    if (null != _password)
                    {
                        _password = null;
                        changed = true;
                    }
                }
                else
                {
                    var freshPasswordBytes = Encoding.UTF8.GetBytes(freshPassword);

                    if (null == _password || !_password.ToByteArray().SequenceEqual(freshPasswordBytes))
                    {
                        var securePassword = new SecureString();
                        securePassword.FromByteArray(freshPasswordBytes);
                        securePassword.MakeReadOnly();

                        _password = securePassword;
                        changed = true;
                    }
                }

                if (changed)
                    Save();
            }
        }

        public void SetKeeperKey(byte[] keeperKey)
        {
            if (null == keeperKey)
                throw new ArgumentNullException(nameof(keeperKey));

            if (AuthenticationMethod.KeeperClassic != _authenticationSettings.AuthenticationMethod)
                throw new InvalidOperationException(
                    "AuthenticationMethod.KeeperClassic != _authenticationSettings.AuthenticationMethod");

            lock (Anchor)
            {
                if (!keeperKey.SequenceEqual(_authenticationSettings.KeeperKey))
                {
                    _authenticationSettings.KeeperKey = keeperKey;
                    Save();

                    _signer = null;
                }
            }
        }

        public string Sign(string message)
        {
            if (null == message)
                throw new ArgumentNullException(nameof(message));

            if (AuthenticationMethod.KeeperClassic != _authenticationSettings.AuthenticationMethod)
                throw new InvalidOperationException(
                    "AuthenticationMethod.KeeperClassic != _authenticationSettings.AuthenticationMethod");

            lock (Anchor)
            {
                if (null == _signer)
                    _signer = ObtainSigner(_authenticationSettings.KeeperKey);

                return _signer.Sign(message);
            }
        }

        public X509Certificate2 GetCertificate()
        {
            if (AuthenticationMethod.KeeperLight != _authenticationSettings.AuthenticationMethod)
                throw new InvalidOperationException(
                    "AuthenticationMethod.KeeperLight != _authenticationSettings.AuthenticationMethod");

            lock (Anchor)
            {
                return _certificate ?? (_certificate =
                           ObtainCertificate(_authenticationSettings.CertificateThumbprint));
            }
        }

        public void SetCertificate(string certificateThumbprint)
        {
            if (null == certificateThumbprint)
                throw new ArgumentNullException(nameof(certificateThumbprint));

            if (AuthenticationMethod.KeeperLight != _authenticationSettings.AuthenticationMethod)
                throw new InvalidOperationException(
                    "AuthenticationMethod.KeeperLight != _authenticationSettings.AuthenticationMethod");

            lock (Anchor)
            {
                if (certificateThumbprint.Equals(_authenticationSettings.CertificateThumbprint,
                    StringComparison.OrdinalIgnoreCase))
                    return;

                _authenticationSettings.CertificateThumbprint = certificateThumbprint;

                Save();
                _certificate = null;
            }
        }

        public IRequestNumberSettings GetRequestNumberSettings()
        {
            lock (Anchor)
            {
                var requestNumberSettings = _authenticationSettings.RequestNumberSettings;
                return (IRequestNumberSettings) ((RequestNumberSettings) requestNumberSettings)?.Clone();
            }
        }

        public void SetRequestNumberSettings(IRequestNumberSettings contractObject)
        {
            if (null == contractObject)
                throw new ArgumentNullException(nameof(contractObject));

            var requestNumberSettings = RequestNumberSettings.Create(contractObject);

            lock (Anchor)
            {
                bool changed = false;

                if (!requestNumberSettings.Equals(_authenticationSettings.RequestNumberSettings))
                {
                    _authenticationSettings.RequestNumberSettings =
                        (IRequestNumberSettings) requestNumberSettings.Clone();
                    changed = true;
                }

                if (changed)
                    Save();
            }
        }

        public long GetRequestNumber(RequestNumberGenerationMethod method, long increment)
        {
            long requestNumber;

            switch (method)
            {
                case RequestNumberGenerationMethod.UnixTimestamp:
                    requestNumber = (long) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                    break;
                case RequestNumberGenerationMethod.LiteralTimestamp:
                    string timestamp =
                        DateTime.UtcNow.ToString("yyMMddHHmmssfff", CultureInfo.InvariantCulture.DateTimeFormat);
                    requestNumber = long.Parse(timestamp, NumberStyles.Integer,
                        CultureInfo.InvariantCulture.NumberFormat);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            requestNumber += increment;

            return requestNumber;
        }

        public IConnectionSettings GetConnectionSettings()
        {
            lock (Anchor)
            {
                var connectionSettings = _authenticationSettings.ConnectionSettings;
                return (IConnectionSettings) ((ConnectionSettings) connectionSettings)?.Clone();
            }
        }

        public void SetConnectionSettings(IConnectionSettings contractObject)
        {
            var connectionSettings = ConnectionSettings.Create(contractObject);

            lock (Anchor)
            {
                bool changed = false;

                if (!connectionSettings.Equals(_authenticationSettings.ConnectionSettings))
                {
                    _authenticationSettings.ConnectionSettings =
                        (IConnectionSettings) connectionSettings.Clone();
                    changed = true;
                }

                if (changed)
                    Save();
            }

            if (HasConnectionSettings)
                IdentifierService.RegisterMasterIdentifierIfNeeded(this);
        }

        public WebProxy GetProxy()
        {
            lock (Anchor)
            {
                if (null == _authenticationSettings.ProxySettings)
                    return null;

                return _webProxy ?? (_webProxy = ObtainWebProxy(_authenticationSettings.ProxySettings));
            }
        }

        public void SetProxySettings(IProxySettings contractObject)
        {
            lock (Anchor)
            {
                bool changed = false;

                if (null == contractObject)
                {
                    if (null != _authenticationSettings.ProxySettings)
                    {
                        _authenticationSettings.ProxySettings = null;
                        changed = true;
                    }
                }
                else
                {
                    var proxySettings = ProxySettings.Create(contractObject);

                    if (!proxySettings.Equals(_authenticationSettings.ProxySettings))
                    {
                        _authenticationSettings.ProxySettings = (ProxySettings) proxySettings.Clone();
                        changed = true;
                    }
                }

                if (changed)
                {
                    Save();
                    _webProxy = null;
                }
            }
        }

        private static WebProxy ObtainWebProxy(IProxySettings proxySettings)
        {
            var proxy = new WebProxy(proxySettings.Host, proxySettings.Port);

            var credential = proxySettings.Credential;

            if (null != credential)
                proxy.Credentials = new NetworkCredential(credential.Username, credential.Password);

            return proxy;
        }

        private static Signer ObtainSigner(byte[] keeperKeyBytes)
        {
            var keeperKey = (KeeperKey) SerializationUtility.Deserialize(keeperKeyBytes);

            var signer = new Signer();
            signer.Initialize(keeperKey);

            return signer;
        }

        private static X509Certificate2 ObtainCertificate(string certificateThumbprint)
        {
            var x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            try
            {
                x509Store.Open(OpenFlags.MaxAllowed);

                var certificate = x509Store.Certificates.OfType<X509Certificate2>()
                    .FirstOrDefault(c => null != c.Thumbprint &&
                                         c.Thumbprint.Replace("-", string.Empty)
                                             .Equals(certificateThumbprint, StringComparison.OrdinalIgnoreCase));

                if (null == certificate)
                    throw new KeyNotFoundException($"thumbprint={certificateThumbprint}");

                return certificate;
            }
            finally
            {
                x509Store.Close();
            }
        }

        private void Save()
        {
            string filePath = SettingsUtility.GetAuthenticationSettingsFilePath(_authenticationSettings.Identifier);
            _authenticationSettings.Save(filePath, _password);
        }
    }
}