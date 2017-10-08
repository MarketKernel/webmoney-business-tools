using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xml2WinForms.Utils;

namespace Xml2WinForms.Templates
{
    public sealed class ListIconTemplate : Template
    {
        private string _name;

        [JsonProperty("name", Required = Required.Always)]
        [XmlAttribute("name")]
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("iconBytes")]
        [XmlAttribute("iconBytes")]
        public byte[] IconBytes { get; set; }


        [JsonProperty("iconPath")]
        [XmlAttribute("iconPath")]
        public string IconPath { get; set; }

        internal ListIconTemplate()
        {
        }

        public ListIconTemplate(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        internal IconHolder BuildIconHolder()
        {
            if (null != IconBytes)
                return new IconHolder(IconBytes);
            if (null != IconPath)
                return new IconHolder(BaseDirectory, IconPath);

            return null;
        }
    }
}
