using System;
using System.Collections.Generic;
using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal abstract class StronglyTypedTemplateWrapper
    {
        private readonly List<ControlTemplate> _controlTemplates;
        private readonly Dictionary<string, ControlTemplate> _controlTemplateDictionary;

        protected StronglyTypedTemplateWrapper(StepTemplate<WMColumnTemplate> template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            var controlTemplates = new List<ControlTemplate>();

            foreach (var tunableShapeColumn in template.TunableShape.Columns)
            {
                foreach (var controlTemplate in tunableShapeColumn.Controls)
                {
                    controlTemplates.AddRange(SelectControlTemplates(controlTemplate));
                }
            }

            var controlTemplateDictionary = new Dictionary<string, ControlTemplate>();

            foreach (var controlTemplate in controlTemplates)
            {
                if (null == controlTemplate.Name)
                    continue;

                controlTemplateDictionary.Add(controlTemplate.Name, controlTemplate);
            }

            _controlTemplates = controlTemplates;
            _controlTemplateDictionary = controlTemplateDictionary;
        }

        protected ControlTemplate GetControlTemplate(int index)
        {
            if (null == _controlTemplates)
                throw new InvalidOperationException("null == _template");

            if (index < 0 || index > _controlTemplates.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _controlTemplates[index];
        }

        protected ControlTemplate GetControlTemplate(string name)
        {
            if (null == name)
                throw new ArgumentNullException(nameof(name));

            if (null == _controlTemplates)
                throw new InvalidOperationException("null == _template");

            return _controlTemplateDictionary[name];
        }

        private static List<ControlTemplate> SelectControlTemplates(ControlTemplate controlTemplate)
        {
            var result = new List<ControlTemplate>();

            switch (controlTemplate.Type)
            {
                case ControlType.GroupBox:

                    var groupBoxTemplate = (GroupBoxTemplate<WMColumnTemplate>) controlTemplate;

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
    }
}
