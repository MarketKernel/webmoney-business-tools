using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LocalizationAssistant;
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
    public sealed class FindMerchantTransferFormProvider : IPurseFormProvider, ITransferFormProvider,
        IOutgoingInvoiceFormProvider
    {
        public bool CheckCompatibility(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            var currency = currencyService.ObtainCurrencyByAccountNumber(context.Account.Number);

            if (!currencyService.CheckCapabilities(currency, CurrencyCapabilities.Invoice))
                return false;

            if (AuthenticationMethod.KeeperClassic != context.Session.AuthenticationService.AuthenticationMethod &&
                !context.Account.HasMerchantKey)
                return false;

            return true;
        }

        public bool CheckCompatibility(TransferContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (AuthenticationMethod.KeeperClassic != context.Session.AuthenticationService.AuthenticationMethod &&
                !context.Account.HasMerchantKey)
                return false;

            return true;
        }

        public bool CheckCompatibility(OutgoingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (AuthenticationMethod.KeeperClassic != context.Session.AuthenticationService.AuthenticationMethod &&
                !context.Account.HasMerchantKey)
                return false;

            return true;
        }

        public Form GetForm(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.Account.Number, null, PaymentNumberKind.Auto);
        }

        public Form GetForm(TransferContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.Transfer.TargetPurse, context.Transfer.PrimaryId,
                PaymentNumberKind.TransferPrimaryId);
        }

        public Form GetForm(OutgoingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.Invoice.TargetPurse, context.Invoice.PrimaryId,
                PaymentNumberKind.InvoicePrimaryId);
        }

        private Form GetForm(SessionContext context, string purseNumber, long? id, PaymentNumberKind numberKind)
        {
            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.FindMerchantTransfer);

            var origin =
                new AccountDropDownListOrigin(context.UnityContainer)
                {
                    SelectedAccountNumber = purseNumber
                };

            origin.FilterCriteria.CurrencyCapabilities = CurrencyCapabilities.Invoice;

            var itemTemplates = AccountDisplayHelper.BuildAccountDropDownListItemTemplates(origin);

            var step1TemplateWrapper = new FindMerchantTransferFormTemplateWrapper.Step1(template);

            step1TemplateWrapper.Control1FromPurse.Items.Clear();
            step1TemplateWrapper.Control1FromPurse.Items.AddRange(itemTemplates);

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            var incomeValuesWrapperStep1 = new FindMerchantTransferFormValuesWrapper.Step1();

            if (id.HasValue)
                incomeValuesWrapperStep1.Control2Number = id.Value;

            incomeValuesWrapperStep1.Control3NumberType = numberKind.ToString();

            form.ApplyTemplate(template, incomeValuesWrapperStep1.CollectIncomeValues());

            form.WorkCallback = (step, list) =>
            {
                switch (step)
                {
                    case 0:
                        var valuesWrapperStep1 = new FindMerchantTransferFormValuesWrapper.Step1(list);

                        var paymentService = context.UnityContainer.Resolve<IPaymentService>();

                        var merchantPayment = paymentService.FindPayment(valuesWrapperStep1.Control1FromPurse,
                            valuesWrapperStep1.Control2Number,
                            (PaymentNumberKind) Enum.Parse(typeof(PaymentNumberKind),
                                valuesWrapperStep1.Control3NumberType));

                        var formattingService = context.UnityContainer.Resolve<IFormattingService>();

                        var records = new List<ResultRecord>();
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "System transfer ID"),
                            merchantPayment.TransferId.ToString()));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "System invoice ID"),
                            merchantPayment.InvoiceId.ToString()));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Amount"),
                            formattingService.FormatAmount(merchantPayment.Amount)));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Creation time"),
                            formattingService.FormatDateTime(merchantPayment.CreationTime)));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Description"),
                            merchantPayment.Description ?? string.Empty));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Source purse"),
                            merchantPayment.SourcePurse ?? string.Empty));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Source WMID"),
                            formattingService.FormatIdentifier(merchantPayment.SourceIdentifier)));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Is Capitaller"),
                            merchantPayment.IsCapitaller.ToString()));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Is enum"),
                            merchantPayment.IsEnum.ToString()));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "IP Address"),
                            merchantPayment.IPAddress?.ToString()));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Telepat phone"),
                            merchantPayment.TelepatPhone ?? string.Empty));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Telepat method"),
                            merchantPayment.TelepatMethod?.ToString() ?? string.Empty));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Paymer number"),
                            merchantPayment.PaymerNumber ?? string.Empty));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Paymer email"),
                            merchantPayment.PaymerEmail ?? string.Empty));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Paymer type"),
                            merchantPayment.PaymerType.ToString()));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Cashier number"),
                            merchantPayment.CashierNumber ?? string.Empty));
                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Cashier date"),
                            merchantPayment.CashierDate?.ToString() ?? string.Empty));

                        var cashierAmountValue = string.Empty;

                        if (null != merchantPayment.CashierAmount)
                            cashierAmountValue = formattingService.FormatAmount(merchantPayment.CashierAmount.Value);

                        records.Add(new ResultRecord(
                            Translator.Instance.Translate(ExtensionCatalog.FindMerchantTransfer, "Cashier amount"),
                            cashierAmountValue));
                        // TODO [M] добавить SDP
                        // records.Add(new ResultRecord("sdp_type", merchantPayment.));

                        var valuesWrapperStep2 = new FindMerchantTransferFormValuesWrapper.Step2();
                        valuesWrapperStep2.Control1Payment = records.Select(r => new ListItemContent(r)).ToList();

                        return valuesWrapperStep2.CollectIncomeValues();
                    case 1:
                        return new Dictionary<string, object>();
                    default:
                        throw new InvalidOperationException("step == " + step);
                }
            };

            if (null != id)
            {
                form.Load += (sender, args) =>
                {
                    form.Submit();
                };
            }

            return form;
        }
    }
}