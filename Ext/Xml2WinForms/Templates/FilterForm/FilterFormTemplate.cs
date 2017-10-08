using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    [XmlRoot("filterForm")]
    public sealed class FilterFormTemplate<TColumnTemplate> : Template
        where TColumnTemplate : class, IShapeColumnTemplate
    {
        private FilterScreenTemplate<TColumnTemplate> _filterControl;

        [JsonProperty("title")]
        [XmlAttribute("title")]
        public string Title { get; set; }

        [JsonProperty("fileFilter")]
        [XmlAttribute("fileFilter")]
        public string FileFilter { get; set; }

        [JsonProperty("filterScreen", Required = Required.Always)]
        [XmlAttribute("filterScreen")]
        public FilterScreenTemplate<TColumnTemplate> FilterScreen
        {
            get => _filterControl;
            set => _filterControl = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("statusLabels")]
        [XmlArray(ElementName = "tatusLabels")]
        [XmlArrayItem("tatusLabel")]
        public List<StatusLabelTemplate> StatusLabels { get; }

        internal FilterFormTemplate()
        {
            StatusLabels = new List<StatusLabelTemplate>();
        }

        public FilterFormTemplate(FilterScreenTemplate<TColumnTemplate> filterScreen)
        {
            FilterScreen = filterScreen ?? throw new ArgumentNullException(nameof(filterScreen));
            StatusLabels = new List<StatusLabelTemplate>();
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            FilterScreen.SetTemplateInternals(templateName, baseDirectory);

            foreach (var template in StatusLabels)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }
        }
    }
}