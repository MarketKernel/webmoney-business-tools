using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    [XmlRoot("tunableMenu")]
    public sealed class TunableMenuTemplate : Template
    {
        [JsonProperty("items", Required = Required.Always)]
        [XmlArray(ElementName = "items")]
        [XmlArrayItem("item")]
        public List<MenuItemTemplate> Items{ get; }

        public TunableMenuTemplate()
        {
            Items = new List<MenuItemTemplate>();
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            foreach (var template in Items)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }
        }
    }
}
