using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class ComboBoxTemplate : ControlTemplate
    {
        [JsonProperty("items", Required = Required.Always)]
        [XmlArray(ElementName = "items")]
        [XmlArrayItem("item")]
        public List<ComboBoxItemTemplate> Items { get; }

        internal ComboBoxTemplate()
        {
            Items = new List<ComboBoxItemTemplate>();
        }

        public ComboBoxTemplate(string desc)
            :base(ControlType.ComboBox, desc)
        {
            Items = new List<ComboBoxItemTemplate>();
        }

        protected override Control BuildControl()
        {
            if (null == Items)
                throw new InvalidOperationException("null == Items");

            var control = new TunableComboBox();
            control.ApplyTemplate(this);

            return control;
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            foreach (var itemTemplate in Items)
            {
                itemTemplate.SetTemplateInternals(templateName, baseDirectory);
            }
        }
    }
}