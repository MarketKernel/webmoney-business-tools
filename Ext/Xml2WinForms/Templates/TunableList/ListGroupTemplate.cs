using System;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class ListGroupTemplate : Template
    {
        private readonly bool _localized;

        private string _headerText;
        private string _key;

        [JsonProperty("key", Required = Required.Always)]
        [XmlAttribute("key")]
        public string Key
        {
            get => _key;
            set => _key = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("headerText", Required = Required.Always)]
        [XmlAttribute("headerText")]
        public string HeaderText
        {
            get => _localized ? _headerText : Translator.Instance.Translate(TemplateName, _headerText);
            set => _headerText = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal ListGroupTemplate()
        {
        }

        public ListGroupTemplate(string key, string headerText, bool localized)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            HeaderText = headerText ?? throw new ArgumentNullException(nameof(headerText));
            _localized = localized;
        }
    }
}
