using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.DisplayHelpers.Origins;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class RedeemPaymerFormProvider : IPurseFormProvider
    {
        public bool CheckCompatibility(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            var currency = currencyService.ObtainCurrencyByAccountNumber(context.Account.Number);

            if (!currencyService.CheckCapabilities(currency, CurrencyCapabilities.Actual | CurrencyCapabilities.Transfer))
                return false;

            return true;
        }

        public Form GetForm(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.RedeemPaymer);

            var origin =
                new AccountDropDownListOrigin(context.UnityContainer)
                {
                    SelectedAccountNumber = context.Account.Number
                };
            origin.FilterCriteria.CurrencyCapabilities = CurrencyCapabilities.Transfer;

            var itemTemplates = AccountDisplayHelper.BuildAccountDropDownListItemTemplates(origin);

            var redeemPaymerFormTemplateWrapper = new RedeemPaymerFormTemplateWrapper(template);

            redeemPaymerFormTemplateWrapper.Control1RedeemTo.Items.Clear();
            redeemPaymerFormTemplateWrapper.Control1RedeemTo.Items.AddRange(itemTemplates);

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            form.ApplyTemplate(template);

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new RedeemPaymerFormValuesWrapper(list);

                var transferService = context.UnityContainer.Resolve<ITransferService>();

                transferService.RedeemPaymer(valuesWrapper.Control1RedeemTo, valuesWrapper.Control2Number,
                    valuesWrapper.Control3Code);

                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}
