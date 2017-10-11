using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using log4net;
using WebMoney.Cryptography;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.DataAccess.EF;
using WebMoney.Services.Properties;
using WebMoney.Services.Utils;

namespace WebMoney.Services
{
    public sealed class EntranceService : IEntranceService
    {
        private const string DatabaseFileTemplate = "{0:000000000000}-v2";
        private const string DatabaseFileExtension = "sdf";
        private const string SuggestConnectionStringTemplate = "Provider=System.Data.SqlServerCe.4.0; Data Source={0}; Persist Security Info=False;";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(EntranceService));

        public byte[] DecryptKeeperKey(byte[] encrypted, long id, string password)
        {
            if (null == encrypted)
                throw new ArgumentNullException(nameof(encrypted));

            if (id < 0 || id > 999999999999L)
                throw new ArgumentOutOfRangeException(nameof(id));

            if (null == password)
                throw new ArgumentNullException(nameof(password));

            var idBytes =
                Encoding.Default.GetBytes(id.ToString("000000000000", CultureInfo.InvariantCulture.NumberFormat));
            var passwordBytes = Encoding.Default.GetBytes(password);

            KeeperKey keeperKey;

            try
            {
                var decryptedKey = new DecryptedKey(encrypted, idBytes, passwordBytes);
                keeperKey = new KeeperKey(decryptedKey.Modulus, decryptedKey.D);
            }
            catch (CryptographicException exception)
            {
                throw new WrongPasswordException(exception.Message, exception);
            }


            return SerializationUtility.Serialize(keeperKey);
        }

        public IEnumerable<ILightCertificate> SelectCertificates()
        {
            var x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            try
            {
                x509Store.Open(OpenFlags.MaxAllowed);

                return x509Store.Certificates.OfType<X509Certificate2>()
                    .Select(ConvertToLightCertificate)
                    .Where(c => c != null);
            }
            finally
            {
                x509Store.Close();
            }
        }

        public string SuggestConnectionString(long identifier)
        {
            if (ApplicationUtility.IsRunningOnMono)
                return null;

            string directoryPath = SettingsUtility.GetBaseDirectoryPath();
            string fileName = string.Format(CultureInfo.InvariantCulture, DatabaseFileTemplate, identifier);

            var path = Path.Combine(directoryPath, fileName);
            path = Path.ChangeExtension(path, DatabaseFileExtension);

            return string.Format(CultureInfo.InvariantCulture, SuggestConnectionStringTemplate, path);
        }

        public void Connect(string connectionString, string providerInvariantName)
        {
            if (null == connectionString)
                throw new ArgumentNullException(nameof(connectionString));

            if (null == connectionString)
                throw new ArgumentNullException(nameof(providerInvariantName));

            var connectionSettings = new ConnectionSettings(connectionString, providerInvariantName);

            using (var context = new DataContext(connectionSettings))
            {
                var testRecord = context.Records.FirstOrDefault(r => r.Id == "Test");

                if (null != testRecord)
                    return;

                context.Records.Add(new Record("Test"));
                context.SaveChanges();
            }

            using (var context = new DataContext(connectionSettings))
            {
                var testRecord = context.Records.FirstOrDefault(r => r.Id == "Test");

                if (null != testRecord)
                    return;
            }

            throw new InvalidOperationException();
        }

        public bool CheckRegistration(long identifier)
        {
            string authenticationSettingsFilePath = SettingsUtility.GetAuthenticationSettingsFilePath(identifier);
            return File.Exists(authenticationSettingsFilePath);
        }

