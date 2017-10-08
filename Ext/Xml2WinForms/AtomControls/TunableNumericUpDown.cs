using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    public sealed class TunableNumericUpDown : NumericUpDown, IAtomControl
    {
        public void ApplyTemplate(NumericUpDownTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Value = template.DefaultValue;
            Minimum = template.MinValue;
            Maximum = template.MaxValue;
            ReadOnly = template.ReadOnly;
            DecimalPlaces = template.DecimalPlaces;
            TextAlign = template.Alignment;

            if (template.ReadOnly)
            {
                Increment = 0;
                ReadOnly = true;
            }
            else
            {
                Increment = 1;
                ReadOnly = false;
            }
        }

        public void ApplyValue(object value)
        {
            Value = (decimal?)value ?? throw new ArgumentNullException(nameof(value));
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

            errorProvider.SetIconPadding(this, -35);
        }

        public bool Validate(IDictionary<string, IControlHolder> namedControlHolders, InspectionRule rule)
        {
            if (null == namedControlHolders)
                throw new ArgumentNullException(nameof(namedControlHolders));

            if (null == rule)
                throw new ArgumentNullException(nameof(rule));

            switch (rule.Type)
            {
                case InspectionType.ValueMoreThen:
                {
                    if (Value <= rule.Number)
                        return false;
                }
                    break;
                case InspectionType.ValueLessThen:
                {
                    if (Value >= rule.Number)
                        return false;
                }
                    break;
                case InspectionType.NotEmpty:

                    if (string.IsNullOrEmpty(Text))
                        return false;

                    break;
                default:
                    throw new BadTemplateException("rule.Type == " + rule.Type);
            }

            return true;
        }

        public object ObtainValue()
        {
            return Value;
        }
    }
}
