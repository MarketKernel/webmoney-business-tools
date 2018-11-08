using System;
using System.Windows.Forms;
using Unity;
using WebMoney.Services.Contracts;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.Forms;
using WMBusinessTools.Extensions.Templates;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class CertificateFormProvider : ICertificateFormProvider
    {
        public bool CheckCompatibility(CertificateContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(CertificateContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var template =
                TemplateLoader.LoadTemplate<CertificateFormTemplate>(context.ExtensionManager,
                    ExtensionCatalog.Certificate);

            template.CommandButtons.Clear();
            template.CommandButtons.AddRange(
                CommandBarDisplayHelper.BuildCommandButtons(context.ExtensionManager,
                    ExtensionCatalog.Tags.CertificateExtension));

            // Отключаем кнопки.
            foreach (var buttonTemplate in template.CommandButtons)
            {
                var formProvider =
                    context.ExtensionManager.TryGetCertificateFormProvider(buttonTemplate.Command);

                if (!(formProvider?.CheckCompatibility(context) ?? false))
                    buttonTemplate.Enabled = false;
            }

            template.SetTemplateInternals();

            var form = new CertificateForm(context);

            form.ServiceCommand += (sender, args) =>
            {
                if (TunableList.CellMouseDoubleClickCommandName.Equals(args.Command))
                    return;

                if ("Copy".Equals(args.Command))
                {
                    if (args.Argument is CertificateRecord certificateRecord)
                    {
                        Clipboard.SetText(certificateRecord.Value, TextDataFormat.UnicodeText);
                        return;
                    }

                    if (args.Argument is AttachedIdentifierRecord attachedIdentifierRecord)
                    {
                        var identifierValue = context.UnityContainer.Resolve<IFormattingService>()
                            .FormatIdentifier(attachedIdentifierRecord.Identifier);

                        Clipboard.SetText(identifierValue, TextDataFormat.UnicodeText);
                        return;
                    }

                    return;
                }

                var formProvider =
                    context.ExtensionManager.TryGetCertificateFormProvider(args.Command);

                if (null == formProvider)
                    return;

                formProvider.GetForm(context).Show(form);
            };

            form.ApplyTemplate(template);
            form.DisplayValue(context.Certificate);

            return form;
        }
    }
}