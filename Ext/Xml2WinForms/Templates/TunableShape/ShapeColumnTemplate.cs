using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class ShapeColumnTemplate : Template, IShapeColumnTemplate
    {
        [JsonProperty("controls", Required = Required.Always)]
        [XmlArray(ElementName = "controls")]
        [XmlArrayItem("textBox", Type = typeof(TextBoxTemplate))]
        [XmlArrayItem("comboBox", Type = typeof(ComboBoxTemplate))]
        [XmlArrayItem("numericUpDown", Type = typeof(NumericUpDownTemplate))]
        [XmlArrayItem("checkBox", Type = typeof(CheckBoxTemplate))]
        [XmlArrayItem("textBoxWithButton", Type = typeof(TextBoxWithButtonTemplate))]
        [XmlArrayItem("dateTimePicker", Type = typeof(DateTimePickerTemplate))]
        [XmlArrayItem("table", Type = typeof(TableTemplate))]
        [XmlArrayItem("groupBox", Type = typeof(GroupBoxTemplate<ShapeColumnTemplate>))]
        public List<ControlTemplate> Controls { get; }

        public ShapeColumnTemplate()
        {
            Controls = new List<ControlTemplate>();
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            foreach (var template in Controls)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }
        }
    }
}
