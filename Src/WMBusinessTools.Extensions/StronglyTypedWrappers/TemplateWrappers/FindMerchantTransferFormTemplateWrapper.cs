using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class FindMerchantTransferFormTemplateWrapper
    {
        public sealed class Step1 : StronglyTypedTemplateWrapper
        {
            public AccountDropDownListTemplate Control1FromPurse => (AccountDropDownListTemplate) GetControlTemplate(0);
            public NumericUpDownTemplate Control2Number => (NumericUpDownTemplate) GetControlTemplate("Number");
            public ComboBoxTemplate Control3NumberType => (ComboBoxTemplate) GetControlTemplate("NumberType");

            public Step1(SubmitFormTemplate<WMColumnTemplate> template)
                : base(template.Steps[0])
            {
            }
        }

        public sealed class Step2 : StronglyTypedTemplateWrapper
        {
            public TableTemplate Control1Payment => (TableTemplate) GetControlTemplate("Payment");

            public Step2(SubmitFormTemplate<WMColumnTemplate> template)
                : base(template.Steps[1])
            {
            }
        }
    }
}