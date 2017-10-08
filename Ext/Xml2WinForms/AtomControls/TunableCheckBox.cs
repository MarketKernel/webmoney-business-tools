using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    public sealed class TunableCheckBox : CheckBox, IAtomControl
    {
        public void ApplyTemplate(CheckBoxTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Text = template.Desc;
            Height = CheckBoxTemplate.Height;

            Checked = template.DefaultValue;
        }

        public void ApplyValue(object value)
        {
            Checked = (bool?)value ?? throw new ArgumentNullException(nameof(value));
        }

        public Action ApplyBehavior(IDictionary<string, IControlHolder> namedControlHolders, BehaviorRule rule)
        {
            if (null == namedControlHolders)
                throw new ArgumentNullException(nameof(namedControlHolders));

            if (null == rule)
                throw new ArgumentNullException(nameof(rule));

            if (null == rule.Trigger)
                throw new BadTemplateException("null == rule.Trigger");

            if (null == rule.AffectedControls)
                throw new BadTemplateException("null == rule.AffectedControls");

            if (null == rule.Action)
                throw new BadTemplateException("null == rule.Action");

            if (!"CheckedChanged".Equals(rule.Trigger, StringComparison.Ordinal))
                return null;

            Action action;

            switch (rule.Action)
            {
                case "Enable":

                    action = () =>
                    {
                        foreach (var affectedControl in rule.AffectedControls)
                        {
                            IControlHolder namedControlHolder;

                            if (!namedControlHolders.TryGetValue(affectedControl, out namedControlHolder))
                                return;

                            namedControlHolder.Control.Enabled = Checked;
                        }
                    };

                    break;
                case "Disable":

                    action = () =>
                    {
                        foreach (var affectedControl in rule.AffectedControls)
                        {
                            IControlHolder namedControlHolder;

                            if (!namedControlHolders.TryGetValue(affectedControl, out namedControlHolder))
                                return;

                            namedControlHolder.Control.Enabled = !Checked;
                        }
                    };

                    break;
                default:
                    throw new BadTemplateException("rule.Type == " + rule.Action);
            }

            CheckedChanged += (sender, args) =>
            {
                action();
            };

            return action;
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
                {
                    if (!Checked)
                        return false;
                }
                    break;
                default:
                    throw new BadTemplateException("rule.Type == " + rule.Type);
            }

            return true;
        }

        public object ObtainValue()
        {
            return Checked;
        }
    }
}
