using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.DisplayHelpers.Origins;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class CreateTransferFormProvider : IPurseFormProvider
    {
        public bool CheckCompatibility(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (0 == context.Account.Amount)
                return false;

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            var currency = currencyService.ObtainCurrencyByAccountNumber(context.Account.Number);

            if (!currencyService.CheckCapabilities(currency,
                CurrencyCapabilities.Actual | CurrencyCapabilities.Transfer))
                return false;

            return true;
        }

        public Form GetForm(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.CreateTransfer);

            var origin =
                new AccountDropDownListOrigin(context.UnityContainer)
                {
                    SelectedAccountNumber = context.Account.Number
                };
            origin.FilterCriteria.HasMoney = true;
            origin.FilterCriteria.CurrencyCapabilities = CurrencyCapabilities.Transfer;

            var itemTemplates = AccountDisplayHelper.BuildAccountDropDownListItemTemplates(origin);

            var templateWrapper = new CreateTransferFormTemplateWrapper(template);

            templateWrapper.Control3FromPurse.Items.Clear();
            templateWrapper.Control3FromPurse.Items.AddRange(itemTemplates);

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            var incomeValuesWrapper =
                new CreateTransferFormValuesWrapper
                {
                    Control1TransferId = context.Session.SettingsService.AllocateTransferId()
                };

            form.ApplyTemplate(template, incomeValuesWrapper.CollectIncomeValues());

            form.ServiceCommand += (sender, args) =>
            {
                if (!CreateTransferFormValuesWrapper.Control2ToPurseCommandFindIdentifier.Equals(args.Command))
                    return;

                IdentifierDisplayHelper.ShowFindIdentifierForm(form, context, (string) args.Argument);
            };

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new CreateTransferFormValuesWrapper(list);
                var transferService = context.UnityContainer.Resolve<ITransferService>();

                var originalTransfer = new OriginalTransfer(valuesWrapper.Control1TransferId,
                    valuesWrapper.Control3FromPurse, valuesWrapper.Control2ToPurse, valuesWrapper.Control4Amount,
                    valuesWrapper.Control5Description);

                if (valuesWrapper.Control6UsePaymentProtection)
                {
                    originalTransfer.ProtectionPeriod = valuesWrapper.Control7ProtectionPeriod;

                    if (!valuesWrapper.Control9ProtectionByTime)
                        originalTransfer.ProtectionCode = valuesWrapper.Control8ProtectionCode;
                }

                transferService.CreateTransfer(originalTransfer);

                return new Dictionary<string, object>();
            };

            form.FinalAction = objects =>
            {
                EventBroker.OnPurseChanged(new DataChangedEventArgs {FreshDataRequired = true});
                return true;
            };

            return form;
        }
    }
}
