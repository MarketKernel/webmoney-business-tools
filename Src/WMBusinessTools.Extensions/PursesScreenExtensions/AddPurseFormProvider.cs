using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using WebMoney.Services.Contracts;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.StronglyTypedWrappers;

namespace WMBusinessTools.Extensions
{
    public sealed class AddPurseFormProvider : ITopFormProvider
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

            var form = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager, ExtensionCatalog.AddPurse);

            form.ServiceCommand += (sender, args) =>
            {
                if (!AddPurseFormValuesWrapper.Control1PurseNumberCommandFindIdentifier.Equals(args.Command))
                    return;

                IdentifierDisplayHelper.ShowFindIdentifierForm(form, context, (string) args.Argument);
            };

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new AddPurseFormValuesWrapper(list);

                var purseService = context.UnityContainer.Resolve<IPurseService>();
                purseService.AddPurse(valuesWrapper.Control1PurseNumber, valuesWrapper.Control2Alias);

                return new Dictionary<string, object>();
            };

            form.FinalAction = objects =>
            {
                EventBroker.OnPurseChanged(new DataChangedEventArgs { FreshDataRequired = false });
                return true;
            };

            return form;
        }
    }
}
