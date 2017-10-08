using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class CreateOutgoingInvoiceFormTemplateWrapper : StronglyTypedTemplateWrapper
    {
        public NumericUpDownTemplate Control1OrderId => (NumericUpDownTemplate) GetControlTemplate("OrderId");
        public TextBoxWithButtonTemplate Control2ReceiversWmid => (TextBoxWithButtonTemplate) GetControlTemplate(1);
        public AccountDropDownListTemplate Control3PayTo => (AccountDropDownListTemplate) GetControlTemplate(2);
        public AmountNumericUpDownTemplate Control4Amount => (AmountNumericUpDownTemplate) GetControlTemplate("Amount");
        public TextBoxTemplate Control5Description => (TextBoxTemplate) GetControlTemplate(4);
        public CheckBoxTemplate Control6SpecifyAdditionalParameters => (CheckBoxTemplate) GetControlTemplate(5);

        public GroupBoxTemplate<WMColumnTemplate> Control7AdditionalParametersGroup => (
            GroupBoxTemplate<WMColumnTemplate>) GetControlTemplate("AdditionalParametersGroup");

        public TextBoxTemplate Control8Address => (TextBoxTemplate) GetControlTemplate(7);
        public CheckBoxTemplate Control9SpecifyPaymentPeriod => (CheckBoxTemplate) GetControlTemplate(8);

        public NumericUpDownTemplate Control10PaymentPeriod => (NumericUpDownTemplate) GetControlTemplate(
            "PaymentPeriod");

        public CheckBoxTemplate Control11AllowPaymentWithProtection => (CheckBoxTemplate) GetControlTemplate(10);

        public NumericUpDownTemplate Control12ProtectionPeriod => (NumericUpDownTemplate) GetControlTemplate(
            "ProtectionPeriod");

        public CreateOutgoingInvoiceFormTemplateWrapper(SubmitFormTemplate<WMColumnTemplate> template)
            : base(template.Steps[0])
        {
        }
    }
}