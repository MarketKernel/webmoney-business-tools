using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    [XmlRoot("submitForm")]
    public sealed class SubmitFormTemplate<TColumnTemplate> : Template
        where TColumnTemplate : IShapeColumnTemplate
    {
        private string _text;

        [JsonProperty("text", Required = Required.Always)]
        [XmlAttribute("text")]
        public string Text
        {
            get => Translator.Instance.Translate(TemplateName, _text);
            set => _text = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("steps", Required = Required.Always)]
        [XmlArray(ElementName = "steps")]
        [XmlArrayItem("step")]
        public List<StepTemplate<TColumnTemplate>> Steps { get; }

        internal SubmitFormTemplate()
        {
            Steps = new List<StepTemplate<TColumnTemplate>>();
        }

        public SubmitFormTemplate(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Steps = new List<StepTemplate<TColumnTemplate>>();
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            foreach (var template in Steps)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }
        }
    }
}
