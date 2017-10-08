using System;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.Properties;
using WMBusinessTools.Extensions.Services;

namespace WMBusinessTools.Extensions
{
    public sealed class CreateTrustFormProvider : ITopFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (!new AccountService(context.UnityContainer).CheckingAccountExists())
                return ErrorFormDisplayHelper.BuildErrorForm(context.ExtensionManager, Resources.CreateTrustFormProvider_GetForm_Purses_list_is_empty,
                    Resources.CreateTrustFormProvider_GetForm_Please_refresh_the_list_of_purses_or_add_it_manually);

            return TrustDisplayHelper.CreateForm(context, null);
        }
    }
}
