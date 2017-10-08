using System;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;
using Xml2WinForms.Utils;

namespace Xml2WinForms.Templates
{
    public sealed class MenuItemTemplate : Template
    {
        private string _text;

        [JsonProperty("text", Required = Required.Always)]
        [XmlAttribute("text")]
        public string Text
        {
            get => Translator.Instance.Translate(TemplateName, _text);
            set => _text = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("command")]
        [XmlAttribute("command")]
        public string Command { get; set; }

        [JsonProperty("iconBytes")]
        [XmlAttribute("iconBytes")]
        public byte[] IconBytes { get; set; }

        [JsonProperty("iconPath")]
        [XmlAttribute("iconPath")]
        public string IconPath { get; set; }

        internal MenuItemTemplate()
        {
        }

        public MenuItemTemplate(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
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
