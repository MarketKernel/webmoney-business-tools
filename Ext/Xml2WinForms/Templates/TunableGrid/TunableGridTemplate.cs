using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    [XmlRoot("tunableGrid")]
    public sealed class TunableGridTemplate : Template
    {
        [JsonProperty("readOnly")]
        [XmlAttribute("readOnly")]
        public bool ReadOnly { get; set; } = true;

        [JsonProperty("columnHeaderDefaultCellStyle")]
        [XmlElement("columnHeaderDefaultCellStyle")]
        public CellStyleTemplate ColumnHeaderDefaultCellStyle { get; set; }

        [JsonProperty("rowHeaderDefaultCellStyle")]
        [XmlElement("rowHeaderDefaultCellStyle")]
        public CellStyleTemplate RowHeaderDefaultCellStyle { get; set; }

        [JsonProperty("rowsDefaultCellStyle")]
        [XmlElement("rowsDefaultCellStyle")]
        public CellStyleTemplate RowsDefaultCellStyle { get; set; }

        [JsonProperty("gridColor")]
        [XmlAttribute("gridColor")]
        public string GridColor { get; set; }

        [JsonProperty("columns", Required = Required.Always)]
        [XmlArray(ElementName = "columns")]
        [XmlArrayItem("column")]
        public List<GridColumnTemplate> Columns { get; }

        [JsonProperty("commandMenu")]
        [XmlElement("commandMenu")]
        public TunableMenuTemplate CommandMenu { get; set; }

        public TunableGridTemplate()
        {
            Columns = new List<GridColumnTemplate>();
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            ColumnHeaderDefaultCellStyle?.SetTemplateInternals(templateName, baseDirectory);
            RowHeaderDefaultCellStyle?.SetTemplateInternals(templateName, baseDirectory);
            RowsDefaultCellStyle?.SetTemplateInternals(templateName, baseDirectory);

            foreach (var template in Columns)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }

            CommandMenu?.SetTemplateInternals(templateName, baseDirectory);
        }
    }
}
