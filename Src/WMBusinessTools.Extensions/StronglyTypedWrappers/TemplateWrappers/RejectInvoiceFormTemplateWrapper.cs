using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class RejectInvoiceFormTemplateWrapper : StronglyTypedTemplateWrapper
    {
        public GroupBoxTemplate<WMColumnTemplate> Control1IncomeInvoiceGroup => (GroupBoxTemplate<WMColumnTemplate>)
            GetControlTemplate("IncomeInvoiceGroup");

        public TextBoxWithButtonTemplate Control2TargetIdentifier => (TextBoxWithButtonTemplate) GetControlTemplate(
            "TargetIdentifier");

        public TextBoxTemplate Control3TargetPurse => (TextBoxTemplate) GetControlTemplate("TargetPurse");
        public AmountNumericUpDownTemplate Control4Amount => (AmountNumericUpDownTemplate) GetControlTemplate("Amount");
        public TextBoxTemplate Control5Description => (TextBoxTemplate) GetControlTemplate("Description");
        public TextBoxTemplate Control6OrderId => (TextBoxTemplate) GetControlTemplate("OrderId");
        public TextBoxTemplate Control7Address => (TextBoxTemplate) GetControlTemplate("Address");

        public NumericUpDownTemplate Control8PaymentPeriod => (NumericUpDownTemplate) GetControlTemplate(
            "PaymentPeriod");

        public NumericUpDownTemplate Control9ProtectionPeriod => (NumericUpDownTemplate) GetControlTemplate(
            "ProtectionPeriod");

        public RejectInvoiceFormTemplateWrapper(SubmitFormTemplate<WMColumnTemplate> template)
            : base(template.Steps[0])
        {
        }
    }
}