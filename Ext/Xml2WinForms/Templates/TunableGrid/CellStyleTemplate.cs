using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Xml2WinForms.Templates
{
    public sealed class CellStyleTemplate : Template
    {
        [JsonProperty("font")]
        [XmlAttribute("font")]
        public FontTemplate Font { get; set; }

        [JsonProperty("backColor")]
        [XmlAttribute("backColor")]
        public string BackColor { get; set; }

        [JsonProperty("foreColor")]
        [XmlAttribute("foreColor")]
        public string ForeColor { get; set; }

        [JsonProperty("selectionBackColor")]
        [XmlAttribute("selectionBackColor")]
        public string SelectionBackColor { get; set; }

        [JsonProperty("selectionForeColor")]
        [XmlAttribute("selectionForeColor")]
        public string SelectionForeColor { get; set; }

        [JsonProperty("alignment")]
        [JsonConverter(typeof(StringEnumConverter))]
        [XmlElement("alignment")]
        public DataGridViewContentAlignment Alignment { get; set; }

        [JsonProperty("padding")]
        [XmlAttribute("padding")]
        public int Padding { get; set; }

        [JsonProperty("wrapMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        [XmlAttribute("wrapMode")]
        public DataGridViewTriState WrapMode { get; set; }
    }
}
