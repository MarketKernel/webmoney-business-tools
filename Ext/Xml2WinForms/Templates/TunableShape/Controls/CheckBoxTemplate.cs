using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class CheckBoxTemplate : ControlTemplate
    {
        public const int Height = 17;

        [JsonProperty("defaultValue")]
        [XmlAttribute("defaultValue")]
        public bool DefaultValue { get; set; }

        internal CheckBoxTemplate()
        {
        }

        public CheckBoxTemplate(string desc)
            :base(ControlType.CheckBox, desc)
        {
        }

        protected override Label BuildLabel()
        {
            return null;
        }

        protected override Control BuildControl()
        {
            var control = new TunableCheckBox();
            control.ApplyTemplate(this);

            return control;
        }
    }
}
