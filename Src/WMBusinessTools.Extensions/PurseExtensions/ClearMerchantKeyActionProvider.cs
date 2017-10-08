using System;
using System.Globalization;
using System.Windows.Forms;
using LocalizationAssistant;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;

namespace WMBusinessTools.Extensions
{
    public sealed class ClearMerchantKeyActionProvider : IPurseActionProvider
    {
        public bool CheckCompatibility(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return context.Account.HasMerchantKey;
        }

        public void RunAction(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var message = string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}",
                Translator.Instance.Translate(ExtensionCatalog.ClearMerchantKey, "Merchant key for purse"), context.Account.Number,
                Translator.Instance.Translate(ExtensionCatalog.ClearMerchantKey, "will be deleted permanently."));

            if (DialogResult.OK != MessageBox.Show(null, message,
                    Translator.Instance.Translate(ExtensionCatalog.ClearMerchantKey, "Сonfirm deletion"), MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
                return;

            var purseService = context.UnityContainer.Resolve<IPurseService>();
            purseService.ClearMerchantKey(context.Account.Number);

            EventBroker.OnPurseChanged(new DataChangedEventArgs {FreshDataRequired = false});
        }
    }
}
