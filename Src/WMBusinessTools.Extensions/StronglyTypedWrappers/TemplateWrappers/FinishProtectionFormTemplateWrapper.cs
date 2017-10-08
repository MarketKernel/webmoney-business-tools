using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class FinishProtectionFormTemplateWrapper : StronglyTypedTemplateWrapper
    {
        public GroupBoxTemplate<WMColumnTemplate> Control1FinishProtectionGroup => (GroupBoxTemplate<WMColumnTemplate>)
            GetControlTemplate("FinishProtectionGroup");

        public TextBoxWithButtonTemplate Control2SourcePurse => (TextBoxWithButtonTemplate) GetControlTemplate(
            "SourcePurse");

        public TextBoxTemplate Control3TargetPurse => (TextBoxTemplate) GetControlTemplate("TargetPurse");
        public AmountNumericUpDownTemplate Control4Amount => (AmountNumericUpDownTemplate) GetControlTemplate("Amount");
        public TextBoxTemplate Control5Description => (TextBoxTemplate) GetControlTemplate("Description");
        public TextBoxTemplate Control6Code => (TextBoxTemplate) GetControlTemplate("Code");
        public CheckBoxTemplate Control7HoldingFeatureIsUsed => (CheckBoxTemplate) GetControlTemplate(6);

        public FinishProtectionFormTemplateWrapper(SubmitFormTemplate<WMColumnTemplate> template)
            : base(template.Steps[0])
        {
        }
    }
}