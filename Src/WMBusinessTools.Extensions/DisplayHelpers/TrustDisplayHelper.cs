using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LocalizationAssistant;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers.Origins;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions.DisplayHelpers
{
    public static class TrustDisplayHelper
    {
        public static Form CreateForm(SessionContext context, ITrust trust)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager,
                    null == trust ? ExtensionCatalog.CreateTrust : ExtensionCatalog.UpdateTrust);

            if (null != trust)
                template.Text = Translator.Instance.Translate(ExtensionCatalog.UpdateTrust, "Change trust");

            // Устанавливаем кошельки
            var origin = new AccountDropDownListOrigin(context.UnityContainer);

            origin.FilterCriteria.CurrencyCapabilities = CurrencyCapabilities.Actual;

            if (null != trust)
            {
                origin.Source = AccountSource.Trusts;
                origin.SelectedAccountNumber = trust.Purse;
            }

            var itemTemplates = AccountDisplayHelper.BuildAccountDropDownListItemTemplates(origin);

            var templateWrapper = new CreateTrustFormTemplateWrapper(template);

            templateWrapper.Control2PurseNumber.Items.Clear();
            templateWrapper.Control2PurseNumber.Items.AddRange(itemTemplates);

            if (null != trust)
            {
                templateWrapper.Control1MasterIdentifier.ReadOnly = true;
                templateWrapper.Control2PurseNumber.Enabled = false;
            }

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            var incomeValuesWrapper = new CreateTrustFormValuesWrapper();

            if (null != trust)
            {
                var formattingService = context.UnityContainer.Resolve<IFormattingService>();

                incomeValuesWrapper.Control1MasterIdentifier =
                    formattingService.FormatIdentifier(trust.MasterIdentifier);
                incomeValuesWrapper.Control2PurseNumber = trust.Purse;
                incomeValuesWrapper.Control3InvoiceAllowed = trust.InvoiceAllowed;
                incomeValuesWrapper.Control4BalanceAllowed = trust.BalanceAllowed;
                incomeValuesWrapper.Control5HistoryAllowed = trust.HistoryAllowed;
                incomeValuesWrapper.Control6TransferAllowed = trust.TransferAllowed;

                if (trust.TransferAllowed)
                {
                    incomeValuesWrapper.Control7DailyAmountLimit = trust.DayLimit;
                    incomeValuesWrapper.Control8DayAmountLimit = trust.DayLimit;
                    incomeValuesWrapper.Control9WeeklyAmountLimit = trust.WeekLimit;
                    incomeValuesWrapper.Control10MonthlyAmountLimit = trust.MonthLimit;
                }
            }

            form.ApplyTemplate(template, incomeValuesWrapper.CollectIncomeValues());

            form.ServiceCommand += (sender, args) =>
            {
                if (!CreateTrustFormValuesWrapper.Control1MasterIdentifierCommandFindPassport.Equals(args.Command))
                    return;

                var identifierValue = (string)args.Argument;
                IdentifierDisplayHelper.ShowFindCertificateForm(form, context, identifierValue);
            };

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new CreateTrustFormValuesWrapper(list);

                var purse = null != trust ? trust.Purse : valuesWrapper.Control2PurseNumber;

                var originalTrust = new OriginalTrust(long.Parse(valuesWrapper.Control1MasterIdentifier), purse)
                {
                    InvoiceAllowed = valuesWrapper.Control3InvoiceAllowed,
                    BalanceAllowed = valuesWrapper.Control4BalanceAllowed,
                    HistoryAllowed = valuesWrapper.Control5HistoryAllowed,
                    TransferAllowed = valuesWrapper.Control6TransferAllowed
                };

                if (valuesWrapper.Control6TransferAllowed)
                {
                    originalTrust.DayLimit = valuesWrapper.Control8DayAmountLimit;
                    originalTrust.WeekLimit = valuesWrapper.Control9WeeklyAmountLimit;
                    originalTrust.MonthLimit = valuesWrapper.Control10MonthlyAmountLimit;
                }

                var trustService = context.UnityContainer.Resolve<ITrustService>();
                trustService.CreateTrust(originalTrust);

                return new Dictionary<string, object>();
            };

            form.FinalAction = objects =>
            {
                EventBroker.OnTrustChanged(new DataChangedEventArgs {FreshDataRequired = true});
                return true;
            };

            return form;
        }
    }
}
