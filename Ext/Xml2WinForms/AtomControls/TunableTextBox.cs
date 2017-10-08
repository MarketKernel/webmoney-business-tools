using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    public sealed class TunableTextBox : TextBox, IAtomControl
    {
        const int OnlyNumbersStyle = 0x2000;
        const int PasteMessageCode = 0x0302;
        const string NumberTemplate = @"^\d+$";

        protected override CreateParams CreateParams
        {
            get
            {
                if (DigitsOnly)
                {
                    CreateParams parameters = base.CreateParams;
                    parameters.Style |= OnlyNumbersStyle;
                    return parameters;
                }

                return base.CreateParams;
            }
        }

        [Category("Behavior"), Description("Indicates whether only digits are accepted as valid input.")]
        public bool DigitsOnly { get; set; }

        protected override void WndProc(ref Message m)
        {
            if (DigitsOnly && PasteMessageCode == m.Msg)
            {
                var dataObject = Clipboard.GetDataObject();

                var data = dataObject?.GetData(DataFormats.Text) as string;

                if (data == null)
                    return;

                if (!Regex.IsMatch(data, NumberTemplate))
                    return;
            }

            base.WndProc(ref m);
        }

        public void ApplyTemplate(TextBoxTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Text = template.DefaultValue;
            ReadOnly = template.ReadOnly;
            MaxLength = template.MaxLength;

            if (template.Multiline)
            {
                Multiline = true;
                AcceptsReturn = true;
                Height = TextBoxTemplate.MultilineTextBoxHeight;
            }

            if (template.DigitsOnly)
                DigitsOnly = true;

            if (template.UseSystemPasswordChar)
                UseSystemPasswordChar = true;
        }

        public void ApplyValue(object value)
        {
            Text = (string)value ?? throw new ArgumentNullException(nameof(value));
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
                {
                    if (string.IsNullOrEmpty(Text))
                        return false;
                }
                    break;
                case InspectionType.LengthEqual:
                {
                    if (Text.Length != rule.Number)
                        return false;
                }
                    break;
                case InspectionType.RegExp:
                {
                    var r = new Regex(rule.Argument);

                    if (!r.IsMatch(Text))
                        return false;
                }
                    break;

                case InspectionType.EqualsToControl:
                {
                    IControlHolder controlHolder;

                    if (!namedControlHolders.TryGetValue(rule.Argument, out controlHolder))
                        return false;

                    var text = controlHolder.ObtainValue() as string;

                    if (!Text.Equals(text, StringComparison.Ordinal))
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
            return Text;
        }
    }
}
