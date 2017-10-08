using System;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions
{
    public sealed class AboutFormProvider : ITopFormProvider
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

            var aboutBox = new AboutBox();

            var template =
                TemplateLoader.LoadTemplate<AboutBoxTemplate>(context.ExtensionManager, ExtensionCatalog.About);

            aboutBox.ApplyTemplate(template);

            return aboutBox;
        }
    }
}
