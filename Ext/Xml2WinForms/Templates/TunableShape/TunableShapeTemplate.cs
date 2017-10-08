using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    [XmlRoot("tunableShape")]
    public sealed class TunableShapeTemplate<TColumnTemplate> : Template
        where TColumnTemplate: IShapeColumnTemplate
    {
        [JsonProperty("columns", Required = Required.Always)]
        [XmlArray(ElementName = "columns")]
        [XmlArrayItem("column")]
        public List<TColumnTemplate> Columns { get; }

        public TunableShapeTemplate()
        {
            Columns = new List<TColumnTemplate>();
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            foreach (var template in Columns)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }
        }
    }
}