using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class BehaviorRule : Template
    {
        private string _trigger;
        private string _action;

        [JsonProperty("trigger", Required = Required.Always)]
        [XmlAttribute("trigger")]
        public string Trigger
        {
            get => _trigger;
            set => _trigger = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("activationСondition")]
        [XmlAttribute("activationСondition")]
        public string ActivationСondition { get; set; }

        [JsonProperty("affectedControls")]
        [XmlArray(ElementName = "affectedControls")]
        [XmlArrayItem("affectedControl")]
        public List<string> AffectedControls{ get; set; }

        [JsonProperty("action", Required = Required.Always)]
        [XmlAttribute("action")]
        public string Action
        {
            get => _action;
            set => _action = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("actionParameter")]
        [XmlAttribute("actionParameter")]
        public string ActionParameter { get; set; }

        [JsonProperty("additionalParameters")]
        [XmlArray(ElementName = "additionalParameters")]
        [XmlArrayItem("additionalParameter")]
        public List<string> AdditionalParameters { get; }

        internal BehaviorRule()
        {
            AdditionalParameters = new List<string>();
        }

        public BehaviorRule(string trigger, string action)
        {
            Trigger = trigger ?? throw new ArgumentNullException(nameof(trigger));
            Action = action ?? throw new ArgumentNullException(nameof(action));
            AdditionalParameters = new List<string>();
        }
    }
}
