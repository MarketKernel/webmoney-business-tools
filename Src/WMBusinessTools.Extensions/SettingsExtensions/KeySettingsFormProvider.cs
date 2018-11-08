using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.Services;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using Xml2WinForms;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions
{
    public sealed class KeySettingsFormProvider : ITopFormProvider
    {
        private const string ClassicKeySettingsFormTemplateName = "ClassicKeySettingsForm.json";
        private const string LightKeySettingsFormTemplateName = "LightKeySettingsForm.json";

        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var extensionConfiguration = context.ExtensionManager.TryObtainExtensionConfiguration(ExtensionCatalog.KeySettings);

            if (null == extensionConfiguration)
                throw new InvalidOperationException("null == extensionConfiguration");

            if (string.IsNullOrEmpty(extensionConfiguration.ConfigurationString))
                throw new BadTemplateException("ConfigurationString is null or empty!");

            string templateFileName;
            var authenticationMethod = context.Session.AuthenticationService.AuthenticationMethod;

            Dictionary<string, object> initValues = null;

            switch (authenticationMethod)
            {
                case AuthenticationMethod.KeeperClassic:
                    templateFileName = ClassicKeySettingsFormTemplateName;
                    break;
                case AuthenticationMethod.KeeperLight:
                {
                    templateFileName = LightKeySettingsFormTemplateName;
                    var certificates = new KeySettingsService(context.UnityContainer).SelectCertificates();
                    var valuesWrapper = new LightKeySettingsFormValuesWrapper
                    {
                        Control1Certificate = certificates
                            .Where(c => c.Identifier == context.Session.AuthenticationService.MasterIdentifier)
                            .Select(c => new ListItemContent(c) {ImageKey = "Pfx"})
                            .ToList()
                    };

                    initValues = valuesWrapper.CollectIncomeValues();
                }
                    break;
                default:
                    throw new InvalidOperationException("authenticationMethod == " + authenticationMethod);
            }

            var pathTemplate = Path.Combine(extensionConfiguration.BaseDirectory,
                extensionConfiguration.ConfigurationString);

            var templatePath = string.Format(CultureInfo.InvariantCulture, pathTemplate, templateFileName);

            var from = SubmitFormDisplayHelper.LoadSubmitFormByTemplatePath(context.ExtensionManager, templatePath, initValues);

            from.WorkCallback = (step, list) =>
            {
                if (0 != step)
                    throw new InvalidOperationException("0 != step");

                switch (authenticationMethod)
                {
                    case AuthenticationMethod.KeeperClassic:
                    {
                        var valuesWrapper = new ClassicKeySettingsFormValuesWrapper(list);

                        var entranceService = context.UnityContainer.Resolve<IEntranceService>();
                        var keeperKey =
                            entranceService.DecryptKeeperKey(
                                File.ReadAllBytes(valuesWrapper.Control1PathToKeysBackupFile),
                                context.Session.AuthenticationService.MasterIdentifier,
                                valuesWrapper.Control2PasswordToBackupFile);

                        context.Session.AuthenticationService.SetKeeperKey(keeperKey);
                    }
                        break;
                    case AuthenticationMethod.KeeperLight:
                    {
                        var valuesWrapper = new LightKeySettingsFormValuesWrapper(list);
                        var lightCertificate = (ILightCertificate) valuesWrapper.Control1Certificate;

                        context.Session.AuthenticationService.SetCertificate(lightCertificate.Thumbprint);
                    }
                        break;
                    default:
                        throw new InvalidOperationException("authenticationMethod == " + authenticationMethod);
                }

                return new Dictionary<string, object>();
            };

            return from;
        }
    }
}
