using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.Services;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Utils;

namespace WMBusinessTools.Extensions
{
    public sealed class DbSettingsFormProvider : ITopFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (ApplicationUtility.IsRunningOnMono)
                return false;

            return true;
        }

        public Form GetForm(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var dbSettingsService = new DbSettingsService(context.UnityContainer);

            var connectionSettings = context.Session.AuthenticationService.GetConnectionSettings();

            DbSettingsFormValuesWrapper incomeValuesWrapper = null;

            if (null != connectionSettings)
                incomeValuesWrapper = DbSettingsService.MapToValuesWrapper(connectionSettings);

            var form = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager, ExtensionCatalog.DbSettings,
                incomeValuesWrapper?.CollectIncomeValues());

            form.Closed += (sender, args) =>
            {
                if (DialogResult.OK == form.DialogResult)
                    EventBroker.OnDatabaseChanged();
            };

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new DbSettingsFormValuesWrapper(list);
                connectionSettings = DbSettingsService.ExtractConnectionSettings(valuesWrapper);

                dbSettingsService.CheckConnectionSettings(connectionSettings);

                context.Session.AuthenticationService.SetConnectionSettings(connectionSettings);

                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}
