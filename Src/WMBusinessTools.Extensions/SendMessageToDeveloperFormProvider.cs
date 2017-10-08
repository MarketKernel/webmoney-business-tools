using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.StronglyTypedWrappers;

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

            var submitForm = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager,
                ExtensionCatalog.SendMessageToDeveloper);

            submitForm.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new SendMessageToDeveloperFormValuesWrapper(list);

                var supportService = context.UnityContainer.Resolve<ISupportService>();

                var sender =
                    $"{valuesWrapper.Control1YourName} WMID#{context.Session.AuthenticationService.MasterIdentifier}";

                supportService.SendMessage("None", sender, valuesWrapper.Control2Message);

                return new Dictionary<string, object>();
            };

            return submitForm;
        }
    }
}
