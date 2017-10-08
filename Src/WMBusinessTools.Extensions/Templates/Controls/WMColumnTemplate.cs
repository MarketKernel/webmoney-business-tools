using System.Collections.Generic;
using System.Xml.Serialization;
using Xml2WinForms.Templates;
using Newtonsoft.Json;

namespace WMBusinessTools.Extensions.Templates.Controls
{
    internal sealed class WMColumnTemplate : Template, IShapeColumnTemplate
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
        [XmlArrayItem("amountNumericUpDown", Type = typeof(AmountNumericUpDownTemplate))]
        [XmlArrayItem("accountDropDownList", Type = typeof(AccountDropDownListTemplate))]
        [XmlArrayItem("groupBox", Type = typeof(GroupBoxTemplate<WMColumnTemplate>))]
        public List<ControlTemplate> Controls { get; }

        public WMColumnTemplate()
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
