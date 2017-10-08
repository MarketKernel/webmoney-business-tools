using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    public sealed class TunableTable : TunableList, IAtomControl, IServiceControl
    {
        public void ApplyTemplate(TableTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Height = TableTemplate.Height;

            var listTemplate = new TunableListTemplate
            {
                HeaderClickable = template.HeaderClickable,
                CommandMenu = template.CommandMenu
            };

            listTemplate.Columns.AddRange(template.Columns);
            listTemplate.Icons.AddRange(template.Icons);

            listTemplate.SetTemplateInternals(template.TemplateName, template.BaseDirectory);

            ApplyTemplate(listTemplate);
        }

        public void ApplyValue(object value)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            DisplayContent((List<ListItemContent>) value);
        }

        public Action ApplyBehavior(IDictionary<string, IControlHolder> namedControlHolders, BehaviorRule rule)
        {
            if (null == namedControlHolders)
                throw new ArgumentNullException(nameof(namedControlHolders));

            if (null == rule)
                throw new ArgumentNullException(nameof(rule));

            return null;
        }

        public void SetErrorProvider(ErrorProvider errorProvider)
        {
            if (null == errorProvider)
                throw new ArgumentNullException(nameof(errorProvider));

            errorProvider.SetIconPadding(this, -20);
        }

        public bool Validate(IDictionary<string, IControlHolder> namedControlHolders, InspectionRule rule)
        {
            if (null == namedControlHolders)
                throw new ArgumentNullException(nameof(namedControlHolders));

            if (null == rule)
                throw new ArgumentNullException(nameof(rule));

            switch (rule.Type)
            {
                case InspectionType.NotEmpty:
                    return null != CurrentEntity;
                default:
                    throw new BadTemplateException("rule.Type == " + rule.Type);
            }
        }

        public object ObtainValue()
        {
            return CurrentEntity;
        }
    }
}