        public void Register(IAuthenticationSettings contractObject, SecureString password)
        {
            if (null == contractObject)
                throw new ArgumentNullException(nameof(contractObject));

            if (null != password && 0 == password.Length)
                password = null;

            string authenticationSettingsFilePath = SettingsUtility.GetAuthenticationSettingsFilePath(contractObject.Identifier);

            if (File.Exists(authenticationSettingsFilePath))
                throw new DuplicateRegistrationException(
                    string.Format(CultureInfo.InvariantCulture,
                        Resources.EntranceService_Register_WMID__0_000000000000__already_registered_in_the_program_, contractObject.Identifier));

            var authenticationSettings = contractObject as AuthenticationSettings ?? AuthenticationSettings.Create(contractObject);

            if (null == authenticationSettings.RequestNumberSettings)
                authenticationSettings.RequestNumberSettings =
                    new RequestNumberSettings
                    {
                        Method = RequestNumberGenerationMethod.UnixTimestamp
                    };

            authenticationSettings.Save(authenticationSettingsFilePath, password);

            if (!ApplicationUtility.IsRunningOnMono)
            {
                try
                {
                    File.Encrypt(authenticationSettingsFilePath);
                    File.SetAttributes(authenticationSettingsFilePath, FileAttributes.Hidden | FileAttributes.Encrypted);
                }
                catch (Exception exception)
                {
                    Logger.Error(exception.Message, exception);
                }
            }
        }

        public void RemoveRegistration(long identifier)
        {
            string authenticationSettingsFilePath = SettingsUtility.GetAuthenticationSettingsFilePath(identifier);

            if (File.Exists(authenticationSettingsFilePath))
                File.Delete(authenticationSettingsFilePath);

            string settingsFilePath = SettingsUtility.GetSettingsFilePath(identifier);

            if (File.Exists(settingsFilePath))
                File.Delete(settingsFilePath);
        }

        public IEnumerable<IRegistration> SelectRegistrations()
        {
            var registrations = new List<Registration>();

            var baseDirectoryPath = SettingsUtility.GetBaseDirectoryPath();

            var files = Directory.GetFiles(baseDirectoryPath, "*.dat");

            foreach (var file in files)
            {
                var identifierValue = Path.GetFileNameWithoutExtension(file);

                long identifier;

                if (!long.TryParse(identifierValue, out identifier))
                    continue;

                var creationUtcTime = File.GetCreationTimeUtc(file);
                registrations.Add(new Registration(identifier, creationUtcTime));
            }

            return registrations;
        }

        public ISession CreateSession(long identifier, SecureString password = null)
        {
            var authenticationService = new AuthenticationService(identifier, password);
            var settingsService = new SettingsService(identifier);
            return new Session(identifier, authenticationService, settingsService);
        }

        public void Handshake(ISession session)
        {
            if (null == session)
                throw new ArgumentNullException(nameof(session));

            if (session.AuthenticationService.HasConnectionSettings)
                IdentifierService.RegisterMasterIdentifierIfNeeded(session.AuthenticationService);
        }

        private static ILightCertificate ConvertToLightCertificate(X509Certificate2 x509Certificate2)
        {
            if (!x509Certificate2.HasPrivateKey)
                return null;

            var thumbprint = x509Certificate2.Thumbprint;

            if (null == thumbprint)
                return null;

            thumbprint = thumbprint.Replace("-", string.Empty).ToLower(CultureInfo.InvariantCulture);

            var subjectCommonName = ObtainCommonName(x509Certificate2.SubjectName);
            var issuerCommonName = ObtainCommonName(x509Certificate2.IssuerName);

            if (null == subjectCommonName || null == issuerCommonName)
                return null;

            var identifier = TryObtainIdentifier(subjectCommonName);

            if (null == identifier)
                return null;

            return new LightCertificate(subjectCommonName, issuerCommonName, identifier.Value, thumbprint,
                x509Certificate2.NotBefore);
        }

        private static string ObtainCommonName(X500DistinguishedName distinguishedName)
        {
            const string prefix = "CN=";

            foreach (var value in distinguishedName.Format(true).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!value.StartsWith(prefix))
                    continue;

                return value.Remove(0, prefix.Length);
            }

            return null;
        }

        private static long? TryObtainIdentifier(string subjectCommonName)
        {
            var match = Regex.Match(subjectCommonName, @"\d{12}");

            if (!match.Success)
                return null;

            return long.Parse(match.Value);
        }
    }
}
