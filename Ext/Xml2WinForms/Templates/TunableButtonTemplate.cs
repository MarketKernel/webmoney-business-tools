using System;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class TunableButtonTemplate : Template
    {
        private string _text;
        private string _command;

        [JsonProperty("text")]
        [XmlAttribute("text")]
        public string Text
        {
            get => Translator.Instance.Translate(TemplateName, _text);
            set => _text = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("command")]
        [XmlAttribute("command")]
        public string Command
        {
            get => _command;
            set => _command = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("enabled")]
        [XmlAttribute("enabled")]
        public bool Enabled { get; set; } = true;

        internal TunableButtonTemplate()
        {
        }

        public TunableButtonTemplate(string text, string command)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Command = command ?? throw new ArgumentNullException(nameof(text));
        }
    }
}
