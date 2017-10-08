using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xml2WinForms.Templates
{
    public abstract class Template : ITemplate
    {
        [JsonIgnore]
        [XmlIgnore]
        public string TemplateName { get; private set; }

        [JsonIgnore]
        [XmlIgnore]
        public string BaseDirectory { get; private set; }

        public void SetTemplateInternals()
        {
            SetTemplateInternals(TemplateName, BaseDirectory);
        }

        public virtual void SetTemplateInternals(string templateName, string baseDirectory)
        {
            TemplateName = templateName ?? throw new ArgumentNullException(nameof(templateName));
            BaseDirectory = baseDirectory;
        }
    }
}