using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.Forms;
using WMBusinessTools.Extensions.Properties;
using WMBusinessTools.Extensions.Services;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions
{
    public sealed class TransferRegisterFormProvider : ITopFormProvider, IPurseFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public bool CheckCompatibility(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            var currency = currencyService.ObtainCurrencyByAccountNumber(context.Account.Number);

            if (!currencyService.CheckCapabilities(currency,
                CurrencyCapabilities.Actual | CurrencyCapabilities.Transfer))
                return false;

            return true;
        }

        public Form GetForm(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (!new AccountService(context.UnityContainer).CheckingAccountExists())
                return ErrorFormDisplayHelper.BuildErrorForm(context.ExtensionManager, Resources.CreateTrustFormProvider_GetForm_Purses_list_is_empty,
                    Resources.CreateTrustFormProvider_GetForm_Please_refresh_the_list_of_purses_or_add_it_manually);

            var template = TemplateLoader.LoadTemplate<TunableGridTemplate>(context.ExtensionManager, ExtensionCatalog.TransferRegister);

            var form = new TransferRegisterForm(context);
            form.ApplyTemlate(template);

            return form;
        }

        public Form GetForm(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var template = TemplateLoader.LoadTemplate<TunableGridTemplate>(context.ExtensionManager, ExtensionCatalog.TransferRegister);

            var form = new TransferRegisterForm(context, context.Account.Number);
            form.ApplyTemlate(template);

            return form;
        }
    }
}
