using System;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class LicenseTemplate : Template
    {
        private string _licenseName;
        private string _licenseUrl;

        [JsonProperty("licenseName", Required = Required.Always)]
        [XmlAttribute("licenseName")]
        public string LicenseName
        {
            get => Translator.Instance.Translate(TemplateName, _licenseName);
            set => _licenseName = value;
        }

        [JsonProperty("licenseUrl")]
        [XmlAttribute("licenseUrl")]
        public string LicenseUrl
        {
            get => Translator.Instance.Translate(TemplateName, _licenseUrl);
            set => _licenseUrl = value;
        }

        internal LicenseTemplate()
        {
        }

        public LicenseTemplate(string licenseName)
        {
            LicenseName = licenseName ?? throw new ArgumentNullException(nameof(licenseName));
        }
    }
}
