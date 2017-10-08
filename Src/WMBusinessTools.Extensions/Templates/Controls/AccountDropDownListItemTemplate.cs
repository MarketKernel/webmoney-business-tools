using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.Templates.Controls
{
    internal sealed class AccountDropDownListItemTemplate : Template
    {
        [JsonProperty("number", Required = Required.Always)]
        [XmlAttribute("number")]
        public string Number { get; set; }

        [JsonProperty("name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
        [XmlElement("amount", IsNullable = true)]
        public decimal? Amount { get; set; }

        [JsonProperty("currency")]
        [XmlAttribute("currency")]
        public string Currency { get; set; }

        [JsonProperty("availableAmount")]
        [XmlElement("availableAmount", IsNullable = true)]
        public decimal? AvailableAmount { get; set; }

        [JsonProperty("recommendedAmount")]
        [XmlElement("recommendedAmount", IsNullable = true)]
        public decimal? RecommendedAmount { get; set; }

        [JsonProperty("selected")]
        [XmlAttribute("selected")]
        public bool Selected { get; set; }

        internal AccountDropDownListItemTemplate()
        {
        }

        public AccountDropDownListItemTemplate(string number)
        {
            Number = number ?? throw new ArgumentNullException(nameof(number));
        }
    }
}
