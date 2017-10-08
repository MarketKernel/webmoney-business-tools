using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class DbSettingsFormTemplateWrapper : StronglyTypedTemplateWrapper
    {
        public ComboBoxTemplate Control1DataSource => (ComboBoxTemplate) GetControlTemplate("DataSource");

        public TextBoxWithButtonTemplate Control2PathToDatabase => (TextBoxWithButtonTemplate) GetControlTemplate(
            "PathToDatabase");

        public CheckBoxTemplate Control3UsePassword => (CheckBoxTemplate) GetControlTemplate("UsePassword");
        public TextBoxTemplate Control4Password => (TextBoxTemplate) GetControlTemplate("Password");

        public TextBoxTemplate Control5PasswordConfirmation => (TextBoxTemplate) GetControlTemplate(
            "PasswordConfirmation");

        public TextBoxTemplate Control6ConnectionString => (TextBoxTemplate) GetControlTemplate("ConnectionString");

        public DbSettingsFormTemplateWrapper(SubmitFormTemplate<WMColumnTemplate> template)
            : base(template.Steps[0])
        {
        }
    }
}