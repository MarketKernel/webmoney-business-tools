using System;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class LogoTemplate : Template
    {
        private string _url;

        [JsonProperty("imagePath", Required = Required.Always)]
        [XmlAttribute("imagePath")]
        public string ImagePath { get; set; }

        [JsonProperty("url")]
        [XmlAttribute("url")]
        public string Url
        {
            get => Translator.Instance.Translate(TemplateName, _url);
            set => _url = value;
        }

        internal LogoTemplate()
        {
        }

        public LogoTemplate(string imagePath)
        {
            ImagePath = imagePath ?? throw new ArgumentNullException(nameof(imagePath));
        }
    }
}