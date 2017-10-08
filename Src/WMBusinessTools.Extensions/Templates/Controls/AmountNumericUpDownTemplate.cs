using System.Windows.Forms;
using System.Xml.Serialization;
using Xml2WinForms.Templates;
using Newtonsoft.Json;
using WMBusinessTools.Extensions.Controls;

namespace WMBusinessTools.Extensions.Templates.Controls
{
    internal sealed class AmountNumericUpDownTemplate : ControlTemplate
    {
        [JsonProperty("defaultValue")]
        [XmlAttribute("defaultValue")]
        public decimal DefaultValue { get; set; }

        [JsonProperty("currencyName")]
        [XmlAttribute("currencyName")]
        public string CurrencyName { get; set; }

        [JsonProperty("availableAmount")]
        [XmlElement("availableAmount", IsNullable = true)]
        public decimal? AvailableAmount { get; set; }

        [JsonProperty("readOnly")]
        [XmlAttribute("readOnly")]
        public bool ReadOnly { get; set; }

        internal AmountNumericUpDownTemplate()
        {
        }

        public AmountNumericUpDownTemplate(string desc)
            :base(WMControlType.AmountNumericUpDown, desc)
        {
        }

        protected override Control BuildControl()
        {
            var control = new AmountNumericUpDown();
            control.ApplyTemplate(this);

            return control;
        }
    }
}
