using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class FontTemplate : Template
    {
        private string _fontName = "Microsoft Sans Serif";

        [JsonProperty("fontName")]
        [XmlAttribute("fontName")]
        public string FontName
        {
            get => _fontName;
            set => _fontName = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("fontSize")]
        [XmlAttribute("fontSize")]
        public float FontSize { get; set; } = 8.25F;

        [JsonProperty("isBold")]
        [XmlAttribute("isBold")]
        public bool IsBold { get; set; }
    }
}
