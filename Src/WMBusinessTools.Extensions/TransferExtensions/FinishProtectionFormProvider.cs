using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class FinishProtectionFormProvider : ITransferFormProvider
    {
        public bool CheckCompatibility(TransferContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (TransferType.Protected != context.Transfer.Type)
                return false;

            if (context.Transfer.SourcePurse.Equals(context.Account.Number))
                return false;

            return true;
        }

        public Form GetForm(TransferContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.FinishProtection);

            var templateWrapper = new FinishProtectionFormTemplateWrapper(template);

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            var currency = currencyService.ObtainCurrencyByAccountNumber(context.Transfer.TargetPurse);

            templateWrapper.Control4Amount.CurrencyName = currencyService.AddPrefix(currency);

            var transfer = context.Transfer;

            var incomeValuesWrapper = new FinishProtectionFormValuesWrapper
            {
                Control1SourcePurse = transfer.SourcePurse,
                Control2TargetPurse = transfer.TargetPurse,
                Control3Amount = transfer.Amount,
                Control4Description = transfer.Description
            };

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            form.ApplyTemplate(template, incomeValuesWrapper.CollectIncomeValues());

            form.ServiceCommand += (sender, args) =>
            {
                if (!FinishProtectionFormValuesWrapper.Control1SourcePurseCommandFindIdentifier.Equals(args.Command))
                    return;

                var purseNumber = (string)args.Argument;
                IdentifierDisplayHelper.ShowFindIdentifierForm(form, context, purseNumber);
            };

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new FinishProtectionFormValuesWrapper(list);

                var transferService = context.UnityContainer.Resolve<ITransferService>();
                transferService.FinishProtection(transfer.PrimaryId,
                    valuesWrapper.Control6HoldingFeatureIsUsed ? string.Empty : valuesWrapper.Control5Code);

                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}
