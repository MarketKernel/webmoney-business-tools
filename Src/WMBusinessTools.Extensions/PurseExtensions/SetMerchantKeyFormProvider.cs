using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.StronglyTypedWrappers;

namespace WMBusinessTools.Extensions
{
    public sealed class SetMerchantKeyFormProvider : IPurseFormProvider
    {
        public bool CheckCompatibility(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (context.Account.HasMerchantKey)
                return false;

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            var currency = currencyService.ObtainCurrencyByAccountNumber(context.Account.Number);

            if (!currencyService.CheckCapabilities(currency, CurrencyCapabilities.Invoice))
                return false;

            return true;
        }

        public Form GetForm(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var form = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager, ExtensionCatalog.SetMerchantKey);

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new SetMerchantKeyFormValuesWrapper(list);

                var purseService = context.UnityContainer.Resolve<IPurseService>();
                purseService.SetMerchantKey(context.Account.Number, valuesWrapper.Control1MerchantKey);

                return new Dictionary<string, object>();
            };

            form.FinalAction = objects =>
            {
                EventBroker.OnPurseChanged(new DataChangedEventArgs { FreshDataRequired = false });
                return true;
            };

            return form;
        }
    }
}
