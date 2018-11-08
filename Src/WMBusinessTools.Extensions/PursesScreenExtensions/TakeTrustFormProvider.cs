using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.DisplayHelpers.Origins;
using WMBusinessTools.Extensions.Properties;
using WMBusinessTools.Extensions.Services;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class TakeTrustFormProvider : ITopFormProvider, IPurseFormProvider
    {
        public const string CheckSmsStateUrlTemplate = "https://sms.webmoney.ru/SmsStatus/Sms/ru-RU/MSK/HtmlPage/{0}";

        public bool CheckCompatibility(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            var currency = currencyService.ObtainCurrencyByAccountNumber(context.Account.Number);

            if (!currencyService.CheckCapabilities(currency,
                CurrencyCapabilities.Actual | CurrencyCapabilities.Transfer))
                return false;

            return true;
        }

        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (context.Session.IsMaster())
                return false;

            return true;
        }

        public Form GetForm(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (!new AccountService(context.UnityContainer).CheckingAccountExists())
                return ErrorFormDisplayHelper.BuildErrorForm(context.ExtensionManager,
                    Resources.CreateTrustFormProvider_GetForm_Purses_list_is_empty,
                    Resources.CreateTrustFormProvider_GetForm_Please_refresh_the_list_of_purses_or_add_it_manually);

            return GetForm(context, null);
        }

        public Form GetForm(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.Account.Number);
        }

        private Form GetForm(SessionContext context, string accountNumber)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.TakeTrust);

            var origin = new AccountDropDownListOrigin(context.UnityContainer)
            {
                Source = AccountSource.MasterIdentifier
            };

            origin.FilterCriteria.CurrencyCapabilities = CurrencyCapabilities.Transfer;

            if (null != accountNumber)
                origin.SelectedAccountNumber = accountNumber;

            var itemTemplates = AccountDisplayHelper.BuildAccountDropDownListItemTemplates(origin);

            var step1TemplateWrapper = new TakeTrustFormTemplateWrapper.Step1(template);

            step1TemplateWrapper.Control1StorePurse.Items.Clear();
            step1TemplateWrapper.Control1StorePurse.Items.AddRange(itemTemplates);

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            form.ApplyTemplate(template);

            form.ServiceCommand += (sender, args) =>
            {
                if (TakeTrustFormValuesWrapper.Step1.Control7WMIDCommandFindPassport.Equals(args.Command))
                {
                    var identifierValue = (string) args.Argument;
                    IdentifierDisplayHelper.ShowFindCertificateForm(form, context, identifierValue);

                    return;
                }

                if (TakeTrustFormValuesWrapper.Step2.Control4SmsReferenceCommandGoTo.Equals(args.Command))
                {
                    Process.Start((string) args.Argument);
                }
            };

            form.WorkCallback = (step, list) =>
            {
                var trustService = context.UnityContainer.Resolve<ITrustService>();

                switch (step)
                {
                    case 0:
                        var step1ValuesWrapper = new TakeTrustFormValuesWrapper.Step1(list);

                        ExtendedIdentifier extendedIdentifier;

                        switch (step1ValuesWrapper.Control5IdentifierType)
                        {
                            case TakePaymentFormValuesWrapper.Step1.Control3IdentifierTypeValueWmid:
                                extendedIdentifier = new ExtendedIdentifier(ExtendedIdentifierType.WmId,
                                    step1ValuesWrapper.Control7WMID);
                                break;
                            case TakePaymentFormValuesWrapper.Step1.Control3IdentifierTypeValuePhone:
                                extendedIdentifier = new ExtendedIdentifier(ExtendedIdentifierType.Phone,
                                    step1ValuesWrapper.Control6Phone);
                                break;
                            case TakePaymentFormValuesWrapper.Step1.Control3IdentifierTypeValueEmail:
                                extendedIdentifier = new ExtendedIdentifier(ExtendedIdentifierType.Email,
                                    step1ValuesWrapper.Control8Email);
                                break;
                            default:
                                throw new InvalidOperationException(
                                    "step1ValuesWrapper.Control5IdentifierType == " +
                                    step1ValuesWrapper.Control5IdentifierType);
                        }

                        var originalExpressTrust =
                            new OriginalExpressTrust(step1ValuesWrapper.Control1StorePurse, extendedIdentifier);

                        originalExpressTrust.DayLimit = step1ValuesWrapper.Control2DailyAmountLimit;
                        originalExpressTrust.WeekLimit = step1ValuesWrapper.Control3WeeklyAmountLimit;
                        originalExpressTrust.MonthLimit = step1ValuesWrapper.Control4MonthlyAmountLimit;

                        var trustConfirmationInstruction = trustService.RequestTrust(originalExpressTrust);

                        var step2IncomeValuesWrapper = new TakeTrustFormValuesWrapper.Step2
                        {
                            Control1RequestNumber =
                            trustConfirmationInstruction.Reference.ToString(),
                            Control2Message =
                            trustConfirmationInstruction.PublicMessage ?? string.Empty,


                            Control4SmsReference =
                            !string.IsNullOrEmpty(trustConfirmationInstruction.SmsReference)
                                ? string.Format(CultureInfo.InvariantCulture, CheckSmsStateUrlTemplate, trustConfirmationInstruction.SmsReference)
                                : string.Empty
                        };

                        return step2IncomeValuesWrapper.CollectIncomeValues();

                    case 1:
                        var step2ValuesWrapper = new TakeTrustFormValuesWrapper.Step2(list);

                        var trustConfirmation =
                            new TrustConfirmation(int.Parse(step2ValuesWrapper.Control1RequestNumber),
                                step2ValuesWrapper.Control5Code);

                        trustService.ConfirmTrust(trustConfirmation);

                        break;
                }

                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}
