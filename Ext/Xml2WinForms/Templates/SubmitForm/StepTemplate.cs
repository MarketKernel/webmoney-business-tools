using System;
using System.Xml.Serialization;
using LocalizationAssistant;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public sealed class StepTemplate<TColumnTemplate> : Template
        where TColumnTemplate : IShapeColumnTemplate
    {
        private string _description;
        private string _actionText;
        private TunableShapeTemplate<TColumnTemplate> _tunableShape;

        [JsonProperty("description")]
        [XmlAttribute("description")]
        public string Description
        {
            get => Translator.Instance.Translate(TemplateName, _description);
            set => _description = value;
        }

        [JsonProperty("tunableShape", Required = Required.Always)]
        [XmlElement("tunableShape")]
        public TunableShapeTemplate<TColumnTemplate> TunableShape
        {
            get => _tunableShape;
            set => _tunableShape = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("actionText", Required = Required.Always)]
        [XmlAttribute("actionText")]
        public string ActionText
        {
            get => Translator.Instance.Translate(TemplateName, _actionText);
            set => _actionText = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal StepTemplate()
        {
        }

        public StepTemplate(TunableShapeTemplate<TColumnTemplate> tunableShape, string actionText)
        {
            TunableShape = tunableShape ?? throw new ArgumentNullException(nameof(tunableShape));
            ActionText = actionText ?? throw new ArgumentNullException(nameof(actionText));
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            TunableShape.SetTemplateInternals(templateName, baseDirectory);
        }
    }
}