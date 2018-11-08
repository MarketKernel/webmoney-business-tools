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
    public sealed class MoneybackFormProvider : ITransferFormProvider
    {
        public bool CheckCompatibility(TransferContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (TransferType.Canceled == context.Transfer.Type)
                return false;

            if (context.Transfer.SourcePurse.Equals(context.Account.Number))
                return false;

            return true;
        }

        public Form GetForm(TransferContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var transfer = context.Transfer;

            var template = TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager,
                TransferType.Protected == context.Transfer.Type
                    ? ExtensionCatalog.RejectProtection
                    : ExtensionCatalog.Moneyback);

            var templateWrapper = new MoneybackFormTemplateWrapper(template);

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();

            templateWrapper.Control6ReturnAmount.CurrencyName =
                currencyService.AddPrefix(currencyService.ObtainCurrencyByAccountNumber(transfer.SourcePurse));
            templateWrapper.Control6ReturnAmount.AvailableAmount = transfer.Amount;
            templateWrapper.Control6ReturnAmount.DefaultValue = transfer.Amount;

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            var formattingService = context.UnityContainer.Resolve<IFormattingService>();

            var incomeValuesWrapper = new MoneybackFormValuesWrapper
            {
                Control1SourcePurse = transfer.SourcePurse,
                Control2TargetPurse = transfer.TargetPurse,
                Control3Amount = formattingService.FormatAmount(transfer.Amount),
                Control4Description = transfer.Description
            };

            form.ApplyTemplate(template, incomeValuesWrapper.CollectIncomeValues());

            form.ServiceCommand += (sender, args) =>
            {
                if (!MoneybackFormValuesWrapper.Control1SourcePurseCommandFindIdentifier.Equals(args.Command))
                    return;

                var purseNumber = (string) args.Argument;
                IdentifierDisplayHelper.ShowFindIdentifierForm(form, context, purseNumber);
            };

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new MoneybackFormValuesWrapper(list);

                var transferService = context.UnityContainer.Resolve<ITransferService>();

                if (TransferType.Protected == context.Transfer.Type)
                    transferService.RejectProtection(transfer.PrimaryId);
                else
                    transferService.Moneyback(transfer.PrimaryId, valuesWrapper.Control5ReturnAmount);

                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}