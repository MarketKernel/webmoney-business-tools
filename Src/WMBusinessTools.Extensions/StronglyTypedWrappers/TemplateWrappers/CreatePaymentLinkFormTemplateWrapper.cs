using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class CreatePaymentLinkFormTemplateWrapper
    {
        public sealed class Step1 : StronglyTypedTemplateWrapper
        {
            public AccountDropDownListTemplate Control1StorePurse =>
                (AccountDropDownListTemplate) GetControlTemplate(0);

            public NumericUpDownTemplate Control2OrderId => (NumericUpDownTemplate) GetControlTemplate("OrderId");

            public AmountNumericUpDownTemplate Control3PaymentAmount =>
                (AmountNumericUpDownTemplate) GetControlTemplate("PaymentAmount");

            public NumericUpDownTemplate Control4ValidityPeriod => (NumericUpDownTemplate) GetControlTemplate(3);
            public TextBoxTemplate Control5Description => (TextBoxTemplate) GetControlTemplate(4);

            public Step1(SubmitFormTemplate<WMColumnTemplate> template)
                : base(template.Steps[0])
            {
            }
        }

        public sealed class Step2 : StronglyTypedTemplateWrapper
        {
            public TextBoxWithButtonTemplate Control1PaymentLink => (TextBoxWithButtonTemplate) GetControlTemplate(
                "PaymentLink");

            public Step2(SubmitFormTemplate<WMColumnTemplate> template)
                : base(template.Steps[1])
            {
            }
        }
    }
}