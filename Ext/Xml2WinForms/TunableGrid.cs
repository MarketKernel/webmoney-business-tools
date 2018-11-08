using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Xml2WinForms.Templates;
using LocalizationAssistant;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    public delegate Tuple<string, object> ParseCellValueDelegate(string value, string columnName);

    public sealed partial class TunableGrid : UserControl
    {
        const int FirstPage = 1;

        private static readonly Color LinkColor = Color.FromArgb(0, 0, 255);
        private static readonly Color ActiveLinkColor = Color.FromArgb(0, 0, 255);
        private static readonly Color VisitedLinkColor = Color.FromArgb(0, 0, 255);

        private static readonly Color WarningColor = Color.FromArgb(255, 188, 170);
        private static readonly Color WarningSelectionColor = Color.FromArgb(230, 171, 155);

        private readonly TunableMenu _commandMenu;

        private string _templateName;

        private List<GridRowContent> _rowContentList;
        private Dictionary<string, GridRowContent> _rowContentDictionary;

        private string _directionColumnName;
        private SortOrder _sortDirection;

        private int _pageCount;

        [Category("Behavior"), Description("Indicates whether the user can edit the cells of the TunableGrid control.")]
        public bool ReadOnly
        {
            get => mDataGridView.ReadOnly;
            set
            {
                ((ISupportInitialize)mDataGridView).BeginInit();

                if (value)
                {
                    mDataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
                    mDataGridView.AllowUserToAddRows = false;
                    mDataGridView.AllowUserToDeleteRows = false;
                    mDataGridView.ReadOnly = true;

                    mDataGridView.ColumnHeadersVisible = false;
                    mDataGridView.RowHeadersVisible = false;
                }
                else
                {
                    mDataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
                    mDataGridView.AllowUserToAddRows = true;
                    mDataGridView.AllowUserToDeleteRows = true;
                    mDataGridView.ReadOnly = false;

                    mDataGridView.ColumnHeadersVisible = true;
                    mDataGridView.RowHeadersVisible = true;
                }

                ((ISupportInitialize)mDataGridView).EndInit();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object CurrentEntity { get; private set; }

        [Category("Pagination"), Description("Indicates the page number on which the user is at the moment.")]
        public int PageCount
        {
            get => _pageCount;
            set
            {
                _pageCount = value;

                if (_pageCount <= FirstPage)
                {
                    _pageCount = FirstPage;
                    navigatorToolStrip.Visible = false;
                    return;
                }

                navigatorToolStrip.Visible = true;

                positionComboBox.BeginUpdate(); //

                positionComboBox.Items.Clear();
                CurrentPage = FirstPage;

                for (var i = FirstPage; i <= _pageCount; i++)
                {
                    positionComboBox.Items.Add(i);
                }

                positionComboBox.EndUpdate(); // -

                countLabel.Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}",
                    Translator.Instance.Translate("xml2winforms", "of"), _pageCount);

                UpdatePager();
            }
        }

        [Category("Pagination"), Description("Indicates the current page.")]
        public int CurrentPage { get; private set; }

        [Category("Action"), Description("Occurs when the page number has changed.")]
        public event EventHandler<NavigatorEventArgs> PageNumberChanged;

        [Category("Action"), Description("Occurs when the value of a cell has changed.")]
        public event EventHandler<ValueChangedEventArgs> CellValueChanged;

        [Category("Action"), Description("Service command.")]
        public event EventHandler<CommandEventArgs> ServiceCommand;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MenuItemResolverDelegate MenuItemResolver { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ParseCellValueDelegate ParseCellValue { get; set; }

        public TunableGrid()
        {
            InitializeComponent();

            positionComboBox.Items.Add(1);
            positionComboBox.SelectedIndex = 0;

            if (null == components)
                components = new Container();

            var commandMenu = new TunableMenu(components)
            {
                MenuItemResolver = (entity, command) => null == MenuItemResolver ||
                                                        MenuItemResolver(CurrentEntity, command)
            };

            commandMenu.ServiceCommand += (sender, args) =>
            {
                if (null == CurrentEntity)
                    return;

                ServiceCommand?.Invoke(this,
                    new CommandEventArgs { Command = args.Command, Argument = CurrentEntity });
            };

            _commandMenu = commandMenu;
        }

        /*********************************************************************************************/
        // НАЧАЛО -- Внешние методы
        /*********************************************************************************************/

        public void ApplyTemplate(TunableGridTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Reset();

            _templateName = template.TemplateName;

            if (ReadOnly != template.ReadOnly)
                ReadOnly = template.ReadOnly;

            ((ISupportInitialize)mDataGridView).BeginInit();
            mDataGridView.Enabled = false;

            // Column Header Cell Style
            if (null != template.ColumnHeaderDefaultCellStyle)
                mDataGridView.ColumnHeadersDefaultCellStyle = SelectCellStyle(template.ColumnHeaderDefaultCellStyle);

            // Row Header Cell Style
            if (null != template.RowHeaderDefaultCellStyle)
                mDataGridView.RowHeadersDefaultCellStyle = SelectCellStyle(template.RowHeaderDefaultCellStyle);

            // Rows Cell Style
            if (null != template.RowsDefaultCellStyle)
                mDataGridView.RowsDefaultCellStyle = SelectCellStyle(template.RowsDefaultCellStyle);

            // Grid Color
            if (null != template.GridColor)
                mDataGridView.GridColor = DisplayContentHelper.SelectColor(template.GridColor);

            AddColumns(template.Columns);

            if (null != template.CommandMenu)
            {
                _commandMenu.ApplyTemplate(template.CommandMenu);
                mDataGridView.ContextMenuStrip = _commandMenu;
            }

            mDataGridView.Enabled = true;
            ((ISupportInitialize)mDataGridView).EndInit();
        }

        public void DisplayContent(List<GridRowContent> rowContentList, bool clear = true, bool embodyColumnsFromMetadata = false)
        {
            if (null == rowContentList)
                throw new ArgumentNullException(nameof(rowContentList));

            mDataGridView.Enabled = false;

            if (embodyColumnsFromMetadata)
            {
                mDataGridView.Columns.Clear();

                if (rowContentList.Count > 0)
                    AddColumnsFromMetadata(rowContentList[0].ContentItem);
            }

            if (clear)
                ClearContent();

            _rowContentList = rowContentList;
            _rowContentDictionary = rowContentList.ToDictionary(rc => rc.Key);

            bool exists = false;

            var rows = new List<DataGridViewRow>();

            foreach (var rowContent in rowContentList)
            {
                var row = new DataGridViewRow();
                rowContent.RowReference = row;

                ApplyRowContent(rowContent);

                rows.Add(row);
                exists = true;
            }

            if (exists)
            {
                mDataGridView.Rows.AddRange(rows.ToArray());

                if (ReadOnly)
                    mDataGridView.ColumnHeadersVisible = true;
            }

            // SortGlyphDirection
            foreach (DataGridViewColumn column in mDataGridView.Columns)
            {
                if (DataGridViewColumnSortMode.Programmatic != column.SortMode)
                    continue;

                if (null != _directionColumnName &&
                    _directionColumnName.Equals(column.Name, StringComparison.OrdinalIgnoreCase))
                    column.HeaderCell.SortGlyphDirection = _sortDirection;
                else
                    column.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            mDataGridView.Enabled = true;
        }

        public void UpdateRow(GridRowContent rowContent)
        {
            if (null == rowContent)
                throw new ArgumentNullException(nameof(rowContent));

            if (mDataGridView.InvokeRequired)
            {
                mDataGridView.Invoke(new Action(() =>
                {
                    UpdateRow(rowContent);
                }));

                return;
            }

            GridRowContent currentRowContent;

            if (!_rowContentDictionary.TryGetValue(rowContent.Key, out currentRowContent))
                return;

            rowContent.CopyTo(currentRowContent);

            ApplyRowContent(currentRowContent);
        }

        public void Reset()
        {
            if (!ReadOnly)
                ReadOnly = true;

            ((ISupportInitialize)mDataGridView).BeginInit();
            mDataGridView.Enabled = false;

            mDataGridView.Columns.Clear();
            mDataGridView.Rows.Clear();

            mDataGridView.Enabled = true;
            ((ISupportInitialize)mDataGridView).EndInit();

            _commandMenu.Reset();
            mDataGridView.ContextMenuStrip = null;

            PageCount = 1;
        }

        public void ClearContent()
        {
            _rowContentList?.Clear();
            _rowContentDictionary?.Clear();

            mDataGridView.Rows.Clear();

            if (ReadOnly)
                mDataGridView.ColumnHeadersVisible = false;
        }

        public List<TResult> MapRows<TResult>()
            where TResult : class
        {
            if (ReadOnly)
                throw new InvalidOperationException("ReadOnly");

            var valueList = new List<TResult>();

            foreach (DataGridViewRow row in mDataGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                var orderEntity = MapRow<TResult>(row);

                valueList.Add(orderEntity);
            }

            return valueList;
        }

        public List<object> SelectContentItems()
        {
            if (null == _rowContentList)
                return new List<object>();

            return _rowContentList.Select(rt => rt.ContentItem).ToList();
        }

        public List<GridColumnSettings> SelectGridSettings()
        {
            var gridColumnSettingses = new List<GridColumnSettings>();

            foreach (DataGridViewColumn column in mDataGridView.Columns.Cast<DataGridViewColumn>()
                .OrderBy(c => (int)c.Tag))
            {
                gridColumnSettingses.Add(
                    new GridColumnSettings { DisplayIndex = column.DisplayIndex, Width = column.Width });
            }

            return gridColumnSettingses;
        }

        /*********************************************************************************************/
        // Внешние методы -- КОНЕЦ
        /*********************************************************************************************/

        /*********************************************************************************************/
        // НАЧАЛО -- DataGridView события
        /*********************************************************************************************/

        private void mDataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            var dataGridView = sender as DataGridView;

            if (null == dataGridView)
                return;

            var hitTestInfo = dataGridView.HitTest(e.X, e.Y);

            if (hitTestInfo.Type == DataGridViewHitTestType.Cell)
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                    row.Selected = false;

                var currentRow = dataGridView.Rows[hitTestInfo.RowIndex];
                currentRow.Selected = true;

                CurrentEntity = ReadOnly ? currentRow.Tag : GetEntityAsDictionary(currentRow);
            }
            else
                CurrentEntity = null;
        }

        private void mDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (-1 == e.RowIndex)
                return;

            var name = mDataGridView.Columns[e.ColumnIndex].Name;

            var commandText = "CellContentClick:" + name;
            ServiceCommand?.Invoke(this, new CommandEventArgs { Command = commandText });
        }

        private void mDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (-1 == e.RowIndex)
                return;

            if (!mDataGridView.ReadOnly)
                SendKeys.Send("{F2}");

            if (null == CurrentEntity)
                return;

            string commandText = "CellMouseDoubleClick:" + mDataGridView.Columns[e.ColumnIndex].Name;
            ServiceCommand?.Invoke(this, new CommandEventArgs { Command = commandText, Argument = CurrentEntity });
        }

        private void mDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var dataGridViewColumn = mDataGridView.Columns[e.ColumnIndex];

            if (DataGridViewColumnSortMode.Programmatic != dataGridViewColumn?.SortMode)
                return;

            _directionColumnName = dataGridViewColumn.Name;

            if (SortOrder.None == dataGridViewColumn.HeaderCell.SortGlyphDirection)
                _sortDirection = SortOrder.Ascending;
            else
                _sortDirection = SortOrder.Ascending == dataGridViewColumn.HeaderCell.SortGlyphDirection
                                     ? SortOrder.Descending
                                     : SortOrder.Ascending;

            var list = _rowContentList;

            object KeySelector(GridRowContent rowTemplate)
            {
                var contentItem = rowTemplate.ContentItem;

                var propertyInfo = contentItem.GetType()
                    .GetProperty(_directionColumnName, BindingFlags.Instance | BindingFlags.Public);

                if (null == propertyInfo)
                    throw new InvalidOperationException("null == propertyInfo");

                var value = propertyInfo.GetValue(contentItem, null);

                return value;
            }

            switch (_sortDirection)
            {
                case SortOrder.Ascending:
                    list = list.OrderBy(KeySelector).ToList();
                    break;
                case SortOrder.Descending:
                    list = list.OrderByDescending(KeySelector).ToList();
                    break;
            }

            DisplayContent(list);
        }

        private void mDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (-1 == e.RowIndex || -1 == e.ColumnIndex || mDataGridView.Rows[e.RowIndex].IsNewRow)
                return;

            mDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor =
                mDataGridView.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor;
            mDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                mDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor;
        }

        private void mDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (-1 == e.RowIndex || -1 == e.ColumnIndex || mDataGridView.Rows[e.RowIndex].IsNewRow)
                return;

            if (null != ParseCellValue)
            {

                foreach (DataGridViewColumn column in mDataGridView.Columns)
                {
                    Tuple<string, object> result = ParseCellValue((string)mDataGridView.Rows[e.RowIndex].Cells[column.Name].Value,
                                                   column.Name);

                    if (null == result)
                    {
                        mDataGridView.Rows[e.RowIndex].Cells[column.Name].Value = string.Empty;
                        mDataGridView.Rows[e.RowIndex].Cells[column.Name].Tag = null;

                        mDataGridView.Rows[e.RowIndex].Cells[column.Name].Style.SelectionBackColor = WarningSelectionColor;
                        mDataGridView.Rows[e.RowIndex].Cells[column.Name].Style.BackColor = WarningColor;
                    }
                    else
                    {
                        mDataGridView.Rows[e.RowIndex].Cells[column.Name].Value = result.Item1;
                        mDataGridView.Rows[e.RowIndex].Cells[column.Name].Tag = result.Item2;
                    }
                }
            }

            CellValueChanged?.Invoke(null, new ValueChangedEventArgs(mDataGridView.Columns[e.ColumnIndex].Name));
        }

        private void mDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (mDataGridView.ReadOnly)
                return;

            CellValueChanged?.Invoke(null, new ValueChangedEventArgs(null));
        }

        /*********************************************************************************************/
        // DataGridView события -- КОНЕЦ
        /*********************************************************************************************/

        /*********************************************************************************************/
        // НАЧАЛО -- Navigator
        /*********************************************************************************************/

        private void UpdatePager()
        {
            if (CurrentPage <= FirstPage)
            {
                moveFirstButton.Enabled = false;
                movePreviousButton.Enabled = false;
            }
            else
            {
                moveFirstButton.Enabled = true;
                movePreviousButton.Enabled = true;
            }

            if (CurrentPage >= _pageCount)
            {
                moveNextButton.Enabled = false;
                moveLastButton.Enabled = false;
            }
            else
            {
                moveNextButton.Enabled = true;
                moveLastButton.Enabled = true;
            }

            PageNumberChanged?.Invoke(this, new NavigatorEventArgs { CurrentPage = CurrentPage });
        }

        private void moveFirstButton_Click(object sender, EventArgs e)
        {
            CurrentPage = FirstPage;
            positionComboBox.SelectedIndex = CurrentPage - 1;
        }

        private void movePreviousButton_Click(object sender, EventArgs e)
        {
            if (CurrentPage > FirstPage)
            {
                CurrentPage--;
                positionComboBox.SelectedIndex = CurrentPage - 1;
            }
        }

        private void positionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = (int)positionComboBox.SelectedItem;
            UpdatePager();
        }

        private void moveNextButton_Click(object sender, EventArgs e)
        {
            if (CurrentPage < _pageCount)
            {
                CurrentPage++;
                positionComboBox.SelectedIndex = CurrentPage - 1;
            }
        }

        private void moveLastButton_Click(object sender, EventArgs e)
        {
            CurrentPage = _pageCount;
            positionComboBox.SelectedIndex = CurrentPage - 1;
        }

        /*********************************************************************************************/
        // Navigator -- КОНЕЦ
        /*********************************************************************************************/

        /*********************************************************************************************/
        // Вспомогательные методы
        /*********************************************************************************************/

        private void AddColumns(List<GridColumnTemplate> templates)
        {
            var columns = new List<DataGridViewColumn>();
            int index = 0;

            foreach (var template in templates)
            {
                var column = BuildColumn(template, index++);
                columns.Add(column);
            }

            mDataGridView.Columns.AddRange(columns.ToArray());
        }

        private void AddColumnsFromMetadata(object contentItem)
        {
            var columns = new List<DataGridViewColumn>();
            int index = 0;

            foreach (var propertyInfo in contentItem.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(TunableGridColumnAttribute), false);

                if (1 != attributes.Length)
                    continue;

                var presentationAttribute = (TunableGridColumnAttribute)attributes[0];

                if (string.IsNullOrEmpty(presentationAttribute.Name))
                    presentationAttribute.Name = propertyInfo.Name;

                var template = new GridColumnTemplate
                {
                    Name = presentationAttribute.Name,
                    Kind = presentationAttribute.Kind,
                    HeaderText = presentationAttribute.HeaderText,
                    MaxInputLength = presentationAttribute.MaxInputLength,
                    Fill = presentationAttribute.Fill,
                    Width = presentationAttribute.Width,
                    MinimumWidth = presentationAttribute.MinimumWidth,
                    Sortable = presentationAttribute.Sortable,
                    Order = presentationAttribute.Order,
                    SortGlyphDirection = presentationAttribute.SortGlyphDirection,
                    Visible = presentationAttribute.Visible
                };

                var column = BuildColumn(template, index++);
                columns.Add(column);
            }

            mDataGridView.Columns.AddRange(columns.ToArray());
        }

        private DataGridViewColumn BuildColumn(GridColumnTemplate template, int index)
        {
            DataGridViewColumn column;

            switch (template.Kind)
            {
                case ColumnKind.TextBox:
                    column = new DataGridViewTextBoxColumn();
                    break;
                case ColumnKind.Link:
                    column = new DataGridViewLinkColumn();
                    break;
                default:
                    throw new BadTemplateException("template.Kind == " + template.Kind);
            }

            if (null == template.Name)
                throw new BadTemplateException("null == template.Name");

            column.Name = template.Name;
            column.HeaderText = template.HeaderText;
            column.Tag = index;

            if (template.Order >= 0)
                column.DisplayIndex = template.Order;

            if (template.Fill)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            var textBoxColumn = column as DataGridViewTextBoxColumn;

            if (textBoxColumn != null)
                textBoxColumn.MaxInputLength = template.MaxInputLength;

            column.Width = template.Width;
            column.MinimumWidth = template.MinimumWidth;
            column.SortMode = template.Sortable
                                  ? DataGridViewColumnSortMode.Programmatic
                                  : DataGridViewColumnSortMode.NotSortable;

            if (template.Sortable && SortOrder.None != template.SortGlyphDirection)
            {
                _directionColumnName = template.Name;
                _sortDirection = template.SortGlyphDirection;
            }

            column.Visible = template.Visible;

            return column;
        }

        private void ApplyRowContent(GridRowContent rowContent)
        {
            var row = rowContent.RowReference;
            object contentItem = rowContent.ContentItem;

            row.Tag = contentItem;

            var cells = new List<DataGridViewCell>();

            bool hasCells = row.Cells.Count > 0;

            foreach (DataGridViewColumn column in mDataGridView.Columns)
            {
                var cell = hasCells ? row.Cells[column.Name] : BuildCell(column);

                if (null == cell)
                    continue;

                var propertyInfo = contentItem.GetType().GetProperty(column.Name);

                if (null == propertyInfo)
                    throw new InvalidOperationException($"null == propertyInfo [{column.Name}]");

                var value = propertyInfo.GetValue(contentItem, null);

                if (null != value)
                {
                    cell.Value = DisplayContentHelper.GetText(propertyInfo, value);
                    cell.Tag = value;
                }

                if (!hasCells)
                    cells.Add(cell);
            }

            if (!hasCells)
                row.Cells.AddRange(cells.ToArray());

            if (null != rowContent.ForeColor)
                row.DefaultCellStyle.ForeColor = rowContent.ForeColor.Value;

            if (null != rowContent.BackColor)
                row.DefaultCellStyle.BackColor = rowContent.BackColor.Value;

            if (null != rowContent.SelectionForeColor)
                row.DefaultCellStyle.SelectionForeColor = rowContent.SelectionForeColor.Value;

            if (null != rowContent.SelectionBackColor)
                row.DefaultCellStyle.SelectionBackColor = rowContent.SelectionBackColor.Value;

            if (rowContent.Bold || rowContent.Strikeout)
            {
                var fontStyle = FontStyle.Regular;

                if (rowContent.Bold)
                    fontStyle |= FontStyle.Bold;

                if (rowContent.Strikeout)
                    fontStyle |= FontStyle.Strikeout;

                var font = row.DefaultCellStyle.Font ?? mDataGridView.Font;
                row.DefaultCellStyle.Font = new Font(font, fontStyle);
            }
        }

        private static DataGridViewCell BuildCell(DataGridViewColumn column)
        {
            if (column is DataGridViewLinkColumn)
            {
                return new DataGridViewLinkCell
                {
                    LinkBehavior = LinkBehavior.NeverUnderline,
                    LinkColor = LinkColor,
                    ActiveLinkColor = ActiveLinkColor,
                    VisitedLinkColor = VisitedLinkColor
                };
            }

            if (column is DataGridViewTextBoxColumn)
                return new DataGridViewTextBoxCell();

            return null;
        }

        private static DataGridViewCellStyle SelectCellStyle(CellStyleTemplate template)
        {
            var cellStyle = new DataGridViewCellStyle();

            // - Font
            if (null != template.Font)
            {
                var fontFamily = template.Font.FontName;
                var emSize = template.Font.FontSize;

                var fontStyle = template.Font.IsBold
                    ? FontStyle.Bold
                    : FontStyle.Regular;

                var font = new Font(fontFamily, emSize, fontStyle);
                cellStyle.Font = font;
            }

            // -Color
            if (null != template.BackColor)
                cellStyle.BackColor = DisplayContentHelper.SelectColor(template.BackColor);

            if (null != template.ForeColor)
                cellStyle.ForeColor = DisplayContentHelper.SelectColor(template.ForeColor);

            if (null != template.SelectionBackColor)
                cellStyle.SelectionBackColor = DisplayContentHelper.SelectColor(template.SelectionBackColor);

            if (null != template.SelectionForeColor)
                cellStyle.SelectionForeColor = DisplayContentHelper.SelectColor(template.SelectionForeColor);
            // -

            cellStyle.Alignment = template.Alignment;

            if (0 != template.Padding)
                cellStyle.Padding = new Padding(template.Padding);

            cellStyle.WrapMode = template.WrapMode;

            return cellStyle;
        }

        private TResult MapRow<TResult>(DataGridViewRow row)
            where TResult : class
        {
            object @object = Activator.CreateInstance(typeof(TResult));

            foreach (DataGridViewCell cell in row.Cells)
            {
                if (null == cell.Tag)
                    return null;

                //var column = mDataGridView.Columns[cell.ColumnIndex];
                var column = cell.OwningColumn;

                var propertyInfo = typeof(TResult).GetProperty(column.Name);

                if (null == propertyInfo)
                    throw new InvalidOperationException("null == propertyInfo");

                propertyInfo.SetValue(@object, cell.Tag, null);
            }

            return (TResult)@object;
        }

        private Dictionary<string, object> GetEntityAsDictionary(DataGridViewRow row)
        {
            if (row.Cells.Count <= 0)
                return null;

            var dictionary = new Dictionary<string, object>();

            for (int pos = 0; pos < row.Cells.Count; pos++)
            {
                dictionary.Add(mDataGridView.Columns[row.Cells[pos].ColumnIndex].Name, row.Cells[pos].Value);
            }

            return dictionary;
        }
    }
}