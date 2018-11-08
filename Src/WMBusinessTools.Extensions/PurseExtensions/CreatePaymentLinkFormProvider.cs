using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
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
    public sealed class CreatePaymentLinkFormProvider : IPurseFormProvider
    {
        public bool CheckCompatibility(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            var currency = currencyService.ObtainCurrencyByAccountNumber(context.Account.Number);

            if (!currencyService.CheckCapabilities(currency,
                CurrencyCapabilities.Actual | CurrencyCapabilities.Transfer))
                return false;

            if (AuthenticationMethod.KeeperClassic != context.Session.AuthenticationService.AuthenticationMethod &&
                !context.Account.HasMerchantKey)
                return false;

            return true;
        }

        public Form GetForm(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.CreatePaymentLink);

            var step1TemplateWrapper = new CreatePaymentLinkFormTemplateWrapper.Step1(template);

            var origin =
                new AccountDropDownListOrigin(context.UnityContainer)
                {
                    SelectedAccountNumber = context.Account.Number
                };
            origin.FilterCriteria.CurrencyCapabilities = CurrencyCapabilities.Transfer;

            var itemTemplates = AccountDisplayHelper.BuildAccountDropDownListItemTemplates(origin);

            step1TemplateWrapper.Control1StorePurse.Items.Clear();
            step1TemplateWrapper.Control1StorePurse.Items.AddRange(itemTemplates);

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            var inputStep1ValuesWrapper =
                new CreatePaymentLinkFormValuesWrapper.Step1
                {
                    Control2OrderId = context.Session.SettingsService.AllocateOrderId()
                };

            form.ApplyTemplate(template, inputStep1ValuesWrapper.CollectIncomeValues());

            form.WorkCallback = (step, list) =>
            {
                switch (step)
                {
                    case 0:
                        var step1ValuesWrapper = new CreatePaymentLinkFormValuesWrapper.Step1(list);

                        var paymentLinkRequest = new PaymentLinkRequest(step1ValuesWrapper.Control2OrderId,
                            step1ValuesWrapper.Control1StorePurse, step1ValuesWrapper.Control3PaymentAmount,
                            step1ValuesWrapper.Control5Description)
                        {
                            Lifetime = step1ValuesWrapper.Control4ValidityPeriod
                        };


                        var paymentService = context.UnityContainer.Resolve<IPaymentService>();
                        var link = paymentService.CreatePaymentLink(paymentLinkRequest);

                        var step2ValuesWrapper = new CreatePaymentLinkFormValuesWrapper.Step2
                        {
                            Control1PaymentLink = link
                        };

                        return step2ValuesWrapper.CollectIncomeValues();
                    case 1:
                        return new Dictionary<string, object>();
                    default:
                        throw new InvalidOperationException("step == " + step);
                }
            };

            return form;
        }
    }
}
