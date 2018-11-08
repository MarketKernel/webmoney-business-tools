using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using LocalizationAssistant;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.Properties;
using WMBusinessTools.Extensions.Services;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class RegistrationFormProvider : IRegistrationFormProvider
    {
        private EntranceContext _context;

        private SubmitForm _form;

        private long _identifier;
        private byte[] _decryptedKey;
        private ILightCertificate _lightCertificate;

        public bool CheckCompatibility(EntranceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(EntranceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            var certificates = new KeySettingsService(context.UnityContainer).SelectCertificates();

            var step1IncomeValuesWrapper = new RegistrationFormValuesWrapper.Step1
            {
                Control5Certificate = certificates.Select(c => new ListItemContent(c) {ImageKey = "Pfx"}).ToList()
            };

            _form =
                SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager, ExtensionCatalog.Registration,
                    step1IncomeValuesWrapper.CollectIncomeValues());

            _form.ServiceCommand += (sender, args) =>
            {
                if (args.Command.Equals("BuildConnectionString", StringComparison.OrdinalIgnoreCase))
                    BuildConnectionString((string) args.Argument);
            };

            _form.WorkCallback = (step, list) =>
            {
                switch (step)
                {
                    case 0:
                    {
                        return Step1(new RegistrationFormValuesWrapper.Step1(list)).CollectIncomeValues();
                    }
                    case 1:
                        Step2(new RegistrationFormValuesWrapper.Step2(list));
                        return new Dictionary<string, object>();
                    default:
                        throw new InvalidOperationException("step == " + step);
                }
            };

            return _form;
        }

        private RegistrationFormValuesWrapper.Step2 Step1(RegistrationFormValuesWrapper.Step1 valuesWrapper)
        {
            var entranceService = _context.UnityContainer.Resolve<IEntranceService>();

            switch (valuesWrapper.Control1AuthenticationType)
            {
                case RegistrationFormValuesWrapper.Step1.Control1AuthenticationTypeValueClassic:
                {
                    var identifier = long.Parse(valuesWrapper.Control2Identifier);
                    var encrypted = File.ReadAllBytes(valuesWrapper.Control3BackupFile);
                    var password = valuesWrapper.Control4BackupFilePassword;

                    byte[] decryptedKey;

                    try
                    {
                        decryptedKey = entranceService.DecryptKeeperKey(encrypted, identifier, password);
                    }
                    catch (Exception exception)
                    {
                        throw new WrongPasswordException(
                            Resources.RegistrationFormProvider_Step1_Wrong_password_or_WMID_for_kwm_file_, exception);
                    }

                    _identifier = identifier;
                    _decryptedKey = decryptedKey;
                    _lightCertificate = null;
                }
                    break;
                case RegistrationFormValuesWrapper.Step1.Control1AuthenticationTypeValueLight:
                    var lightCertificate = (ILightCertificate) valuesWrapper.Control5Certificate;
                    _identifier = lightCertificate.Identifier;
                    _decryptedKey = null;

                    _lightCertificate = lightCertificate;
                    break;
                default:
                    throw new InvalidOperationException("valuesWrapper.Control1AuthenticationType == " +
                                                        valuesWrapper.Control1AuthenticationType);
            }

            if (entranceService.CheckRegistration(_identifier))
            {
                var identifierValue = _context.UnityContainer.Resolve<IFormattingService>()
                    .FormatIdentifier(_identifier);
                throw new DuplicateRegistrationException(string.Format(CultureInfo.InvariantCulture,
                    Resources.RegistrationFormProvider_Step1_WMID__0__already_registered_, identifierValue));
            }

            var connectionString = entranceService.SuggestConnectionString(_identifier) ??
                                   Translator.Instance.Translate(ExtensionCatalog.Registration, "<DB-connection is not supported>");

            var step2Wrapper =
                new RegistrationFormValuesWrapper.Step2 {Control5ConnectionString = connectionString};

            return step2Wrapper;
        }

        private void Step2(RegistrationFormValuesWrapper.Step2 valuesWrapper)
        {
            SecureString securePassword = null;

            if (!valuesWrapper.Control1LoginWithoutPassword)
            {
                securePassword = new SecureString();

                foreach (char ch in valuesWrapper.Control2Password)
                {
                    securePassword.AppendChar(ch);
                }

                securePassword.MakeReadOnly();
            }

            AuthenticationSettings authenticationSettings;

            if (null != _decryptedKey)
                authenticationSettings = new AuthenticationSettings(_identifier, _decryptedKey);
            else if (null != _lightCertificate)
                authenticationSettings =
                    new AuthenticationSettings(_lightCertificate.Identifier, _lightCertificate.Thumbprint);
            else
                throw new InvalidOperationException();

            var entranceService = _context.UnityContainer.Resolve<IEntranceService>();

            var connectionString = valuesWrapper.Control4ChangeConnectionString
                ? valuesWrapper.Control5ConnectionString
                : entranceService.SuggestConnectionString(_identifier);

            IConnectionSettings connectionSettings = null;

            if (null != connectionString)
                connectionSettings = DbSettingsService.ParseConnectionSettings(connectionString);

            authenticationSettings.ConnectionSettings = connectionSettings;

            entranceService.Register(authenticationSettings, securePassword);
        }

        private void BuildConnectionString(string connectionStringWithProvider)
        {
            var connectionSettings = DbSettingsService.ParseConnectionSettings(connectionStringWithProvider);
            var inputValuesWrapper = DbSettingsService.MapToValuesWrapper(connectionSettings);

            var dbSettingsForm = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(_context.ExtensionManager,
                ExtensionCatalog.DbSettings, inputValuesWrapper.CollectIncomeValues());

            dbSettingsForm.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new DbSettingsFormValuesWrapper(list);
                connectionSettings = DbSettingsService.ExtractConnectionSettings(valuesWrapper);

                var dbSettingsService = new DbSettingsService(_context.UnityContainer);
                dbSettingsService.CheckConnectionSettings(connectionSettings);

                connectionStringWithProvider =
                    DbSettingsService.ToConnectionStringWithProvider(connectionSettings);

                return new Dictionary<string, object>();
            };

            if (DialogResult.OK == dbSettingsForm.ShowDialog(_form))
            {
                var step2 =
                    new RegistrationFormValuesWrapper.Step2
                    {
                        Control5ConnectionString = connectionStringWithProvider
                    };

                _form.ApplyValues(step2.CollectIncomeValues());
            }
        }
    }
}