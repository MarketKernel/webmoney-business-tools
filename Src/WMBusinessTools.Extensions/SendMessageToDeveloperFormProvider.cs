using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Templates.Controls;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions
{
    public sealed class SendMessageToDeveloperFormProvider : ITopFormProvider
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

            var configurationService = context.UnityContainer.Resolve<IConfigurationService>();
            var installationReference = configurationService.InstallationReference;

            var template =
                TemplateLoader.LoadTemplate<SubmitFormTemplate<WMColumnTemplate>>(context.ExtensionManager,
                    ExtensionCatalog.SendMessageToDeveloper);

            var templateWrapper = new SendMessageToDeveloperFormTemplateWrapper(template);
            templateWrapper.Control2InstallationReference.DefaultValue = installationReference;

            var submitForm = SubmitFormDisplayHelper.BuildSubmitForm(context.ExtensionManager, template);

            submitForm.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new SendMessageToDeveloperFormValuesWrapper(list);

                var supportService = context.UnityContainer.Resolve<ISupportService>();

                var sender =
                    $"{valuesWrapper.Control1YourName} #{installationReference} WMID#{context.Session.AuthenticationService.MasterIdentifier}";

                supportService.SendMessage("None", sender, valuesWrapper.Control3Message);

                return new Dictionary<string, object>();
            };

            return submitForm;
        }
    }
}
