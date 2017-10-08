using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class TakePaymentFormTemplateWrapper
    {
        public sealed class Step1 : StronglyTypedTemplateWrapper
        {
            public GroupBoxTemplate<WMColumnTemplate> Control1StoreInfo =>
                (GroupBoxTemplate<WMColumnTemplate>) GetControlTemplate(0);

            public AccountDropDownListTemplate Control2StorePurse =>
                (AccountDropDownListTemplate) GetControlTemplate(1);

            public NumericUpDownTemplate Control3OrderId => (NumericUpDownTemplate) GetControlTemplate("OrderId");

            public GroupBoxTemplate<WMColumnTemplate> Control4ClientInfo =>
                (GroupBoxTemplate<WMColumnTemplate>) GetControlTemplate(3);

            public ComboBoxTemplate Control5IdentifierType => (ComboBoxTemplate) GetControlTemplate(4);
            public TextBoxTemplate Control6Phone => (TextBoxTemplate) GetControlTemplate("Phone");
            public TextBoxWithButtonTemplate Control7WMID => (TextBoxWithButtonTemplate) GetControlTemplate("WMID");
            public TextBoxTemplate Control8Email => (TextBoxTemplate) GetControlTemplate("Email");

            public AmountNumericUpDownTemplate Control9PaymentAmount =>
                (AmountNumericUpDownTemplate) GetControlTemplate("PaymentAmount");

            public ComboBoxTemplate Control10ConfirmationType => (ComboBoxTemplate) GetControlTemplate(9);
            public TextBoxTemplate Control11Description => (TextBoxTemplate) GetControlTemplate(10);

            public Step1(SubmitFormTemplate<WMColumnTemplate> template)
                : base(template.Steps[0])
            {
            }
        }

        public sealed class Step2 : StronglyTypedTemplateWrapper
        {
            public GroupBoxTemplate<WMColumnTemplate> Control1Confirmation =>
                (GroupBoxTemplate<WMColumnTemplate>) GetControlTemplate(0);

            public TextBoxTemplate Control2InvoiceId => (TextBoxTemplate) GetControlTemplate("InvoiceId");
            public TextBoxTemplate Control3Message => (TextBoxTemplate) GetControlTemplate("Message");
            public TextBoxTemplate Control4Code => (TextBoxTemplate) GetControlTemplate("Code");
            public CheckBoxTemplate Control5CancelInvoice => (CheckBoxTemplate) GetControlTemplate(4);

            public Step2(SubmitFormTemplate<WMColumnTemplate> template)
                : base(template.Steps[1])
            {
            }
        }
    }
}