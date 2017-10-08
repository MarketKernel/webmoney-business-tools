using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ExtensibilityAssistant
{
    [XmlRoot("extensionRegister")]
    public sealed class ConfigurationRegister
    {
        private Collection<ExtensionConfiguration> _extensionConfigurations;

        [JsonProperty("extensions", Required = Required.Always)]
        [XmlArray(ElementName = "extensions")]
        [XmlArrayItem("extension")]
        public Collection<ExtensionConfiguration> ExtensionConfigurations =>
            _extensionConfigurations ?? (_extensionConfigurations =
                new Collection<ExtensionConfiguration>());
    }
}