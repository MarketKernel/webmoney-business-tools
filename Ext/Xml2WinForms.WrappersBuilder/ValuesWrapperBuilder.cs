using System;
using System.Collections.Generic;
using System.Text;
using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace Xml2WinForms.WrappersBuilder
{
    internal static class ValuesWrapperBuilder
    {
        static ValuesWrapperBuilder()
        {
            ControlTemplateJsonConverter.Logics = new WMConverterLogics();
        }

        public static string BuildClass(string className, SubmitFormTemplate<WMColumnTemplate> submitFormTemplate)
        {
            if (null == className)
                throw new ArgumentNullException(nameof(className));

            if (null == submitFormTemplate)
                throw new ArgumentNullException(nameof(submitFormTemplate));

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("using System;");
            stringBuilder.AppendLine("using System.Collections.Generic;");

            stringBuilder.AppendLine("");

            stringBuilder.AppendLine("namespace WMBusinessTools.Extensions.StronglyTypedWrappers");
            stringBuilder.AppendLine("{");

            bool onlyOneStep = 1 == submitFormTemplate.Steps.Count;

            stringBuilder.AppendLine("internal sealed class " + className + (onlyOneStep ? ": StronglyTypedValuesWrapper" : ""));
            stringBuilder.AppendLine("{");

            int stepNumber = 1;

            foreach (var submitFormTemplateStep in submitFormTemplate.Steps)
            {
                var innerClassName = "Step" + stepNumber;
                var constructorName = innerClassName;

                if (!onlyOneStep)
                {
                    stringBuilder.AppendLine("public sealed class " + innerClassName + ": StronglyTypedValuesWrapper");
                    stringBuilder.AppendLine("{");
                }
                else
                    constructorName = className;

                int controlNumber = 1;

                foreach (var tunableShapeColumn in submitFormTemplateStep.TunableShape.Columns)
                {
                    controlNumber = ProcessColumn(stringBuilder, tunableShapeColumn.Controls, controlNumber);
                }

                stringBuilder.AppendLine();

                stringBuilder.AppendLine("public " + constructorName + "()");
                stringBuilder.AppendLine("{");
                stringBuilder.AppendLine("}");

                stringBuilder.AppendLine("public " + constructorName + "(List<object> values)");
                stringBuilder.AppendLine("{");
                stringBuilder.AppendLine("if (null == values)");
                stringBuilder.AppendLine("throw new ArgumentNullException(nameof(values));");
                stringBuilder.AppendLine("ApplyOutcomeValues(values);");
                stringBuilder.AppendLine("}");

                if (!onlyOneStep)
                    stringBuilder.AppendLine("}");

                stepNumber++;
            }

            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }

        private static int ProcessColumn(StringBuilder stringBuilder, List<ControlTemplate> controls, int controlNumber)
        {
            foreach (var controlTemplate in controls)
            {
                switch (controlTemplate.Type)
                {
                    case ControlType.TextBox:
                    case ControlType.TextBoxWithButton:
                    case ControlType.ComboBox:
                        {
                            AddProperty(stringBuilder, "string", controlTemplate.Name, controlTemplate.Desc, controlNumber - 1);

                            // Добавляем константы
                            if (ControlType.ComboBox.Equals(controlTemplate.Type))
                            {
                                var comboBoxTemplate = (ComboBoxTemplate)controlTemplate;

                                var propertyName = FormatUtility.BuildPropertyName(controlTemplate.Name, controlTemplate.Desc, controlNumber - 1);

                                foreach (var comboBoxItemTemplate in comboBoxTemplate.Items)
                                {
                                    var valueName = comboBoxItemTemplate.Value;
                                    valueName = valueName.Replace(".", " ");
                                    valueName = FormatUtility.ToCamelCase(valueName);

                                    stringBuilder.AppendLine("public const string " + propertyName + "Value" +
                                                  valueName + " = \"" + comboBoxItemTemplate.Value +
                                                   "\";");
                                }
                            }

                            // Добавляем константы
                            if (ControlType.TextBoxWithButton.Equals(controlTemplate.Type))
                            {
                                var textBoxWithButtonTemplate = (TextBoxWithButtonTemplate)controlTemplate;

                                var propertyName = FormatUtility.BuildPropertyName(controlTemplate.Name, controlTemplate.Desc, controlNumber - 1);

                                foreach (var textBoxWithButtonBehaviorRule in textBoxWithButtonTemplate.BehaviorRules)
                                {
                                    if (textBoxWithButtonBehaviorRule.Trigger.Equals("ButtonClick"))
                                    {
                                        stringBuilder.AppendLine("public const string " + propertyName + "Command" +
                                                                 textBoxWithButtonBehaviorRule.Action + " = \"" + textBoxWithButtonBehaviorRule.Action + "\";");
                                    }
                                }
                            }

                        }
                        break;
                    case ControlType.NumericUpDown:
                    case WMControlType.AmountNumericUpDown:
                        AddProperty(stringBuilder, "decimal?", controlTemplate.Name, controlTemplate.Desc, controlNumber - 1);
                        break;
                    case ControlType.CheckBox:
                        AddProperty(stringBuilder, "bool?", controlTemplate.Name, controlTemplate.Desc, controlNumber - 1);
                        break;
                    case ControlType.DateTimePicker:
                        AddProperty(stringBuilder, "DateTime?", controlTemplate.Name, controlTemplate.Desc, controlNumber - 1);
                        break;
                    case ControlType.Table:
                        AddProperty(stringBuilder, "object", controlTemplate.Name, controlTemplate.Desc, controlNumber - 1);
                        break;
                    case WMControlType.AccountDropDownList:
                        AddProperty(stringBuilder, "string", controlTemplate.Name, controlTemplate.Desc, controlNumber - 1);
                        break;
                    case ControlType.GroupBox:
                        controlNumber = ProcessColumn(stringBuilder,
                            ((GroupBoxTemplate<WMColumnTemplate>)controlTemplate).Column.Controls,
                            controlNumber);
                        controlNumber--;
                        break;
                    default:
                        throw new InvalidOperationException("controlTemplate.Type = " + controlTemplate.Type);
                }

                controlNumber++;
            }

            return controlNumber;
        }

        private static void AddProperty(StringBuilder stringBuilder, string type, string name, string desc, int index)
        {
            if (null == name)
                stringBuilder.AppendLine("public " + type + " " + FormatUtility.BuildPropertyName(name, desc, index) + " => " + "(" + type + ")GetValue(" + index + ");");
            else
            {
                stringBuilder.AppendLine("public " + type + " " + FormatUtility.BuildPropertyName(name, desc, index));
                stringBuilder.AppendLine("{");
                stringBuilder.AppendLine("get => " + "(" + type + ")GetValue(" + index + ");");
                stringBuilder.AppendLine("set => " + "SetValue(\"" + name + "\", value);");
                stringBuilder.AppendLine("}");
            }
        }
    }
}
