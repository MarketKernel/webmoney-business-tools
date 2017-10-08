using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Xml2WinForms.Templates
{
    public sealed class GridColumnTemplate : Template
    {
        private string _headerText;
        private string _name;

        [JsonProperty("name", Required = Required.Always)]
        [XmlAttribute("name")]
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("kind")]
        [JsonConverter(typeof(StringEnumConverter))]
        [XmlAttribute("kind")]
        public ColumnKind Kind { get; set; }

        [JsonProperty("headerText", Required = Required.Always)]
        [XmlAttribute("headerText")]
        public string HeaderText
        {
            get => Translator.Instance.Translate(TemplateName, _headerText);
            set => _headerText = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("maxInputLength")]
        [XmlAttribute("maxInputLength")]
        public int MaxInputLength { get; set; }

        [JsonProperty("fill")]
        [XmlAttribute("fill")]
        public bool Fill { get; set; }

        [JsonProperty("width")]
        [XmlAttribute("width")]
        public int Width { get; set; }

        [JsonProperty("minimumWidth")]
        [XmlAttribute("minimumWidth")]
        public int MinimumWidth { get; set; }

        [JsonProperty("sortable")]
        [XmlAttribute("sortable")]
        public bool Sortable { get; set; }

        [JsonProperty("order")]
        [XmlAttribute("order")]
        public int Order { get; set; } = -1;

        [JsonProperty("sortGlyphDirection")]
        [JsonConverter(typeof(StringEnumConverter))]
        [XmlAttribute("sortGlyphDirection")]
        public SortOrder SortGlyphDirection { get; set; }

        [JsonProperty("visible")]
        [XmlAttribute("visible")]
        public bool Visible { get; set; } = true;

        internal GridColumnTemplate()
        {
        }

        public GridColumnTemplate(string name, string headerText)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Kind = ColumnKind.TextBox;
            HeaderText = headerText ?? throw new ArgumentNullException(nameof(headerText));
            MaxInputLength = 32767;
            Fill = false;
            Width = 100;
            MinimumWidth = 5;
            Sortable = false;
            SortGlyphDirection = SortOrder.None;
        }
    }

}
