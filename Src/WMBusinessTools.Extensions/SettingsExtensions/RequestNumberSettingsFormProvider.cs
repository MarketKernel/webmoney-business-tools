using System;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.Forms;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions
{
    public sealed class RequestNumberSettingsFormProvider : ITopFormProvider
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
                TemplateLoader.LoadTemplate<TunableShapeTemplate<ShapeColumnTemplate>>(context.ExtensionManager, ExtensionCatalog.RequestNumberSettings);

            var requestNumberSettingsForm = new RequestNumberSettingsForm(context);
            requestNumberSettingsForm.ApplyTemplate(template);

            return requestNumberSettingsForm;
        }
    }
}
