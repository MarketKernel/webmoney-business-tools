using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.Templates
{
    [XmlRoot("certificateForm")]
    internal sealed class CertificateFormTemplate : Template
    {
        private TunableListTemplate _certificateTunableList;
        private TunableListTemplate _attachedIdentifierList;

        [JsonProperty("certificateRecordList", Required = Required.Always)]
        [XmlAttribute("certificateRecordList")]
        public TunableListTemplate CertificateRecordList
        {
            get => _certificateTunableList;
            set => _certificateTunableList = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("attachedIdentifierList", Required = Required.Always)]
        [XmlAttribute("attachedIdentifierList")]
        public TunableListTemplate AttachedIdentifierList
        {
            get => _attachedIdentifierList;
            set => _attachedIdentifierList = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("commandButtons")]
        [XmlArray(ElementName = "commandButtons")]
        [XmlArrayItem("commandButton")]
        public List<TunableButtonTemplate> CommandButtons { get; }

        internal CertificateFormTemplate()
        {
            CommandButtons = new List<TunableButtonTemplate>();
        }

        public CertificateFormTemplate(TunableListTemplate certificateRecordList, TunableListTemplate attachedIdentifierList)
        {
            CertificateRecordList = certificateRecordList ?? throw new ArgumentNullException(nameof(certificateRecordList));
            AttachedIdentifierList = attachedIdentifierList ?? throw new ArgumentNullException(nameof(attachedIdentifierList));

            CommandButtons = new List<TunableButtonTemplate>();
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            CertificateRecordList.SetTemplateInternals(templateName, baseDirectory);
            AttachedIdentifierList?.SetTemplateInternals(templateName, baseDirectory);

            foreach (var template in CommandButtons)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }
        }
    }
}