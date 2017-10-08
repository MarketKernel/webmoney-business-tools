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
using WMBusinessTools.Extensions.Properties;
using WMBusinessTools.Extensions.Services;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class CreateOutgoingInvoiceFormProvider : ICertificateFormProvider, IPurseFormProvider
    {
        public bool CheckCompatibility(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            var currency = currencyService.ObtainCurrencyByAccountNumber(context.Account.Number);

            if (!currencyService.CheckCapabilities(currency, CurrencyCapabilities.Invoice))
                return false;

            return true;
        }

        public bool CheckCompatibility(CertificateContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, null, context.Account.Number);
        }

        public Form GetForm(CertificateContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (!new AccountService(context.UnityContainer).CheckingAccountExists())
                return ErrorFormDisplayHelper.BuildErrorForm(context.ExtensionManager,
                    Resources.CreateTrustFormProvider_GetForm_Purses_list_is_empty,
                    Resources.CreateTrustFormProvider_GetForm_Please_refresh_the_list_of_purses_or_add_it_manually);

            var identifierValue = context.UnityContainer.Resolve<IFormattingService>()
                .FormatIdentifier(context.Certificate.Identifier);

            return GetForm(context, identifierValue, null);
        }

        public Form GetForm(SessionContext context, string sourceIdentifierValue, string targetAccountNumber)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.CreateOutgoingInvoice);

            var templateWrapper = new CreateOutgoingInvoiceFormTemplateWrapper(template);

            if (null != sourceIdentifierValue)
                templateWrapper.Control2ReceiversWmid.DefaultValue = sourceIdentifierValue;

            // Кошельки
            var origin = new AccountDropDownListOrigin(context.UnityContainer);

            if (null != targetAccountNumber)
                origin.SelectedAccountNumber = targetAccountNumber;

            origin.FilterCriteria.CurrencyCapabilities = CurrencyCapabilities.Invoice;

            var itemTemplates = AccountDisplayHelper.BuildAccountDropDownListItemTemplates(origin);

            templateWrapper.Control3PayTo.Items.Clear();
            templateWrapper.Control3PayTo.Items.AddRange(itemTemplates);

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            var incomeValuesWrapper =
                new CreateOutgoingInvoiceFormValuesWrapper
                {
                    Control1OrderId = context.Session.SettingsService.AllocateOrderId()
                };

            form.ApplyTemplate(template, incomeValuesWrapper.CollectIncomeValues());

            form.ServiceCommand += (sender, args) =>
            {
                if (!CreateOutgoingInvoiceFormValuesWrapper.Control2ReceiversWmidCommandFindPassport
                    .Equals(args.Command))
                    return;

                IdentifierDisplayHelper.ShowFindCertificateForm(form, context, (string) args.Argument);
            };

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new CreateOutgoingInvoiceFormValuesWrapper(list);


                var originalInvoice = new OriginalOutgoingInvoice(valuesWrapper.Control1OrderId,
                    valuesWrapper.Control2ReceiversWmid, valuesWrapper.Control3PayTo, valuesWrapper.Control4Amount,
                    valuesWrapper.Control5Description);

                if (valuesWrapper.Control6SpecifyAdditionalParameters)
                {
                    originalInvoice.Address = valuesWrapper.Control7Address;

                    if (valuesWrapper.Control8SpecifyPaymentPeriod)
                        originalInvoice.ExpirationPeriod = valuesWrapper.Control9PaymentPeriod;

                    if (valuesWrapper.Control10AllowPaymentWithProtection)
                        originalInvoice.ProtectionPeriod = valuesWrapper.Control11ProtectionPeriod;
                }

                var invoiceService = context.UnityContainer.Resolve<IInvoiceService>();

                invoiceService.CreateOutgoingInvoice(originalInvoice);

                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}
