using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public class TextBoxWithButtonTemplate : ControlTemplate
    {
        [JsonProperty("defaultValue")]
        [XmlAttribute("defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty("maxLength")]
        [XmlAttribute("maxLength")]
        public int MaxLength { get; set; } = 32767;

        [JsonProperty("readOnly")]
        [XmlAttribute("readOnly")]
        public bool ReadOnly { get; set; }

        [JsonProperty("digitsOnly")]
        [XmlAttribute("digitsOnly")]
        public bool DigitsOnly { get; set; }

        [JsonProperty("iconBytes")]
        [XmlAttribute("iconBytes")]
        public byte[] IconBytes { get; set; }

        [JsonProperty("iconPath")]
        [XmlAttribute("iconPath")]
        public string IconPath { get; set; }

        internal TextBoxWithButtonTemplate()
        {
        }

        public TextBoxWithButtonTemplate(string desc, byte[] iconBytes)
            : base(ControlType.TextBoxWithButton, desc)
        {
            IconBytes = iconBytes ?? throw new ArgumentNullException(nameof(iconBytes));
        }

        public TextBoxWithButtonTemplate(string desc, string iconPath)
            : base(ControlType.TextBoxWithButton, desc)
        {
            IconPath = iconPath ?? throw new ArgumentNullException(nameof(iconPath));
        }

        protected override Control BuildControl()
        {
            var control = new TunableTextBoxWithButton();
            control.ApplyTemplate(this);

            return control;
        }
    }
}