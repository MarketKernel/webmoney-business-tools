using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class RedeemPaymerFormTemplateWrapper : StronglyTypedTemplateWrapper
    {
        public AccountDropDownListTemplate Control1RedeemTo => (AccountDropDownListTemplate) GetControlTemplate(0);
        public TextBoxTemplate Control2Number => (TextBoxTemplate) GetControlTemplate(1);
        public TextBoxTemplate Control3Code => (TextBoxTemplate) GetControlTemplate(2);

        public RedeemPaymerFormTemplateWrapper(SubmitFormTemplate<WMColumnTemplate> template)
            : base(template.Steps[0])
        {
        }
    }
}