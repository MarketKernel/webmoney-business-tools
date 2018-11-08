using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class SendMessageToDeveloperFormTemplateWrapper : StronglyTypedTemplateWrapper
    {
        public TextBoxTemplate Control1YourName => (TextBoxTemplate) GetControlTemplate(0);

        public TextBoxTemplate Control2InstallationReference =>
            (TextBoxTemplate) GetControlTemplate("InstallationReference");

        public TextBoxTemplate Control3Message => (TextBoxTemplate) GetControlTemplate(2);

        public SendMessageToDeveloperFormTemplateWrapper(SubmitFormTemplate<WMColumnTemplate> template)
            : base(template.Steps[0])
        {
        }
    }
}