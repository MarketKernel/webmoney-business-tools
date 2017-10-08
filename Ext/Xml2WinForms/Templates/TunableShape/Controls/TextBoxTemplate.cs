using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class TextBoxTemplate : ControlTemplate
    {
        public const int MultilineTextBoxHeight = 75;

        [JsonProperty("defaultValue")]
        [XmlAttribute("defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty("maxLength")]
        [XmlAttribute("maxLength")]
        public int MaxLength { get; set; } = 32767;

        [JsonProperty("multiline")]
        [XmlAttribute("multiline")]
        public bool Multiline { get; set; }

        [JsonProperty("readOnly")]
        [XmlAttribute("readOnly")]
        public bool ReadOnly { get; set; }

        [JsonProperty("digitsOnly")]
        [XmlAttribute("digitsOnly")]
        public bool DigitsOnly { get; set; }

        [JsonProperty("useSystemPasswordChar")]
        [XmlAttribute("useSystemPasswordChar")]
        public bool UseSystemPasswordChar { get; set; }

        internal TextBoxTemplate()
        {
        }

        public TextBoxTemplate(string desc)
            :base(ControlType.TextBox, desc)
        {
        }

        protected override Control BuildControl()
        {
            var control = new TunableTextBox();
            control.ApplyTemplate(this);

            return control;
        }
    }
}