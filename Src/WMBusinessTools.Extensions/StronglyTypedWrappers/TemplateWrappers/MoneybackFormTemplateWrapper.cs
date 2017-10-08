using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class MoneybackFormTemplateWrapper : StronglyTypedTemplateWrapper
    {
        public GroupBoxTemplate<WMColumnTemplate> Control1ReturnPaymentGroup => (GroupBoxTemplate<WMColumnTemplate>)
            GetControlTemplate("ReturnPaymentGroup");

        public TextBoxWithButtonTemplate Control2SourcePurse => (TextBoxWithButtonTemplate) GetControlTemplate(
            "SourcePurse");

        public TextBoxTemplate Control3TargetPurse => (TextBoxTemplate) GetControlTemplate("TargetPurse");
        public TextBoxTemplate Control4Amount => (TextBoxTemplate) GetControlTemplate("Amount");
        public TextBoxTemplate Control5Description => (TextBoxTemplate) GetControlTemplate("Description");

        public AmountNumericUpDownTemplate Control6ReturnAmount => (AmountNumericUpDownTemplate) GetControlTemplate(
            "ReturnAmount");

        public MoneybackFormTemplateWrapper(SubmitFormTemplate<WMColumnTemplate> template)
            : base(template.Steps[0])
        {
        }
    }
}