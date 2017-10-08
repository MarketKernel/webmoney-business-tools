using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class StatusLabelTemplate : Template
    {
        [JsonProperty("textColor")]
        [XmlAttribute("textColor")]
        public string TextColor { get; set; }
    }
}
