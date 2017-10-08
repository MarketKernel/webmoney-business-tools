using System;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class ListColumnTemplate : Template
    {
        private string _headerText;
        private string _name;

        [JsonProperty("name", Required = Required.Always)]
        [XmlAttribute("name")]
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("headerText", Required = Required.Always)]
        [XmlAttribute("headerText")]
        public string HeaderText
        {
            get => Translator.Instance.Translate(TemplateName, _headerText);
            set => _headerText = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("width")]
        [XmlAttribute("width")]
        public int Width { get; set; } = 100;

        internal ListColumnTemplate()
        {
        }

        public ListColumnTemplate(string name, string headerText)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            HeaderText = headerText ?? throw new ArgumentNullException(nameof(headerText));
        }
    }
}
