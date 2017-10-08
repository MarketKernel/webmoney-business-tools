using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class PasswordSettingsFormProvider : ITopFormProvider
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

            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.PasswordSettings);

            if (!context.Session.AuthenticationService.HasPassword)
                template.Steps[0].TunableShape.Columns[0].Controls[0].Enabled = false; // Не спрашивать текущий пароль.

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            form.WorkCallback += (step, list) =>
            {
                if (0 != step)
                    throw new InvalidOperationException("0 != step");

                var valuesWrapper = new PasswordSettingsFormValuesWrapper(list);
                context.Session.AuthenticationService.SetPassword(valuesWrapper.Control1OldPassowrd, valuesWrapper.Control2Password);

                return new Dictionary<string, object>();
            };

            form.ApplyTemplate(template);

            return form;
        }
    }
}
