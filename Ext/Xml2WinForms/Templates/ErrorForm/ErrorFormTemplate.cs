using System;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    [XmlRoot("errorForm")]
    public sealed class ErrorFormTemplate : Template
    {
        private string _caption;
        private string _message;

        [JsonProperty("caption", Required = Required.Always)]
        [XmlAttribute("caption")]
        public string Caption
        {
            get => Translator.Instance.Translate(TemplateName, _caption);
            set => _caption = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonIgnore]
        [XmlIgnore]
        internal string OriginalCaption => _caption;

        [JsonProperty("message", Required = Required.Always)]
        [XmlAttribute("message")]
        public string Message
        {
            get => _message;
            set => _message = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("details")]
        [XmlAttribute("details")]
        public string Details { get; set; }

        [JsonProperty("level")]
        [XmlAttribute("level")]
        public ErrorLevel Level { get; set; }

        internal ErrorFormTemplate()
        {
        }

        public ErrorFormTemplate(string caption, string message)
        {
            Caption = caption ?? throw new ArgumentNullException(nameof(caption));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}
