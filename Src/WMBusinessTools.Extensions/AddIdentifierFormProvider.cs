using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.Exceptions;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.Properties;
using WMBusinessTools.Extensions.StronglyTypedWrappers;

namespace WMBusinessTools.Extensions
{
    public sealed class AddIdentifierFormProvider : ITopFormProvider
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

            var form = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager,
                ExtensionCatalog.AddIdentifier);

            form.ServiceCommand += (sender, args) =>
            {
                if (!AddIdentifierFormValuesWrapper.Control1WmidCommandFindPassport.Equals(args.Command))
                    return;

                var identifierValue = (string) args.Argument;
                IdentifierDisplayHelper.ShowFindCertificateForm(form, context, identifierValue);
            };

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new AddIdentifierFormValuesWrapper(list);

                var identifier = long.Parse(valuesWrapper.Control1Wmid);
                var alias = valuesWrapper.Control2Alias;

                var identifierService = context.UnityContainer.Resolve<IIdentifierService>();

                if (identifierService.IsIdentifierExists(identifier))
                    throw new DuplicateIdentifierException(
                        string.Format(CultureInfo.InvariantCulture,
                            Resources.AddIdentifierFormProvider_GetForm_WMID__0__already_registered_in_the_program,
                            valuesWrapper.Control1Wmid));

                identifierService.AddSecondaryIdentifier(new IdentifierSummary(identifier, alias));
                context.Session.CurrentIdentifier = identifier;

                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}
