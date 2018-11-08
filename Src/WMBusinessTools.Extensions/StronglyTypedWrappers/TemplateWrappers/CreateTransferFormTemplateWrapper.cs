using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class CreateTransferFormTemplateWrapper : StronglyTypedTemplateWrapper
    {
        public NumericUpDownTemplate Control1TransferId => (NumericUpDownTemplate) GetControlTemplate("TransferId");
        public TextBoxWithButtonTemplate Control2ToPurse => (TextBoxWithButtonTemplate) GetControlTemplate(1);
        public AccountDropDownListTemplate Control3FromPurse => (AccountDropDownListTemplate) GetControlTemplate(2);
        public AmountNumericUpDownTemplate Control4Amount => (AmountNumericUpDownTemplate) GetControlTemplate("Amount");
        public TextBoxTemplate Control5Description => (TextBoxTemplate) GetControlTemplate(4);
        public CheckBoxTemplate Control6UsePaymentProtection => (CheckBoxTemplate) GetControlTemplate(5);

        public GroupBoxTemplate<WMColumnTemplate> Control7PaymentProtectionGroup =>
            (GroupBoxTemplate<WMColumnTemplate>) GetControlTemplate("PaymentProtectionGroup");

        public NumericUpDownTemplate Control8ProtectionPeriod =>
            (NumericUpDownTemplate) GetControlTemplate("ProtectionPeriod");

        public TextBoxTemplate Control9ProtectionCode => (TextBoxTemplate) GetControlTemplate("ProtectionCode");
        public CheckBoxTemplate Control10ProtectionByTime => (CheckBoxTemplate) GetControlTemplate(9);

        public CreateTransferFormTemplateWrapper(SubmitFormTemplate<WMColumnTemplate> template)
            : base(template.Steps[0])
        {
        }
    }
}