using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class GroupBoxTemplate<TColumnTemplate> : ControlTemplate
        where TColumnTemplate : class, IShapeColumnTemplate
    {
        private TColumnTemplate _column;

        [JsonProperty("column", Required = Required.Always)]
        [XmlElement("column")]
        public TColumnTemplate Column
        {
            get => _column;
            set => _column = value ?? throw new ArgumentNullException(nameof(value));
        }

        public GroupBoxTemplate()
        {
        }

        public GroupBoxTemplate(string desc)
            :base(ControlType.GroupBox, desc)
        {
        }

        protected override Label BuildLabel()
        {
            return null;
        }

        protected override Control BuildControl()
        {
            var control = new TunableGroupBox();
            control.ApplyTemplate(this);

            return control;
        }

        protected internal override IControlHolder BuildControlHolder()
        {
            var control = BuildControl();
            TuneControl(control);
            return new GroupBoxControlHolder(Name, (TunableGroupBox) control);
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);
            Column.SetTemplateInternals(templateName, baseDirectory);
        }
    }
}
