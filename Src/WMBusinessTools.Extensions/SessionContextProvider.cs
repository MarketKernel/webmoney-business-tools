using System;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.Forms;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions
{
    public sealed class SessionContextProvider : ISessionContextProvider
    {
        public bool CheckCompatibility(EntranceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public SessionContext GetIdentifierContext(EntranceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var form = new EnterForm(context);

            var template =
                TemplateLoader.LoadTemplate<TunableListTemplate>(context.ExtensionManager,
                    ExtensionCatalog.Enter);

            form.ApplyTemplate(template);
            form.DisplayItems();

            if (DialogResult.OK != form.ShowDialog())
                return null;

            return new SessionContext(context, form.Session);
        }
    }
}
