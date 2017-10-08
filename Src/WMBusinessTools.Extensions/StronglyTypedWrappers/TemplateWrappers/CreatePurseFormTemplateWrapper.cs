using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class CreatePurseFormTemplateWrapper : StronglyTypedTemplateWrapper
    {
        public AccountDropDownListTemplate Control1PurseType => (AccountDropDownListTemplate) GetControlTemplate(0);
        public TextBoxTemplate Control2PurseName => (TextBoxTemplate) GetControlTemplate(1);

        public CreatePurseFormTemplateWrapper(SubmitFormTemplate<WMColumnTemplate> template)
            : base(template.Steps[0])
        {
        }
    }
}