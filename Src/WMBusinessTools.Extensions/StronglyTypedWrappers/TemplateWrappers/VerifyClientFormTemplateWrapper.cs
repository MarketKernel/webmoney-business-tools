using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class VerifyClientFormTemplateWrapper
    {
        public sealed class Step1 : StronglyTypedTemplateWrapper
        {
            public ComboBoxTemplate Control1Instrument => (ComboBoxTemplate) GetControlTemplate(0);
            public ComboBoxTemplate Control2Direction => (ComboBoxTemplate) GetControlTemplate(1);
            public AccountDropDownListTemplate Control3PurseType => (AccountDropDownListTemplate) GetControlTemplate(2);

            public AmountNumericUpDownTemplate Control4Amount => (AmountNumericUpDownTemplate) GetControlTemplate(
                "Amount");

            public NumericUpDownTemplate Control5OrderNumber => (NumericUpDownTemplate) GetControlTemplate(4);

            public GroupBoxTemplate<WMColumnTemplate> Control6ClientInfo =>
                (GroupBoxTemplate<WMColumnTemplate>) GetControlTemplate(5);

            public TextBoxWithButtonTemplate Control7Wmid => (TextBoxWithButtonTemplate) GetControlTemplate(6);
            public TextBoxTemplate Control8FirstName => (TextBoxTemplate) GetControlTemplate("FirstName");
            public TextBoxTemplate Control9SecondName => (TextBoxTemplate) GetControlTemplate("SecondName");
            public TextBoxTemplate Control10PassportNumber => (TextBoxTemplate) GetControlTemplate("PassportNumber");
            public TextBoxTemplate Control11BankName => (TextBoxTemplate) GetControlTemplate("BankName");
            public TextBoxTemplate Control12BankAccount => (TextBoxTemplate) GetControlTemplate("BankAccount");
            public TextBoxTemplate Control13CardNumber => (TextBoxTemplate) GetControlTemplate("CardNumber");
            public ComboBoxTemplate Control14PaymentSystem => (ComboBoxTemplate) GetControlTemplate("PaymentSystem");

            public TextBoxTemplate Control15PaymentSystemClientId => (TextBoxTemplate) GetControlTemplate(
                "PaymentSystemClientId");

            public TextBoxTemplate Control16Phone => (TextBoxTemplate) GetControlTemplate("Phone");
            public ComboBoxTemplate Control17CryptoCurrency => (ComboBoxTemplate) GetControlTemplate("CryptoCurrency");

            public TextBoxTemplate Control18CryptoCurrencyAddress => (TextBoxTemplate) GetControlTemplate(
                "CryptoCurrencyAddress");

            public Step1(SubmitFormTemplate<WMColumnTemplate> template)
                : base(template.Steps[0])
            {
            }
        }

        public sealed class Step2 : StronglyTypedTemplateWrapper
        {
            public TableTemplate Control1Result => (TableTemplate) GetControlTemplate("Result");

            public Step2(SubmitFormTemplate<WMColumnTemplate> template)
                : base(template.Steps[1])
            {
            }
        }
    }
}