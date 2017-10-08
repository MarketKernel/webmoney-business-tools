using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.Controls
{
    internal sealed class AccountDropDownList : ComboBox, IAtomControl
    {
        private const string NumberExample = "W000000000000";
        private const float AmountExample = 8000000000F;
        private const string CuttedNameExample = "WWW...";
        private const string SeparatorExample = "_";

        private static readonly Color NegativeAmountColor = Color.Red;
        private static readonly Color PositiveAmountColor = Color.Green;
        private static readonly Color ZeroAmountColor = Color.Blue;

        private int _numberWidth;
        private int _amountWidth;

        [Browsable(false)] // скрывает в редакторе свойств
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // деактивация генерации кода
        public new ComboBoxStyle DropDownStyle
        {
            get => base.DropDownStyle;
            set => throw new NotSupportedException();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DrawMode DrawMode
        {
            get => base.DrawMode;
            set => throw new NotSupportedException();
        }

        public AccountDropDownList()
        {
            base.DropDownStyle = ComboBoxStyle.DropDownList;

            base.DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += OnDrawItem;
        }

        public void ApplyTemplate(AccountDropDownListTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            var index = 0;
            var selectedIndex = -1;

            var items = new List<AccountDropDownListItem>();

            foreach (var itemTemplate in template.Items)
            {
                if (itemTemplate.Selected)
                    selectedIndex = index;

                items.Add(AccountDropDownListItem.FromTemplate(itemTemplate));

                index++;
            }

            BeginUpdate();
            Items.Clear();
            Items.AddRange(items.Select(item => (object)item).ToArray());
            EndUpdate();

            if (-1 != selectedIndex)
                SelectedIndex = selectedIndex;
            else
            {
                if (Items.Count > 0)
                    SelectedIndex = 0;
            }
        }

        public void ApplyValue(object value)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            foreach (var item in Items)
            {
                var account = (AccountDropDownListItem) item;

                if (account.Number.Equals((string) value, StringComparison.Ordinal))
                {
                    SelectedItem = account;
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

            if (null == rule.AffectedControls)
                throw new BadTemplateException("null == rule.AffectedControls");

            if (null == rule.Action)
                throw new BadTemplateException("null == rule.Action");

            if (!"SelectedIndexChanged".Equals(rule.Trigger, StringComparison.Ordinal))
                return null;

            const string updateCurrencyAction = "UpdateCurrency";
            const string updateAvailableAmountAction = "UpdateAvailableAmount";

            if (updateCurrencyAction == rule.Action
                || updateAvailableAmountAction == rule.Action)
            {
                void Update()
                {
                    foreach (var affectedControl in rule.AffectedControls)
                    {
                        if (!namedControlHolders.ContainsKey(affectedControl))
                            continue;

                        var amountNumericUpDown = (AmountNumericUpDown) namedControlHolders[affectedControl].Control;

                        if (null == SelectedItem)
                            continue;

                        var accountDropDownListItemTemplate = (AccountDropDownListItem) SelectedItem;

                        amountNumericUpDown.CurrencyName = accountDropDownListItemTemplate.Currency;

                        if (rule.Action == updateAvailableAmountAction)
                        {
                            amountNumericUpDown.AvailableAmount = accountDropDownListItemTemplate.AvailableAmount;
                            amountNumericUpDown.RecommendedAmount = accountDropDownListItemTemplate.RecommendedAmount;
                        }
                        else
                        {
                            amountNumericUpDown.AvailableAmount = null;
                            amountNumericUpDown.RecommendedAmount = null;
                        }
                    }
                }

                SelectedIndexChanged += (sender, args) =>
                {
                    Update();
                };

                return Update;
            }

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

            return true;
        }

        public object ObtainValue()
        {
            var selectedValue = SelectedItem as AccountDropDownListItem;
            return selectedValue?.Number;
        }

        private void OnDrawItem(object sender, DrawItemEventArgs drawItemEventArgs)
        {
            if (!drawItemEventArgs.State.HasFlag(DrawItemState.Disabled))
                drawItemEventArgs.DrawBackground();

            if (drawItemEventArgs.State.HasFlag(DrawItemState.Focus))
                drawItemEventArgs.DrawFocusRectangle();

            if (-1 == drawItemEventArgs.Index)
                return;

            var account = (AccountDropDownListItem) Items[drawItemEventArgs.Index];

            var bounds = drawItemEventArgs.Bounds;

            var y = bounds.Y; // начальная точка по оси Y совпадает для всех прямоугольников
            var height = bounds.Height; // высота всегда одинакова

            var graphics = drawItemEventArgs.Graphics;
            var font = drawItemEventArgs.Font;
            var color = drawItemEventArgs.ForeColor;

            // Number
            var numberFont = new Font(font, FontStyle.Bold);

            var numberX = bounds.X;

            if (0 == _numberWidth)
                _numberWidth = MeasureString(graphics, font, NumberExample + SeparatorExample);

            if (_numberWidth > bounds.Width)
                _numberWidth = bounds.Width;

            var numberRectangle = new Rectangle(numberX, y, _numberWidth, height);

            using (var solidBrush = new SolidBrush(color))
            {
                graphics.DrawString(account.Number, numberFont, solidBrush, numberRectangle);
            }

            // Amount
            int amountX;

            if (null != account.Amount)
            {
                if (0 == _amountWidth)
                    _amountWidth = MeasureString(graphics, font, SeparatorExample + AmountExample.ToString("0.00"));

                amountX = bounds.Width - _amountWidth;

                if (amountX > numberX + _numberWidth)
                {
                    Color amountColor;

                    if (drawItemEventArgs.State.HasFlag(DrawItemState.Selected))
                        amountColor = color;
                    else if (account.Amount > 0)
                        amountColor = PositiveAmountColor;
                    else if (account.Amount < 0)
                        amountColor = NegativeAmountColor;
                    else
                        amountColor = ZeroAmountColor;

                    var amountStringFormat = new StringFormat
                    {
                        LineAlignment = StringAlignment.Far,
                        Alignment = StringAlignment.Far,
                    };

                    var amountRectangle = new Rectangle(amountX, y, _amountWidth, height);

                    using (var solidBrush = new SolidBrush(amountColor))
                    {
                        graphics.DrawString(account.Amount.Value.ToString("0.00"), font, solidBrush, amountRectangle,
                            amountStringFormat);
                    }
                }
            }
            else
                amountX = bounds.X + bounds.Width;

            // Name

            if (null == account.Name)
                return;

            int nameX = numberX + _numberWidth;

            int availableWidth = amountX - nameX;

            string name = account.Name;
            int nameWidth = MeasureString(graphics, font, name);
            int cuttedNameWidth = MeasureString(graphics, font, CuttedNameExample);

            if (nameWidth <= availableWidth || cuttedNameWidth <= availableWidth)
            {
                bool cutted = false;

                while (nameWidth > availableWidth)
                {
                    if (name.Length < 2)
                        break;

                    name = name.Substring(0, name.Length - 1);
                    cutted = true;

                    nameWidth = MeasureString(graphics, font, $"{name}...");
                }

                if (cutted)
                    name = $"{name}...";

                var nameRectangle = new Rectangle(nameX, y, nameWidth, height);

                using (var solidBrush = new SolidBrush(color))
                {
                    graphics.DrawString(name, font, solidBrush, nameRectangle);
                }
            }
        }

        private int MeasureString(Graphics graphics, Font font, string value)
        {
            return (int) Math.Ceiling(graphics.MeasureString(value, font).Width);
        }
    }
}