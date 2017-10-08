using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;
using Xml2WinForms.Utils;

namespace Xml2WinForms.Templates
{
    [JsonConverter(typeof(ControlTemplateJsonConverter))]
    public abstract class ControlTemplate : Template
    {
        public const int ControlMargin = 3;

        private string _desc;
        private string _type;

        [JsonProperty("type", Required = Required.Always)]
        [XmlAttribute("type")]
        public string Type
        {
            get => _type;
            set => _type = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        [JsonProperty("desc", Required = Required.Always)]
        [XmlAttribute("desc")]
        public string Desc
        {
            get => Translator.Instance.Translate(TemplateName, _desc);
            set => _desc = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("enabled")]
        [XmlAttribute("enabled")]
        public bool Enabled { get; set; } = true;

        [JsonProperty("visible")]
        [XmlAttribute("visible")]
        public bool Visible { get; set; } = true;

        [JsonProperty("important")]
        [XmlAttribute("important")]
        public bool Important { get; set; }

        [JsonProperty("controlWidth")]
        [XmlAttribute("controlWidth")]
        public int ControlWidth { get; set; } = 300;

        [JsonProperty("linuxCompatible")]
        [XmlAttribute("linuxCompatible")]
        public bool LinuxCompatible { get; set; } = true;

        [JsonProperty("inspectionRules")]
        [XmlArray(ElementName = "inspectionRules")]
        [XmlArrayItem("inspectionRule")]
        public List<InspectionRule> InspectionRules { get; }

        [JsonProperty("behaviorRules")]
        [XmlArray(ElementName = "behaviorRules")]
        [XmlArrayItem("behaviorRule")]
        public List<BehaviorRule> BehaviorRules { get; }

        protected internal ControlTemplate()
        {
            InspectionRules = new List<InspectionRule>();
            BehaviorRules = new List<BehaviorRule>();
        }

        protected ControlTemplate(string type, string desc)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Desc = desc;

            InspectionRules = new List<InspectionRule>();
            BehaviorRules = new List<BehaviorRule>();
        }

        protected virtual Label BuildLabel()
        {
            var label = new Label
            {
                Text = Desc,
                AutoSize = true
            };

            if (Important)
                label.Font = new Font(label.Font, FontStyle.Bold);

            if (!Visible)
                label.Visible = false;

            return label;
        }

        protected abstract Control BuildControl();

        protected void TuneControl(Control control)
        {
            if (null == control)
                throw new ArgumentNullException(nameof(control));

            control.Width = ControlWidth;
            control.Enabled = Enabled;
            control.Visible = Visible;

            if (!LinuxCompatible && ApplicationUtility.IsRunningOnMono)
                control.Enabled = false;
        }

        protected internal virtual IControlHolder BuildControlHolder()
        {
            var control = BuildControl();
            TuneControl(control);

            return new AtomControlHolder(Name, BuildLabel(), control, InspectionRules, BehaviorRules);
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            foreach (var template in InspectionRules)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }

            foreach (var template in BehaviorRules)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }
        }
    }
}