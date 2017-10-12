using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ExtensibilityAssistant
{
    public sealed class ExtensionConfiguration
    {
        private string _id;
        private string _name;
        private string _assemblyFullName;
        private string _extensionType;
        private Collection<TagConfiguration> _tags;

        [JsonProperty("id", Required = Required.Always)]
        [XmlAttribute("id")]
        public string Id
        {
            get => _id;
            set => _id = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("name", Required = Required.Always)]
        [XmlAttribute("name")]
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("description")]
        [XmlAttribute("description")]
        public string Description { get; set; }

        [JsonProperty("priority")]
        [XmlAttribute("priority")]
        public byte Priority { get; set; } = 127;

        [JsonProperty("assemblyFullName", Required = Required.Always)]
        [XmlAttribute("assemblyFullName")]
        public string AssemblyFullName
        {
            get => _assemblyFullName;
            set => _assemblyFullName = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("type", Required = Required.Always)]
        [XmlAttribute("type")]
        public string ExtensionType
        {
            get => _extensionType;
            set => _extensionType = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("configurationString")]
        [XmlAttribute("configurationString")]
        public string ConfigurationString { get; set; }

        [JsonProperty("linuxCompatible")]
        [XmlAttribute("linuxCompatible")]
        public bool LinuxCompatible { get; set; } = true;

        [JsonProperty("iconPath")]
        [XmlAttribute("iconPath")]
        public string IconPath { get; set; }

        [JsonProperty("tags")]
        [XmlArray(ElementName = "tags")]
        [XmlArrayItem("tag")]
        public Collection<TagConfiguration> Tags
        {
            get => _tags;
            set => _tags = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonIgnore]
        [XmlIgnore]
        public string BaseDirectory { get; internal set; }

        [JsonIgnore]
        [XmlIgnore]
        public AssemblyBrief AssemblyBrief { get; set; }

        internal ExtensionConfiguration()
        {
        }

        public ExtensionConfiguration(string id, string name, string assemblyFullName, string extensionType)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            AssemblyFullName = assemblyFullName ?? throw new ArgumentNullException(nameof(assemblyFullName));
            ExtensionType = extensionType ?? throw new ArgumentNullException(nameof(extensionType));
            _tags = new Collection<TagConfiguration>();
        }
    }
}