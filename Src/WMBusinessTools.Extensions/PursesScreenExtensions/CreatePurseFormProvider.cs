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
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class CreatePurseFormProvider : ITopFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (context.Session.IsMaster())
                return true;

            return false;
        }

        public Form GetForm(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.CreatePurse);

            // Добавляем список валют
            var itemTemplates =
                AccountDisplayHelper.BuildCurrencyDropDownListItemTemplates(context.UnityContainer,
                    CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse);

            var templateWrapper = new CreatePurseFormTemplateWrapper(template);

            templateWrapper.Control1PurseType.Items.Clear();
            templateWrapper.Control1PurseType.Items.AddRange(itemTemplates);

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            form.ApplyTemplate(template);

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new CreatePurseFormValuesWrapper(list);

                var currencyService = context.UnityContainer.Resolve<ICurrencyService>();
                var purseService = context.UnityContainer.Resolve<IPurseService>();

                purseService.CreatePurse(currencyService.RemovePrefix(valuesWrapper.Control1PurseType),
                    valuesWrapper.Control2PurseName);

                return new Dictionary<string, object>();
            };

            form.FinalAction = objects =>
            {
                EventBroker.OnPurseChanged(new DataChangedEventArgs { FreshDataRequired = true });
                return true;
            };

            return form;
        }
    }
}
