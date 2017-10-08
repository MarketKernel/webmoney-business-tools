using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Xml2WinForms.Templates
{
    public sealed class NumericUpDownTemplate : ControlTemplate
    {
        [JsonProperty("minValue")]
        [XmlAttribute("minValue")]
        public ulong MinValue { get; set; }

        [JsonProperty("maxValue")]
        [XmlAttribute("maxValue")]
        public ulong MaxValue { get; set; } = 100;

        [JsonProperty("defaultValue")]
        [XmlAttribute("defaultValue")]
        public decimal DefaultValue { get; set; }

        [JsonProperty("readOnly")]
        [XmlAttribute("readOnly")]
        public bool ReadOnly { get; set; }

        [JsonProperty("decimalPlaces")]
        [XmlAttribute("decimalPlaces")]
        public int DecimalPlaces { get; set; }

        [JsonProperty("alignment")]
        [JsonConverter(typeof(StringEnumConverter))]
        [XmlAttribute("alignment")]
        public HorizontalAlignment Alignment { get; set; }

        internal NumericUpDownTemplate()
        {
        }

        public NumericUpDownTemplate(string desc)
            :base(ControlType.NumericUpDown, desc)
        {
        }

        protected override Control BuildControl()
        {
            var control = new TunableNumericUpDown();
            control.ApplyTemplate(this);

            return control;
        }
    }
}
