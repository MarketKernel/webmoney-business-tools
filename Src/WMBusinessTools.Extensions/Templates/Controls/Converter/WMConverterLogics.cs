using Xml2WinForms.Templates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WMBusinessTools.Extensions.Templates.Controls
{
    internal sealed class WMConverterLogics : ConverterLogics
    {
        public override object ReadJson(JToken jToken, JsonSerializer serializer)
        {
            var type = (string)jToken["type"];

            ControlTemplate controlTemplate;

            switch (type)
            {
                case "AmountNumericUpDown":
                    controlTemplate = new AmountNumericUpDownTemplate();
                    break;
                case "AccountDropDownList":
                    controlTemplate = new AccountDropDownListTemplate();
                    break;
                case ControlType.GroupBox:
                    controlTemplate = new GroupBoxTemplate<WMColumnTemplate>();
                    break;
                default:
                    return base.ReadJson(jToken, serializer);
            }

            using (var innerReader = jToken.CreateReader())
            {
                serializer.Populate(innerReader, controlTemplate);
            }

            return controlTemplate;
        }
    }
}
