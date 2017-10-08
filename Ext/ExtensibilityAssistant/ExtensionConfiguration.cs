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
        private string _assemblyFile;
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

        [JsonProperty("assemblyFile", Required = Required.Always)]
        [XmlAttribute("assemblyFile")]
        public string AssemblyFile
        {
            get => _assemblyFile;
            set => _assemblyFile = value ?? throw new ArgumentNullException(nameof(value));
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

        public ExtensionConfiguration(string id, string name, string assemblyFile, string extensionType)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            AssemblyFile = assemblyFile ?? throw new ArgumentNullException(nameof(assemblyFile));
            ExtensionType = extensionType ?? throw new ArgumentNullException(nameof(extensionType));
            _tags = new Collection<TagConfiguration>();
        }
    }
}