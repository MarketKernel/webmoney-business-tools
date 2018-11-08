using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    [XmlRoot("tunableList")]
    public sealed class TunableListTemplate : Template
    {
        [JsonProperty("headerClickable")]
        [XmlAttribute("headerClickable")]
        public bool HeaderClickable { get; set; }

        [JsonProperty("groups")]
        [XmlArray(ElementName = "groups")]
        [XmlArrayItem("group")]
        public List<ListGroupTemplate> Groups { get; }

        [JsonProperty("columns", Required = Required.Always)]
        [XmlArray(ElementName = "columns")]
        [XmlArrayItem("column")]
        public List<ListColumnTemplate> Columns { get; }

        [JsonProperty("icons")]
        [XmlArray(ElementName = "icons")]
        [XmlArrayItem("icon")]
        public List<ListIconTemplate> Icons { get; }

        [JsonProperty("checkBoxes")]
        [XmlAttribute("checkBoxes")]
        public bool CheckBoxes { get; }

        [JsonProperty("commandMenu")]
        [XmlElement("commandMenu")]
        public TunableMenuTemplate CommandMenu { get; set; }

        public TunableListTemplate()
        {
            Groups = new List<ListGroupTemplate>();
            Columns = new List<ListColumnTemplate>();
            Icons = new List<ListIconTemplate>();
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            foreach (var template in Groups)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }

            foreach (var template in Columns)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }

            foreach (var template in Icons)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }

            CommandMenu?.SetTemplateInternals(templateName, baseDirectory);
        }
    }
}