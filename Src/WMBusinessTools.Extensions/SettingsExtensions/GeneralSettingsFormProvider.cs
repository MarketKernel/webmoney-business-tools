using System;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class GeneralSettingsFormProvider : ITopFormProvider
    {
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

            var extensionConfiguration =
                context.ExtensionManager.TryObtainExtensionConfiguration(ExtensionCatalog.GeneralSettings);

            if (null == extensionConfiguration)
                throw new InvalidOperationException("null == extensionConfiguration");

            var settingsForm = new SettingsForm(extensionConfiguration.Name.Replace("&", string.Empty))
            {
                SelectedObject = context.Session.SettingsService.GetSettings()
            };

            return settingsForm;
        }
    }
}
