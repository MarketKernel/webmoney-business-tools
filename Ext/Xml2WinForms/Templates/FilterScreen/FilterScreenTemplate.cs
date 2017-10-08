using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    [XmlRoot("filterScreen")]
    public sealed class FilterScreenTemplate<TColumnTemplate> : Template
        where TColumnTemplate : class, IShapeColumnTemplate
    {
        private TunableGridTemplate _grid;
        private string _filterButtonText;

        [JsonProperty("tunableGrid", Required = Required.Always)]
        [XmlAttribute("tunableGrid")]
        public TunableGridTemplate Grid
        {
            get => _grid;
            set => _grid = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("column")]
        [XmlElement("column")]
        public TColumnTemplate Column { get; set; }

        [JsonProperty("filterButtonText")]
        [XmlAttribute("filterButtonText")]
        public string FilterButtonText
        {
            get => Translator.Instance.Translate(TemplateName, _filterButtonText);
            set => _filterButtonText = value;
        }

        [JsonProperty("commandButtons")]
        [XmlArray(ElementName = "commandButtons")]
        [XmlArrayItem("commandButton")]
        public List<TunableButtonTemplate> CommandButtons { get; }

        internal FilterScreenTemplate()
        {
            CommandButtons = new List<TunableButtonTemplate>();
        }

        public FilterScreenTemplate(TunableGridTemplate grid)
        {
            Grid = grid ?? throw new ArgumentNullException(nameof(grid));
            CommandButtons = new List<TunableButtonTemplate>();
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            Grid.SetTemplateInternals(templateName, baseDirectory);
            Column?.SetTemplateInternals(templateName, baseDirectory);

            foreach (var template in CommandButtons)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }
        }
    }
}
