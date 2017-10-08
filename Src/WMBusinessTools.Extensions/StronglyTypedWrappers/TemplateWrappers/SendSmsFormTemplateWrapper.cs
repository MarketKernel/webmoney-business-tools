using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class SendSmsFormTemplateWrapper : StronglyTypedTemplateWrapper
    {
        public TextBoxTemplate Control1PhoneNumber => (TextBoxTemplate) GetControlTemplate(0);
        public TextBoxTemplate Control2Message => (TextBoxTemplate) GetControlTemplate(1);
        public CheckBoxTemplate Control3UseTransliteration => (CheckBoxTemplate) GetControlTemplate(2);

        public SendSmsFormTemplateWrapper(SubmitFormTemplate<WMColumnTemplate> template)
            : base(template.Steps[0])
        {
        }
    }
}