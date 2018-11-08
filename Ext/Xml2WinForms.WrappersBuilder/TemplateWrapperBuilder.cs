using System;
using System.Collections.Generic;
using System.Text;
using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace Xml2WinForms.WrappersBuilder
{
    internal static class TemplateWrapperBuilder
    {
        static TemplateWrapperBuilder()
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

            stringBuilder.AppendLine("using Xml2WinForms.Templates;");
            stringBuilder.AppendLine("using WMBusinessTools.Extensions.Templates.Controls;");

            stringBuilder.AppendLine("");

            stringBuilder.AppendLine("namespace WMBusinessTools.Extensions.StronglyTypedWrappers");
            stringBuilder.AppendLine("{");

            bool onlyOneStep = 1 == submitFormTemplate.Steps.Count;

            stringBuilder.AppendLine("internal sealed class " + className + (onlyOneStep ? ": StronglyTypedTemplateWrapper" : ""));
            stringBuilder.AppendLine("{");

            int stepNumber = 1;

            foreach (var submitFormTemplateStep in submitFormTemplate.Steps)
            {
                var innerClassName = "Step" + stepNumber;
                var constructorName = innerClassName;

                if (!onlyOneStep)
                {
                    stringBuilder.AppendLine("public sealed class " + innerClassName + ": StronglyTypedTemplateWrapper");
                    stringBuilder.AppendLine("{");
                }
                else
                    constructorName = className;

                int controlNumber = 1;

                foreach (var tunableShapeColumn in submitFormTemplateStep.TunableShape.Columns)
                {
                    var сontrolTemplates = new List<ControlTemplate>();

                    foreach (var controlTemplate in tunableShapeColumn.Controls)
                    {
                        сontrolTemplates.AddRange(SelectControlTemplates(controlTemplate));
                    }

                    foreach (var controlTemplate in сontrolTemplates)
                    {
                        controlNumber = ProcessColumn(stringBuilder, controlTemplate, controlNumber);
                        controlNumber++;
                    }
                }

                stringBuilder.AppendLine();

                stringBuilder.AppendLine("public " + constructorName + "(SubmitFormTemplate<WMColumnTemplate> template)");
                stringBuilder.AppendLine(":base(template.Steps[" + (stepNumber - 1) + "])");
                stringBuilder.AppendLine("{");
                stringBuilder.AppendLine("}");

                if (!onlyOneStep)
                    stringBuilder.AppendLine("}");

                stepNumber++;
            }

            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }

        private static List<ControlTemplate> SelectControlTemplates(ControlTemplate controlTemplate)
        {
            var result = new List<ControlTemplate>();

            switch (controlTemplate.Type)
            {
                case ControlType.GroupBox:

                    var groupBoxTemplate = (GroupBoxTemplate<WMColumnTemplate>)controlTemplate;

                    result.Add(groupBoxTemplate);

                    foreach (var innerControlTemplate in groupBoxTemplate.Column.Controls)
                    {
                        result.AddRange(SelectControlTemplates(innerControlTemplate));
                    }
                    break;
                default:
                    result.Add(controlTemplate);
                    break;
            }

            return result;
        }

        private static int ProcessColumn(StringBuilder stringBuilder, ControlTemplate control, int controlNumber)
        {
            var type = control.Type.Equals(ControlType.GroupBox) ? "GroupBoxTemplate<WMColumnTemplate>" : control.Type + "Template";

            AddProperty(stringBuilder, type, control.Name, control.Desc, controlNumber - 1);

            return controlNumber;
        }

        private static void AddProperty(StringBuilder stringBuilder, string type, string name, string desc, int index)
        {
            if (null == name)
                stringBuilder.AppendLine("public " + type + " " + FormatUtility.BuildPropertyName(name, desc, index) + " => " + "(" + type + ")GetControlTemplate(" + index + ");");
            else
                stringBuilder.AppendLine("public " + type + " " + FormatUtility.BuildPropertyName(name, desc, index) + " => " + "(" + type + ")GetControlTemplate(\"" + name + "\");");
        }
    }
}
