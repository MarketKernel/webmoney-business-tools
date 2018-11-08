using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    public sealed class TunableComboBox : ComboBox, IAtomControl
    {
        public void ApplyTemplate(ComboBoxTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            BeginUpdate();

            Items.Clear();

            DropDownStyle = ComboBoxStyle.DropDownList;

            var index = 0;
            var selectedIndex = -1;

            foreach (var itemTemplate in template.Items)
            {
                if (itemTemplate.Selected)
                    selectedIndex = index;

                var comboBoxItem = new ComboBoxItem(itemTemplate.Text, itemTemplate.Value);
                Items.Add(comboBoxItem);

                index++;
            }

            if (-1 != selectedIndex)
                SelectedIndex = selectedIndex;
            else
            {
                if (Items.Count > 0)
                    SelectedIndex = 0;
            }

            EndUpdate();
        }

        public void ApplyValue(object value)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            foreach (var item in Items)
            {
                var itemTemplate = (ComboBoxItem)item;

                if (itemTemplate.Value.Equals(value))
                {
                    SelectedItem = itemTemplate;
                    break;
                }
            }
        }

        public Action ApplyBehavior(IDictionary<string, IControlHolder> namedControlHolders, BehaviorRule rule)
        {
            if (null == namedControlHolders)
                throw new ArgumentNullException(nameof(namedControlHolders));

            if (null == rule)
                throw new ArgumentNullException(nameof(rule));

            if (null == rule.Trigger)
                throw new BadTemplateException("null == rule.Trigger");

            if (null == rule.ActivationСondition)
                throw new BadTemplateException("null == rule.ActivationСondition");

            if (null == rule.AffectedControls)
                throw new BadTemplateException("null == rule.AffectedControls");

            if (null == rule.Action)
                throw new BadTemplateException("null == rule.Action");

            if (!"SelectedIndexChanged".Equals(rule.Trigger, StringComparison.Ordinal))
                return null;

            switch (rule.Action)
            {
                case "SetVisibility":
                {
                    void SetVisibility()
                    {
                        var selectedItem = SelectedItem as ComboBoxItem;

                        if (null == selectedItem)
                            return;

                        if (!rule.ActivationСondition.Equals((string)selectedItem.Value, StringComparison.Ordinal))
                            return;

                        var visibility = bool.Parse(rule.ActionParameter);

                        var parents = new List<Control>();

                        foreach (var affectedControl in rule.AffectedControls)
                        {
                            if (!namedControlHolders.TryGetValue(affectedControl, out var namedControlHolder))
                                continue;

                            var parent = namedControlHolder.Control.Parent;

                            if (!parents.Contains(parent))
                            {
                                parents.Add(parent);
                                parent.SuspendLayout();
                            }

                            if (null != namedControlHolder.Label)
                                namedControlHolder.Label.Visible = visibility;

                            namedControlHolder.Control.Visible = visibility;
                        }

                        foreach (var parent in parents)
                        {
                            parent.ResumeLayout();

                            if (ApplicationUtility.IsRunningOnMono)
                            {
                                var submitForm = parent.FindForm() as SubmitForm;
                                submitForm?.UpdateSize();
                            }
                        }
                    }

                    SelectedIndexChanged += (sender, args) =>
                    {
                        SetVisibility();
                    };

                    return SetVisibility;
                }
                default:
                    throw new BadTemplateException("rule.Type == " + rule.Action);
            }
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
                    if (-1 == SelectedIndex)
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
            var selectedItem = SelectedItem as ComboBoxItem;
            return selectedItem?.Value;
        }
    }
}