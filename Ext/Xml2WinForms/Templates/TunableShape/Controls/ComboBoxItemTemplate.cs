using System;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class ComboBoxItemTemplate :Template
    {
        private string _text;

        [JsonProperty("text", Required = Required.Always)]
        [XmlAttribute("text")]
        public string Text
        {
            get => Translator.Instance.Translate(TemplateName, _text);
            set => _text = value;
        }

        [JsonProperty("value", Required = Required.Always)]
        [XmlAttribute("value")]
        public string Value { get; set; }

        [JsonProperty("selected")]
        [XmlAttribute("selected")]
        public bool Selected { get; set; }

        internal ComboBoxItemTemplate()
        {
        }

        public ComboBoxItemTemplate(string text, string value)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
