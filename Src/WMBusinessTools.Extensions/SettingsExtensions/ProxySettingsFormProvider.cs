using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.StronglyTypedWrappers;

namespace WMBusinessTools.Extensions
{
    public sealed class ProxySettingsFormProvider : ITopFormProvider
    {
        private const string PasswordMask = "{22DB6C13-9E8B-41C8-8D7E-C96CAEEF295D}";

        private const string SharedProxyAddress = "proxy.webmoney-business-tools.com";
        private const int SharedProxyPort = 33117;

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

            var incomeValuesWrapper = new ProxySettingsFormValuesWrapper();

            var proxySettings = context.Session.AuthenticationService.GeProxySettings();

            string password = null;

            if (null != proxySettings)
            {
                var address = proxySettings.Host;
                var port = proxySettings.Port;

                if (SharedProxyAddress.Equals(address, StringComparison.OrdinalIgnoreCase) && SharedProxyPort == port &&
                    null == proxySettings.Credential)
                    incomeValuesWrapper.Control1Mode = ProxySettingsFormValuesWrapper.Control1ModeValueShared;
                else
                {
                    incomeValuesWrapper.Control1Mode = ProxySettingsFormValuesWrapper.Control1ModeValueCustom;

                    incomeValuesWrapper.Control2Address = address;
                    incomeValuesWrapper.Control3Port = port;

                    if (null != proxySettings.Credential)
                    {
                        incomeValuesWrapper.Control4AuthenticationRequired = true;

                        incomeValuesWrapper.Control5Username = proxySettings.Credential.Username;
                        incomeValuesWrapper.Control6Password = PasswordMask;

                        password = proxySettings.Credential.Password;
                    }
                }
            }
            else
                incomeValuesWrapper.Control1Mode = ProxySettingsFormValuesWrapper.Control1ModeValueNone;

            var form = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager,
                ExtensionCatalog.ProxySettings, incomeValuesWrapper.CollectIncomeValues());

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new ProxySettingsFormValuesWrapper(list);

                switch (valuesWrapper.Control1Mode)
                {
                    case ProxySettingsFormValuesWrapper.Control1ModeValueNone:
                        context.Session.AuthenticationService.SetProxySettings(null);
                        break;
                    case ProxySettingsFormValuesWrapper.Control1ModeValueShared:
                        context.Session.AuthenticationService.SetProxySettings(new ProxySettings(SharedProxyAddress,
                            SharedProxyPort));
                        break;
                    case ProxySettingsFormValuesWrapper.Control1ModeValueCustom:

                        proxySettings = new ProxySettings(valuesWrapper.Control2Address, valuesWrapper.Control3Port);

                        if (valuesWrapper.Control4AuthenticationRequired)
                        {
                            var newPassword =
                                PasswordMask.Equals(valuesWrapper.Control6Password, StringComparison.Ordinal)
                                    ? password
                                    : valuesWrapper.Control6Password;

                            proxySettings.Credential = new ProxyCredential(valuesWrapper.Control5Username, newPassword);
                        }

                        context.Session.AuthenticationService.SetProxySettings(proxySettings);

                        break;
                }

                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}