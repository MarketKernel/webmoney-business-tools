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

namespace WMBusinessTools.Extensions
{
    public sealed class SetMerchantKeyFormProvider : IPurseFormProvider
    {
        private const string KeyMask = "{22DB6C13-9E8B-41C8-8D7E-C96CAEEF295D}";

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

        public Form GetForm(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var incomeValuesWrapper = new SetMerchantKeyFormValuesWrapper();

            if (context.Account.HasMerchantKey)
            {
                incomeValuesWrapper.Control1HasSecretKey = true;
                incomeValuesWrapper.Control2SecretKey = KeyMask;
            }

            if (context.Account.HasSecretKeyX20)
            {
                incomeValuesWrapper.Control3HasSecretKeyX20 = true;
                incomeValuesWrapper.Control4SecretKeyX20 = KeyMask;
            }

            var form = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager,
                ExtensionCatalog.SetMerchantKey, incomeValuesWrapper.CollectIncomeValues());

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new SetMerchantKeyFormValuesWrapper(list);

                var purseService = context.UnityContainer.Resolve<IPurseService>();

                if (valuesWrapper.Control1HasSecretKey)
                {
                    if (!KeyMask.Equals(valuesWrapper.Control2SecretKey, StringComparison.Ordinal))
                        purseService.SetMerchantKey(context.Account.Number, valuesWrapper.Control2SecretKey);
                }
                else
                    purseService.ClearMerchantKey(context.Account.Number);

                if (valuesWrapper.Control3HasSecretKeyX20)
                {
                    if (!KeyMask.Equals(valuesWrapper.Control4SecretKeyX20, StringComparison.Ordinal))
                        purseService.SetSecretKeyX20(context.Account.Number, valuesWrapper.Control4SecretKeyX20);
                }
                else
                    purseService.ClearSecretKeyX20(context.Account.Number);

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
