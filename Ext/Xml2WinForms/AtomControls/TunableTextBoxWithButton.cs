using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    public sealed partial class TunableTextBoxWithButton : UserControl, IServiceControl, IAtomControl
    {
        [Category("Appearance"), Description("The text associated with the control.")]
        public override string Text
        {
            get => mExtendedTextBox.Text;
            set => mExtendedTextBox.Text = value ?? string.Empty;
        }

        [Category("Behavior"), Description("Controls whether the text in edit control can be changed or not.")]
        public bool ReadOnly
        {
            get => mExtendedTextBox.ReadOnly;
            set => mExtendedTextBox.ReadOnly = value;
        }

        [Category("Behavior"),
         Description("Specifies the maximum number of characters that can be entered into the edit control.")]
        public int MaxLength
        {
            get => mExtendedTextBox.MaxLength;
            set => mExtendedTextBox.MaxLength = value;
        }

        [Category("Behavior"), Description("Indicates whether only digits are accepted as valid input.")]
        public bool DigitsOnly
        {
            get => mExtendedTextBox.DigitsOnly;
            set => mExtendedTextBox.DigitsOnly = value;
        }

        [Category("Appearance"), Description("The image that will be displayed on the button.")]
        public Image ButtonIcon
        {
            get => mButton.BackgroundImage;
            set => mButton.BackgroundImage = value;
        }

        [Category("Action"), Description("Service command.")]
        public event EventHandler<CommandEventArgs> ServiceCommand;

        public TunableTextBoxWithButton()
        {
            InitializeComponent();
        }

        public void ApplyTemplate(TextBoxWithButtonTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Text = template.DefaultValue;
            MaxLength = template.MaxLength;
            ReadOnly = template.ReadOnly;
            DigitsOnly = template.DigitsOnly;

            IconHolder iconHolder = null;

            if (null != template.IconBytes)
                iconHolder = new IconHolder(template.IconBytes);
            if (null != template.IconPath)
                iconHolder = new IconHolder(template.BaseDirectory, template.IconPath);

            ButtonIcon = iconHolder?.TryBuildImage();
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

            if (null == rule.Trigger)
                throw new BadTemplateException("null == rule.Trigger");

            if (null == rule.Action)
                throw new BadTemplateException("null == rule.Action");

            if (!"ButtonClick".Equals(rule.Trigger, StringComparison.Ordinal))
                return null;

            mButton.Click += (sender, args) =>
            {
                if ("SelectFile".Equals(rule.Action, StringComparison.Ordinal) ||
                    "CreateFile".Equals(rule.Action, StringComparison.Ordinal))
                {
                    FileDialog fileDialog;

                    if ("SelectFile".Equals(rule.Action, StringComparison.Ordinal))
                    {
                        fileDialog = new OpenFileDialog
                        {
                            Multiselect = false,
                            CheckFileExists = rule.AdditionalParameters.Contains("CheckFileExists")
                        };

                    }
                    else
                        fileDialog = new SaveFileDialog();

                    fileDialog.RestoreDirectory = true;

                    if (null != rule.ActionParameter)
                        fileDialog.Filter = rule.ActionParameter;

                    var form = mExtendedTextBox.FindForm();

                    if (DialogResult.OK == fileDialog.ShowDialog(form))
                        mExtendedTextBox.Text = fileDialog.FileName;

                    return;
                }

                if ("Copy".Equals(rule.Action))
                {
                    Clipboard.SetText(mExtendedTextBox.Text, TextDataFormat.UnicodeText);
                    return;
                }

                ServiceCommand?.Invoke(this,
                    new CommandEventArgs {Command = rule.Action, Argument = Text});
            };

            return null;
        }

        public void SetErrorProvider(ErrorProvider errorProvider)
        {
            if (null == errorProvider)
                throw new ArgumentNullException(nameof(errorProvider));

            int padding = -20;
            padding -= mButton.Margin.Left + mButton.Width + mButton.Margin.Right;

            errorProvider.SetIconPadding(this, padding);
        }

        public bool Validate(IDictionary<string, IControlHolder> namedControlHolders, InspectionRule rule)
        {
            if (null == namedControlHolders)
                throw new ArgumentNullException(nameof(namedControlHolders));

            if (null == rule)
                throw new ArgumentNullException(nameof(rule));

            return mExtendedTextBox.Validate(namedControlHolders, rule);
        }

        public object ObtainValue()
        {
            return mExtendedTextBox.Text;
        }
    }
}
