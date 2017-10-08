using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    [XmlRoot("listScreenTemplate")]
    public sealed class ListScreenTemplate : Template
    {
        private TunableListTemplate _tunableList;

        [JsonProperty("tunableList", Required = Required.Always)]
        [XmlAttribute("tunableList")]
        public TunableListTemplate TunableList
        {
            get => _tunableList;
            set => _tunableList = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("commandButtons")]
        [XmlArray(ElementName = "commandButtons")]
        [XmlArrayItem("commandButton")]
        public List<TunableButtonTemplate> CommandButtons { get; }

        internal ListScreenTemplate()
        {
            CommandButtons = new List<TunableButtonTemplate>();
        }

        public ListScreenTemplate(TunableListTemplate tunableList)
        {
            TunableList = tunableList ?? throw new ArgumentNullException(nameof(tunableList));
            CommandButtons = new List<TunableButtonTemplate>();
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            TunableList.SetTemplateInternals(templateName, baseDirectory);

            foreach (var template in CommandButtons)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }
        }
    }
}
