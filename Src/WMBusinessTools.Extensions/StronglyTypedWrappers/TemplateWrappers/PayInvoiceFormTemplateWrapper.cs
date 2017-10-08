using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class PayInvoiceFormTemplateWrapper : StronglyTypedTemplateWrapper
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
        public TextBoxTemplate Control8PaymentPeriod => (TextBoxTemplate) GetControlTemplate("PaymentPeriod");

        public TextBoxTemplate Control9MaxProtectionPeriod => (TextBoxTemplate) GetControlTemplate(
            "MaxProtectionPeriod");

        public NumericUpDownTemplate Control10TransferId => (NumericUpDownTemplate) GetControlTemplate("TransferId");
        public AccountDropDownListTemplate Control11PayFrom => (AccountDropDownListTemplate) GetControlTemplate(10);
        public CheckBoxTemplate Control12UsePaymentProtection => (CheckBoxTemplate) GetControlTemplate(11);

        public GroupBoxTemplate<WMColumnTemplate> Control13PaymentProtectionGroup => (GroupBoxTemplate<WMColumnTemplate>
        ) GetControlTemplate("PaymentProtectionGroup");

        public NumericUpDownTemplate Control14ProtectionPeriod => (NumericUpDownTemplate) GetControlTemplate(
            "ProtectionPeriod");

        public TextBoxTemplate Control15ProtectionCode => (TextBoxTemplate) GetControlTemplate(14);

        public PayInvoiceFormTemplateWrapper(SubmitFormTemplate<WMColumnTemplate> template)
            : base(template.Steps[0])
        {
        }
    }
}