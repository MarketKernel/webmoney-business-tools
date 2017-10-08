using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class TakeTrustFormTemplateWrapper
    {
        public sealed class Step1 : StronglyTypedTemplateWrapper
        {
            public AccountDropDownListTemplate Control1StorePurse =>
                (AccountDropDownListTemplate) GetControlTemplate(0);

            public GroupBoxTemplate<WMColumnTemplate> Control2Limits =>
                (GroupBoxTemplate<WMColumnTemplate>) GetControlTemplate(1);

            public AmountNumericUpDownTemplate Control3DailyAmountLimit =>
                (AmountNumericUpDownTemplate) GetControlTemplate("DailyAmountLimit");

            public AmountNumericUpDownTemplate Control4WeeklyAmountLimit => (AmountNumericUpDownTemplate)
                GetControlTemplate("WeeklyAmountLimit");

            public AmountNumericUpDownTemplate Control5MonthlyAmountLimit => (AmountNumericUpDownTemplate)
                GetControlTemplate("MonthlyAmountLimit");

            public GroupBoxTemplate<WMColumnTemplate> Control6ClientInfo =>
                (GroupBoxTemplate<WMColumnTemplate>) GetControlTemplate(5);

            public ComboBoxTemplate Control7IdentifierType => (ComboBoxTemplate) GetControlTemplate(6);
            public TextBoxTemplate Control8Phone => (TextBoxTemplate) GetControlTemplate("Phone");
            public TextBoxWithButtonTemplate Control9WMID => (TextBoxWithButtonTemplate) GetControlTemplate("WMID");
            public TextBoxTemplate Control10Email => (TextBoxTemplate) GetControlTemplate("Email");
            public ComboBoxTemplate Control11ConfirmationType => (ComboBoxTemplate) GetControlTemplate(10);

            public Step1(SubmitFormTemplate<WMColumnTemplate> template)
                : base(template.Steps[0])
            {
            }
        }

        public sealed class Step2 : StronglyTypedTemplateWrapper
        {
            public TextBoxTemplate Control1RequestNumber => (TextBoxTemplate) GetControlTemplate("RequestNumber");
            public TextBoxTemplate Control2Message => (TextBoxTemplate) GetControlTemplate("Message");

            public TextBoxWithButtonTemplate Control3SmsReference => (TextBoxWithButtonTemplate) GetControlTemplate(
                "SmsReference");

            public TextBoxTemplate Control4Code => (TextBoxTemplate) GetControlTemplate("Code");

            public Step2(SubmitFormTemplate<WMColumnTemplate> template)
                : base(template.Steps[1])
            {
            }
        }
    }
}