using System;
using System.Collections.Generic;
using System.Linq;
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
    public sealed class PayInvoiceFormProvider : IIncomingInvoiceFormProvider
    {
        public bool CheckCompatibility(IncomingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            switch (context.Invoice.State)
            {
                case InvoiceState.NotPaid:
                    return true;
                default:
                    return false;
            }
        }

        public Form GetForm(IncomingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var invoice = context.Invoice;

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            var currency = currencyService.ObtainCurrencyByAccountNumber(invoice.TargetPurse);

            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.PayInvoice);

            var templateWrapper = new PayInvoiceFormTemplateWrapper(template);

            templateWrapper.Control4Amount.CurrencyName = currencyService.AddPrefix(currency);

            // Устанавливаем кошельки
            var origin = new AccountDropDownListOrigin(context.UnityContainer);

            // Оплата в долг не требует денег на кошельке, осуществляется с кредитного кошелька (WMC).
            if (currencyService.CheckCapabilities(currency, CurrencyCapabilities.Debit))
                currency = currencyService.SelectCurrencies(CurrencyCapabilities.Credit).First();
            else
                origin.FilterCriteria.HasMoney = true;

            origin.FilterCriteria.Currency = currency;

            origin.FilterCriteria.CurrencyCapabilities = CurrencyCapabilities.Actual | CurrencyCapabilities.TransferByInvoice;

            var itemTemplates = AccountDisplayHelper.BuildAccountDropDownListItemTemplates(origin);

            templateWrapper.Control11PayFrom.Items.Clear();
            templateWrapper.Control11PayFrom.Items.AddRange(itemTemplates);

            // Запрещена оплата с протекцией
            if (null == invoice.ProtectionPeriod)
                templateWrapper.Control12UsePaymentProtection.Enabled = false;
            else
                templateWrapper.Control14ProtectionPeriod.MaxValue = invoice.ProtectionPeriod.Value;

            var formattingService = context.UnityContainer.Resolve<IFormattingService>();

            var incomeValuesWrapper = new PayInvoiceFormValuesWrapper
            {
                Control1TargetIdentifier = formattingService.FormatIdentifier(invoice.TargetIdentifier),
                Control2TargetPurse = invoice.TargetPurse,
                Control3Amount = invoice.Amount,
                Control4Description = invoice.Description ?? string.Empty,
                Control5OrderId = invoice.OrderId.ToString(),
                Control6Address = invoice.Address ?? string.Empty,
                Control7PaymentPeriod = invoice.ExpirationPeriod.ToString(),
                Control9TransferId = context.Session.SettingsService.AllocateTransferId()
            };

            if (null != invoice.ProtectionPeriod)
                incomeValuesWrapper.Control8MaxProtectionPeriod = invoice.ProtectionPeriod.Value.ToString();

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            form.ApplyTemplate(template, incomeValuesWrapper.CollectIncomeValues());

            form.ServiceCommand += (sender, args) =>
            {
                if (!PayInvoiceFormValuesWrapper.Control1TargetIdentifierCommandFindPassport.Equals(args.Command))
                    return;

                IdentifierDisplayHelper.ShowFindCertificateForm(form, context, (string) args.Argument);
            };

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new PayInvoiceFormValuesWrapper(list);
                var transferService = context.UnityContainer.Resolve<ITransferService>();

                var originalTransfer = new OriginalTransfer(valuesWrapper.Control9TransferId,
                    valuesWrapper.Control10PayFrom, invoice.TargetPurse, invoice.Amount,
                    invoice.Description)
                {
                    InvoiceId = invoice.PrimaryId
                };

                if (valuesWrapper.Control11UsePaymentProtection)
                {
                    originalTransfer.ProtectionPeriod = valuesWrapper.Control12ProtectionPeriod;

                    if (valuesWrapper.Control14ProtectionByTime)
                        originalTransfer.ProtectionCode = valuesWrapper.Control13ProtectionCode;
                }

                transferService.CreateTransfer(originalTransfer);

                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}
