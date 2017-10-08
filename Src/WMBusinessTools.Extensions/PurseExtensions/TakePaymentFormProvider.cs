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
    public sealed class TakePaymentFormProvider : IPurseFormProvider
    {
        public bool CheckCompatibility(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            var currency = currencyService.ObtainCurrencyByAccountNumber(context.Account.Number);

            if (!currencyService.CheckCapabilities(currency, CurrencyCapabilities.Transfer))
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
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.TakePayment);

            var origin =
                new AccountDropDownListOrigin(context.UnityContainer)
                {
                    SelectedAccountNumber = context.Account.Number
                };
            origin.FilterCriteria.CurrencyCapabilities = CurrencyCapabilities.Transfer;

            var itemTemplates = AccountDisplayHelper.BuildAccountDropDownListItemTemplates(origin);

            var step1TemplateWrapper = new TakePaymentFormTemplateWrapper.Step1(template);

            step1TemplateWrapper.Control2StorePurse.Items.Clear();
            step1TemplateWrapper.Control2StorePurse.Items.AddRange(itemTemplates);

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            var incomeValuesWrapper =
                new TakePaymentFormValuesWrapper.Step1
                {
                    Control2OrderId = context.Session.SettingsService.AllocateOrderId()
                };

            form.ApplyTemplate(template, incomeValuesWrapper.CollectIncomeValues());

            form.ServiceCommand += (sender, args) =>
            {
                if (!TakePaymentFormValuesWrapper.Step1.Control5WMIDCommandFindPassport.Equals(args.Command))
                    return;

                var identifierValue = (string) args.Argument;
                IdentifierDisplayHelper.ShowFindCertificateForm(form, context, identifierValue);
            };

            long invoiceId = 0;
            string purse = "";

            form.WorkCallback = (step, list) =>
            {
                var paymentService = context.UnityContainer.Resolve<IPaymentService>();

                switch (step)
                {
                    case 0:
                        var step1ValuesWrapper = new TakePaymentFormValuesWrapper.Step1(list);

                        ExtendedIdentifier extendedIdentifier;

                        switch (step1ValuesWrapper.Control3IdentifierType)
                        {
                            case TakePaymentFormValuesWrapper.Step1.Control3IdentifierTypeValueWmid:
                                extendedIdentifier = new ExtendedIdentifier(ExtendedIdentifierType.WmId,
                                    step1ValuesWrapper.Control5WMID);
                                break;
                            case TakePaymentFormValuesWrapper.Step1.Control3IdentifierTypeValuePhone:
                                extendedIdentifier = new ExtendedIdentifier(ExtendedIdentifierType.Phone,
                                    step1ValuesWrapper.Control4Phone);
                                break;
                            case TakePaymentFormValuesWrapper.Step1.Control3IdentifierTypeValueEmail:
                                extendedIdentifier = new ExtendedIdentifier(ExtendedIdentifierType.Email,
                                    step1ValuesWrapper.Control6Email);
                                break;
                            default:
                                throw new InvalidOperationException(
                                    "step1ValuesWrapper.Control3IdentifierType == " +
                                    step1ValuesWrapper.Control3IdentifierType);
                        }

                        var originalExpressPayment = new OriginalExpressPayment(step1ValuesWrapper.Control2OrderId,
                            step1ValuesWrapper.Control1StorePurse, step1ValuesWrapper.Control7PaymentAmount,
                            step1ValuesWrapper.Control9Description, extendedIdentifier);

                        originalExpressPayment.ConfirmationType =
                            (ConfirmationType) Enum.Parse(typeof(ConfirmationType),
                                step1ValuesWrapper.Control8ConfirmationType);

                        purse = originalExpressPayment.TargetPurse;

                        var confirmationInstruction = paymentService.RequestPayment(originalExpressPayment);

                        invoiceId = confirmationInstruction.PrimaryInvoiceId;

                        var step2IncomeValuesWrapper = new TakePaymentFormValuesWrapper.Step2();
                        step2IncomeValuesWrapper.Control1InvoiceId = confirmationInstruction.PrimaryInvoiceId.ToString();
                        step2IncomeValuesWrapper.Control2Message = confirmationInstruction.PublicMessage ?? string.Empty;

                        return step2IncomeValuesWrapper.CollectIncomeValues();
                    case 1:

                        var step2ValuesWrapper = new TakePaymentFormValuesWrapper.Step2(list);

                        var paymentConfirmation = new PaymentConfirmation(purse, invoiceId)
                        {
                            ConfirmationCode = step2ValuesWrapper.Control4CancelInvoice
                                ? "-1"
                                : step2ValuesWrapper.Control3Code
                        };

                        paymentService.ConfirmPayment(paymentConfirmation);

                        break;
                    default:
                        throw new InvalidOperationException("step == " + step);
                }

                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}