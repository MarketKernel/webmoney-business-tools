using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LocalizationAssistant;
using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.Controls
{
    internal sealed partial class AmountNumericUpDown : UserControl, IAtomControl
    {
        private decimal? _recommendedAmount;

        [Category("Appearance"), Description("The amount entered by the user in the control.")]
        public decimal Amount
        {
            get => mNumericUpDown.Value;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                mNumericUpDown.Value = value;
            }
        }

        [Category("Appearance"), Description("Indicates the currency in which the amount is entered.")]
        public string CurrencyName
        {
            get { return currencyLabel.Text; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    currencyLabel.Visible = false;
                else
                {
                    currencyLabel.Visible = true;
                    currencyLabel.Text = value;
                }
            }
        }

        [Category("Appearance"), Description("Indicates the maximum amount that the user can enter.")]
        public decimal? AvailableAmount
        {
            get => mNumericUpDown.Maximum;
            set
            {
                if (null == value)
                    mNumericUpDown.Maximum = decimal.MaxValue;
                else
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException(nameof(value));

                    mNumericUpDown.Maximum = value.Value;
                }
            }
        }

        [Category("Appearance"), Description("Indicates the maximum amount that the user can enter.")]
        public decimal? RecommendedAmount
        {
            get => _recommendedAmount;
            set
            {
                if (null == value)
                {
                    _recommendedAmount = null;
                    availableAmountLinkLabel.Visible = false;
                }
                else
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException(nameof(value));

                    _recommendedAmount = value;
                    availableAmountLinkLabel.Visible = true;

                    string localizedText = Translator.Instance.Translate("wmbt-extensions", "<< max.");
                    availableAmountLinkLabel.Text = $@"{localizedText} {_recommendedAmount.Value:0.00}";
                }
            }
        }

        [Category("Behavior"), Description("Controls whether the text in edit control can be changed or not.")]
        public bool ReadOnly
        {
            get => mNumericUpDown.ReadOnly;
            set
            {
                mNumericUpDown.ReadOnly = value;
                mNumericUpDown.Increment = value ? 0 : 1;
                availableAmountLinkLabel.Enabled = !value;
            }
        }

        public AmountNumericUpDown()
        {
            InitializeComponent();

            AvailableAmount = decimal.MaxValue;
            RecommendedAmount = null;
        }

        public void ApplyTemplate(AmountNumericUpDownTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            CurrencyName = template.CurrencyName;
            mNumericUpDown.Value = template.DefaultValue;
            AvailableAmount = template.AvailableAmount;
            ReadOnly = template.ReadOnly;
        }

        private void availableAmountLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (null != _recommendedAmount)
                mNumericUpDown.Value = _recommendedAmount.Value;
        }

        public void ApplyValue(object value)
        {
            Amount = (decimal?) value ?? throw new ArgumentNullException(nameof(value));
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

            int padding = -20;

            errorProvider.SetIconPadding(this, padding);
            errorProvider.SetIconAlignment(this, ErrorIconAlignment.MiddleLeft);
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
                    return Math.Round(Amount * 100) > rule.Number;
                }
            }

            return true;
        }

        public object ObtainValue()
        {
            return Amount;
        }
    }
}