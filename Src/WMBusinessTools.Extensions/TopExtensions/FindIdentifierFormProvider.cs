using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.Exceptions;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.Properties;
using WMBusinessTools.Extensions.StronglyTypedWrappers;

namespace WMBusinessTools.Extensions
{
    public sealed class FindIdentifierFormProvider : ITopFormProvider, IPurseNumberFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public bool CheckCompatibility(PurseNumberContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, null);
        }

        public Form GetForm(PurseNumberContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.PurseNumber);
        }

        private Form GetForm(SessionContext context, string purseNumber)
        {
            var incomeValuesWrapper = new FindIdentifierFormValuesWrapper.Step1();

            if (!string.IsNullOrEmpty(purseNumber))
                incomeValuesWrapper.Control1Purse = purseNumber;

            var form = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager,
                ExtensionCatalog.FindIdentifier, incomeValuesWrapper.CollectIncomeValues());

            form.ServiceCommand += (sender, args) =>
            {
                if (!FindIdentifierFormValuesWrapper.Step2.Control1WmIdCommandFindPassport.Equals(args.Command))
                    return;

                var identifierValue = (string)args.Argument;
                IdentifierDisplayHelper.ShowFindCertificateForm(form, context, identifierValue);
            };

            form.WorkCallback = (step, list) =>
            {
                switch (step)
                {
                    case 0:

                        var step1Wrapper = new FindIdentifierFormValuesWrapper.Step1(list);

                        var identifierService = context.UnityContainer.Resolve<IIdentifierService>();

                        var identifier = identifierService.FindIdentifier(step1Wrapper.Control1Purse);

                        if (null == identifier)
                            throw new PurseNotFoundException(string.Format(CultureInfo.InvariantCulture,
                                Resources.FindIdentifierFormProvider_GetForm_Purse__0__not_found_,
                                step1Wrapper.Control1Purse));

                        var formattingService = context.UnityContainer.Resolve<IFormattingService>();

                        var step2Wrapper = new FindIdentifierFormValuesWrapper.Step2(list)
                        {
                            Control1WmId = formattingService.FormatIdentifier(identifier.Value)
                        };
                        return step2Wrapper.CollectIncomeValues();
                    case 1:
                        return new Dictionary<string, object>();
                    default:
                        throw new InvalidOperationException("step == " + step);
                }
            };

            if (!string.IsNullOrEmpty(purseNumber))
                form.Load += (sender, args) =>
                {
                    form.Submit();
                };

            return form;
        }
    }
}
