using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class TableTemplate : ControlTemplate
    {
        public const int Height = 150;

        [JsonProperty("headerClickable")]
        [XmlAttribute("headerClickable")]
        public bool HeaderClickable { get; set; }

        [JsonProperty("columns", Required = Required.Always)]
        [XmlArray(ElementName = "columns")]
        [XmlArrayItem("column")]
        public List<ListColumnTemplate> Columns { get; }

        [JsonProperty("icons")]
        [XmlArray(ElementName = "icons")]
        [XmlArrayItem("icon")]
        public List<ListIconTemplate> Icons { get; }

        [JsonProperty("commandMenu")]
        [XmlElement("commandMenu")]
        public TunableMenuTemplate CommandMenu { get; set; }

        internal TableTemplate()
        {
            Columns = new List<ListColumnTemplate>();
            Icons = new List<ListIconTemplate>();
        }

        public TableTemplate(string desc)
            : base(ControlType.Table, desc)
        {
            Columns = new List<ListColumnTemplate>();
            Icons = new List<ListIconTemplate>();
        }

        protected override Control BuildControl()
        {
            var control = new TunableTable();
            control.ApplyTemplate(this);

            return control;
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            foreach (var columns in Columns)
            {
                columns.SetTemplateInternals(templateName, baseDirectory);
            }

            foreach (var icon in Icons)
            {
                icon.SetTemplateInternals(templateName, baseDirectory);
            }
        }
    }
}
