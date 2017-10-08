using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ExtensibilityAssistant
{
    public sealed class TagConfiguration
    {
        private string _tagName;

        [JsonProperty("tagName", Required = Required.Always)]
        [XmlElement("tagName")]
        public string TagName
        {
            get => _tagName;
            set => _tagName = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("groupOrder")]
        [XmlElement("groupOrder")]
        public decimal GroupOrder { get; set; } = 100;

        [JsonProperty("order")]
        [XmlElement("order")]
        public decimal Order { get; set; } = 100;

        [JsonProperty("contextSpecificExtensionName")]
        [XmlElement("contextSpecificExtensionName")]
        public string ContextSpecificExtensionName { get; set; }

        internal TagConfiguration()
        {
        }

        public TagConfiguration(string tagName)
        {
            TagName = tagName ?? throw new ArgumentNullException(nameof(tagName));
        }
    }
}