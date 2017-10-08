using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class DateTimePickerTemplate : ControlTemplate
    {
        [JsonProperty("format")]
        [XmlAttribute("format")]
        public string Format { get; set; } = "yyyy.MM.dd HH:mm:ss";

        [JsonProperty("defaultValue")]
        [XmlAttribute("defaultValue")]
        public DateTime DefaultValue { get; set; } = DateTime.Now;

        internal DateTimePickerTemplate()
        {
        }

        public DateTimePickerTemplate(string desc)
            :base(ControlType.DateTimePicker, desc)
        {
        }

        protected override Control BuildControl()
        {
            if (null == Format)
                throw new BadTemplateException("null == Format");

            var control = new TunableDateTimePicker();
            control.ApplyTemplate(this);

            return control;
        }
    }
}
