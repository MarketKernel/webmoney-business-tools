using System;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Xml2WinForms.Templates
{
    public sealed class InspectionRule : Template
    {
        private string _message;

        [JsonProperty("type", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        [XmlAttribute("type")]
        public InspectionType Type { get; set; }

        [JsonProperty("argument")]
        [XmlAttribute("argument")]
        public string Argument { get; set; }

        [JsonProperty("number")]
        [XmlAttribute("number")]
        public int Number { get; set; }

        [JsonProperty("message", Required = Required.Always)]
        [XmlAttribute("message")]
        public string Message
        {
            get => Translator.Instance.Translate(TemplateName, _message);
            set => _message = value;
        }

        internal InspectionRule()
        {
        }

        public InspectionRule(InspectionType type, string message)
        {
            Type = type;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}
