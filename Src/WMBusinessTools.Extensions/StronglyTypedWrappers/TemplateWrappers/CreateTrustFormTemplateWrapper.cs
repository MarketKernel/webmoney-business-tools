using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class CreateTrustFormTemplateWrapper : StronglyTypedTemplateWrapper
    {
        public TextBoxWithButtonTemplate Control1MasterIdentifier => (TextBoxWithButtonTemplate) GetControlTemplate(
            "MasterIdentifier");

        public AccountDropDownListTemplate Control2PurseNumber => (AccountDropDownListTemplate) GetControlTemplate(
            "PurseNumber");

        public CheckBoxTemplate Control3InvoiceAllowed => (CheckBoxTemplate) GetControlTemplate("InvoiceAllowed");
        public CheckBoxTemplate Control4BalanceAllowed => (CheckBoxTemplate) GetControlTemplate("BalanceAllowed");
        public CheckBoxTemplate Control5HistoryAllowed => (CheckBoxTemplate) GetControlTemplate("HistoryAllowed");
        public CheckBoxTemplate Control6TransferAllowed => (CheckBoxTemplate) GetControlTemplate("TransferAllowed");

        public GroupBoxTemplate<WMColumnTemplate> Control7AmountLimitsGroup => (GroupBoxTemplate<WMColumnTemplate>)
            GetControlTemplate("AmountLimitsGroup");

        public AmountNumericUpDownTemplate Control8DailyAmountLimit => (AmountNumericUpDownTemplate) GetControlTemplate(
            "DailyAmountLimit");

        public AmountNumericUpDownTemplate Control9DayAmountLimit => (AmountNumericUpDownTemplate) GetControlTemplate(
            "DayAmountLimit");

        public AmountNumericUpDownTemplate Control10WeeklyAmountLimit =>
            (AmountNumericUpDownTemplate) GetControlTemplate("WeeklyAmountLimit");

        public AmountNumericUpDownTemplate Control11MonthlyAmountLimit => (AmountNumericUpDownTemplate)
            GetControlTemplate("MonthlyAmountLimit");

        public CreateTrustFormTemplateWrapper(SubmitFormTemplate<WMColumnTemplate> template)
            : base(template.Steps[0])
        {
        }
    }
}