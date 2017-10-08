namespace Xml2WinForms.Templates
{
    public interface ITemplate
    {
        string TemplateName { get; }
        string BaseDirectory { get; }

        void SetTemplateInternals(string templateName, string baseDirectory);
    }
}
