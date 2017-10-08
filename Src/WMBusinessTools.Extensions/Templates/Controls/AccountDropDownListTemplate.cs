using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using Xml2WinForms.Templates;
using Newtonsoft.Json;
using WMBusinessTools.Extensions.Controls;

namespace WMBusinessTools.Extensions.Templates.Controls
{
    internal sealed class AccountDropDownListTemplate : ControlTemplate
    {
        [JsonProperty("items", Required = Required.Always)]
        [XmlArray(ElementName = "items")]
        [XmlArrayItem("item")]
        public List<AccountDropDownListItemTemplate> Items { get; }

        internal AccountDropDownListTemplate()
        {
            Items = new List<AccountDropDownListItemTemplate>();
        }

        public AccountDropDownListTemplate(string desc)
            :base(WMControlType.AccountDropDownList, desc)
        {
            Items = new List<AccountDropDownListItemTemplate>();
        }

        protected override Control BuildControl()
        {
            var control = new AccountDropDownList();
            control.ApplyTemplate(this);

            return control;
        }

        public override void SetTemplateInternals(string templateName, string baseDirectory)
        {
            base.SetTemplateInternals(templateName, baseDirectory);

            foreach (var template in Items)
            {
                template.SetTemplateInternals(templateName, baseDirectory);
            }
        }
    }
}
