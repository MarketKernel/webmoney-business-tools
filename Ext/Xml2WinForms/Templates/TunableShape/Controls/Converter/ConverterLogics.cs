using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xml2WinForms.Templates
{
    public class ConverterLogics
    {
        public virtual object ReadJson(JToken jToken, JsonSerializer serializer)
        {
            if (null == jToken)
                throw new ArgumentNullException(nameof(jToken));

            if (null == serializer)
                throw new ArgumentNullException(nameof(serializer));

            var type = (string)jToken["type"];

            ControlTemplate controlTemplate;

            switch (type)
            {
                case ControlType.TextBox:
                    controlTemplate = new TextBoxTemplate();
                    break;
                case ControlType.ComboBox:
                    controlTemplate = new ComboBoxTemplate();
                    break;
                case ControlType.NumericUpDown:
                    controlTemplate = new NumericUpDownTemplate();
                    break;
                case ControlType.CheckBox:
                    controlTemplate = new CheckBoxTemplate();
                    break;
                case ControlType.TextBoxWithButton:
                    controlTemplate = new TextBoxWithButtonTemplate();
                    break;
                case ControlType.DateTimePicker:
                    controlTemplate = new DateTimePickerTemplate();
                    break;
                case ControlType.Table:
                    controlTemplate = new TableTemplate();
                    break;
                case ControlType.GroupBox:
                    controlTemplate = new GroupBoxTemplate<ShapeColumnTemplate>();
                    break;
                default:
                    throw new InvalidOperationException("type == " + type);
            }

            using (var innerReader = jToken.CreateReader())
            {
                serializer.Populate(innerReader, controlTemplate);
            }

            return controlTemplate;
        }
    }
}
