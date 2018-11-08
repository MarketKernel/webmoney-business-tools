using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
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
    public sealed class RejectInvoiceFormProvider : IIncomingInvoiceFormProvider, IOutgoingInvoiceFormProvider
    {
        public bool CheckCompatibility(IncomingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return CheckCompatibility(context.Invoice.State);
        }

        public bool CheckCompatibility(OutgoingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return CheckCompatibility(context.Invoice.State);
        }

        public bool CheckCompatibility(InvoiceState invoiceState)
        {
            switch (invoiceState)
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
            var formattingService = context.UnityContainer.Resolve<IFormattingService>();

            var incomeValuesWrapper = new RejectInvoiceFormValuesWrapper
            {
                Control1TargetIdentifier = formattingService.FormatIdentifier(invoice.TargetIdentifier),
                Control2TargetPurse = invoice.TargetPurse,
                Control3Amount = invoice.Amount,
                Control4Description = invoice.Description ?? string.Empty,
                Control5OrderId = invoice.OrderId.ToString(),
                Control6Address = invoice.Address ?? string.Empty,
                Control7PaymentPeriod = invoice.ExpirationPeriod
            };

            if (null != invoice.ProtectionPeriod)
                incomeValuesWrapper.Control8ProtectionPeriod = invoice.ProtectionPeriod.Value;

            return GetForm(context, context.Invoice.PrimaryId, context.Invoice.TargetPurse, incomeValuesWrapper);
        }

        public Form GetForm(OutgoingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var invoice = context.Invoice;

            var formattingService = context.UnityContainer.Resolve<IFormattingService>();

            var incomeValuesWrapper = new RejectInvoiceFormValuesWrapper
            {
                Control1TargetIdentifier = formattingService.FormatIdentifier(invoice.ClientIdentifier),
                Control2TargetPurse = invoice.TargetPurse,
                Control3Amount = invoice.Amount,
                Control4Description = invoice.Description ?? string.Empty,
                Control5OrderId = invoice.OrderId.ToString(),
                Control6Address = invoice.Address ?? string.Empty,
                Control7PaymentPeriod = invoice.ExpirationPeriod,
                Control8ProtectionPeriod = invoice.ProtectionPeriod
            };

            return GetForm(context, context.Invoice.PrimaryId, context.Invoice.TargetPurse, incomeValuesWrapper);
        }

        private Form GetForm(SessionContext context, long invoiceId, string targetPurse,
            RejectInvoiceFormValuesWrapper valuesWrapper)
        {
            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.RejectInvoice);

            var templateWrapper = new RejectInvoiceFormTemplateWrapper(template);

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            var currency = currencyService.ObtainCurrencyByAccountNumber(targetPurse);

            templateWrapper.Control4Amount.CurrencyName = currencyService.AddPrefix(currency);

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            form.ApplyTemplate(template, valuesWrapper.CollectIncomeValues());

            form.ServiceCommand += (sender, args) =>
            {
                if (!RejectInvoiceFormValuesWrapper.Control1TargetIdentifierCommandFindPassport.Equals(args.Command))
                    return;

                IdentifierDisplayHelper.ShowFindCertificateForm(form, context, (string) args.Argument);
            };

            form.WorkCallback = (step, list) =>
            {
                var invoiceService = context.UnityContainer.Resolve<IInvoiceService>();
                invoiceService.RejectInvoice(invoiceId);
                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}
