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
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Templates.Controls;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions
{
    public sealed class VerifyClientFormProvider : ITopFormProvider
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

            var template =
                TemplateLoader.LoadTemplate<SubmitFormTemplate<WMColumnTemplate>>(context.ExtensionManager,
                    ExtensionCatalog.VerifyClient);

            var step1TemplateWrapper = new VerifyClientFormTemplateWrapper.Step1(template);

            // Добавляем список валют
            var itemTemplates =
                AccountDisplayHelper.BuildCurrencyDropDownListItemTemplates(context.UnityContainer,
                    CurrencyCapabilities.Invoice);

            step1TemplateWrapper.Control3PurseType.Items.Clear();
            step1TemplateWrapper.Control3PurseType.Items.AddRange(itemTemplates);

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            form.ApplyTemplate(template);

            form.ServiceCommand += (sender, args) =>
            {
                switch (args.Command)
                {
                    case VerifyClientFormValuesWrapper.Step1.Control6WmidCommandFindPassport:
                        var identifierValue = (string) args.Argument;
                        IdentifierDisplayHelper.ShowFindCertificateForm(form, context, identifierValue);
                        break;
                    case "Copy":
                        Clipboard.SetText(((ResultRecord) args.Argument).Value, TextDataFormat.UnicodeText);
                        break;
                }
            };

            form.WorkCallback = (step, list) =>
            {
                switch (step)
                {
                    case 0:
                    {
                        SuspectedClientInfo suspectedClientInfo;

                        var step1Wrapper = new VerifyClientFormValuesWrapper.Step1(list);

                        var exchangeType = (ExchangeType)Enum.Parse(typeof(ExchangeType), step1Wrapper.Control1Instrument);
                        var currency = context.UnityContainer.Resolve<ICurrencyService>()
                            .RemovePrefix(step1Wrapper.Control3PurseType);
                        long identifier = long.Parse(step1Wrapper.Control6Wmid);

                        switch (exchangeType)
                        {
                            case ExchangeType.Cash:
                                suspectedClientInfo = SuspectedClientInfo.CreateCashPaymentVerification(
                                    currency, step1Wrapper.Control4Amount,
                                    identifier, step1Wrapper.Control9PassportNumber,
                                    step1Wrapper.Control7FirstName, step1Wrapper.Control8SecondName);
                                break;
                            case ExchangeType.OfflineSystem:
                                suspectedClientInfo = SuspectedClientInfo.CreateOfflineSystemPaymentVerification(
                                    currency, step1Wrapper.Control4Amount,
                                    identifier, step1Wrapper.Control7FirstName,
                                    step1Wrapper.Control8SecondName);
                                break;
                            case ExchangeType.BankAccount:
                                suspectedClientInfo = SuspectedClientInfo.CreateBankAccountPaymentVerification(
                                    currency, step1Wrapper.Control4Amount,
                                    identifier, step1Wrapper.Control7FirstName,
                                    step1Wrapper.Control8SecondName, step1Wrapper.Control10BankName,
                                    step1Wrapper.Control11BankAccount);
                                break;
                            case ExchangeType.Bankcard:
                                suspectedClientInfo = SuspectedClientInfo.CreateBankCardPaymentVerification(
                                    currency, step1Wrapper.Control4Amount,
                                    identifier, step1Wrapper.Control7FirstName,
                                    step1Wrapper.Control8SecondName, step1Wrapper.Control10BankName,
                                    step1Wrapper.Control12CardNumber);
                                break;
                            case ExchangeType.InternetSystem:
                                var paymentSystem = (PaymentSystem) Enum.Parse(typeof(PaymentSystem),
                                    step1Wrapper.Control13PaymentSystem);

                                suspectedClientInfo = SuspectedClientInfo.CreateInternetSystemPaymentVerification(
                                    currency, step1Wrapper.Control4Amount,
                                    identifier, paymentSystem,
                                    step1Wrapper.Control14PaymentSystemClientId);
                                break;
                            case ExchangeType.Sms:
                                suspectedClientInfo =
                                    SuspectedClientInfo.CreateSmsPaymentVerification(
                                        currency, step1Wrapper.Control4Amount,
                                        identifier, step1Wrapper.Control15Phone);
                                break;
                            case ExchangeType.Mobile:
                                suspectedClientInfo =
                                    SuspectedClientInfo.CreateMobilePaymentVerification(
                                        currency, step1Wrapper.Control4Amount,
                                        identifier, step1Wrapper.Control15Phone);
                                break;
                            case ExchangeType.Blockchain:
                                var cryptoCurrency = (CryptoCurrency)Enum.Parse(typeof(CryptoCurrency),
                                    step1Wrapper.Control16CryptoCurrency);

                                    suspectedClientInfo =
                                    SuspectedClientInfo.CreateBlockchainPaymentVerification(
                                        currency, step1Wrapper.Control4Amount,
                                        identifier, cryptoCurrency,
                                        step1Wrapper.Control17CryptoCurrencyAddress);
                                break;
                            default:
                                throw new InvalidOperationException(
                                    "valuesWrapper.Control1Instrument == " + step1Wrapper.Control1Instrument);
                        }

                        suspectedClientInfo.Output = step1Wrapper.Control2Direction.Equals(VerifyClientFormValuesWrapper
                            .Step1.Control2DirectionValueOutput);

                        var verificationService = context.UnityContainer.Resolve<IVerificationService>();
                        var verificationReport = verificationService.VerifyClient(suspectedClientInfo);

                        var listItems = new List<ListItemContent>
                        {
                            new ListItemContent(new ResultRecord("Client name", verificationReport.ClientName)),
                            new ListItemContent(new ResultRecord("Client middle name", verificationReport.ClientMiddleName)),
                            new ListItemContent(new ResultRecord("Reference", verificationReport.Reference))
                        };

                        var step2Wrapper = new VerifyClientFormValuesWrapper.Step2 {Control1Result = listItems};

                        return step2Wrapper.CollectIncomeValues();
                    }
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