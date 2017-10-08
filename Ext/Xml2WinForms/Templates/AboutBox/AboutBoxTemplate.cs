using System;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    [XmlRoot("aboutBox")]
    public sealed class AboutBoxTemplate : Template
    {
        private string _description;

        [JsonProperty("description")]
        [XmlAttribute("description")]
        public string Description
        {
            get => Translator.Instance.Translate(TemplateName, _description);
            set => _description = value;
        }

        [JsonProperty("license", Required = Required.Always)]
        [XmlElement("license")]
        public LicenseTemplate License { get; set; }

        [JsonProperty("logo")]
        [XmlElement("logo")]
        public LogoTemplate Logo { get; set; }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            License?.SetTemplateInternals(templateName, baseDirectory);
            Logo?.SetTemplateInternals(templateName, baseDirectory);
        }

        internal AboutBoxTemplate()
        {
        }

        public AboutBoxTemplate(LicenseTemplate license)
        {
            License = license ?? throw new ArgumentNullException(nameof(license));
        }
    }
}
