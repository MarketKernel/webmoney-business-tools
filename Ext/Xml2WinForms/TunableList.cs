using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    public class TunableList : ListView
    {
        public const string CellMouseDoubleClickCommandName = "CellMouseDoubleClick";

        private readonly IContainer _container;
        private readonly TunableMenu _commandMenu;
        private readonly ImageList _imageList;

        private readonly ListViewItemSorter _listViewItemSorter = new ListViewItemSorter();

        private bool _isCommandMenuAllowed;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object CurrentEntity { get; private set; }

        [Category("Action"), Description("Service command.")]
        public event EventHandler<CommandEventArgs> ServiceCommand;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MenuItemResolverDelegate MenuItemResolver { get; set; }

        public TunableList()
        {
            _container = new Container();

            // TunableMenu
            var commandMenu = new TunableMenu(_container)
            {
                MenuItemResolver = (entity, command) => null == MenuItemResolver ||
                                                        MenuItemResolver(CurrentEntity, command)
            };

            commandMenu.ServiceCommand += (sender, args) =>
            {
                if (null == CurrentEntity)
                    return;

                ServiceCommand?.Invoke(this,
                    new CommandEventArgs {Command = args.Command, Argument = CurrentEntity});
            };

            commandMenu.Opening += (sender, args) =>
            {
                if (!_isCommandMenuAllowed)
                {
                    args.Cancel = true;
                    return;
                }

                _isCommandMenuAllowed = false;
            };

            _commandMenu = commandMenu;

            // ImageList
            _imageList = new ImageList(_container);
            _imageList.ColorDepth = ColorDepth.Depth32Bit;

            // OwnerDraw
            DrawColumnHeader += OnDrawColumnHeader;
            DrawItem += (sender, args) => args.DrawDefault = true;
            DrawSubItem += (sender, args) => args.DrawDefault = true;
            ColumnClick += OnColumnClick;

            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _container?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            //
            // TunableListView
            //
            FullRowSelect = true;
            HideSelection = false;
            MultiSelect = false;
            View = View.Details;

            ResumeLayout(false);

        }

        public void ApplyTemplate(TunableListTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Reset();

            if (null == template.Columns)
                throw new BadTemplateException("null == template.Columns");

            BeginUpdate();
            Enabled = false;

            HeaderStyle = template.HeaderClickable ? ColumnHeaderStyle.Clickable : ColumnHeaderStyle.Nonclickable;

            if (template.Groups.Count > 0)
            {
                var groups = new List<ListViewGroup>();

                foreach (var groupTemplate in template.Groups)
                {
                    if (null == groupTemplate.Key)
                        throw new BadTemplateException("null == groupTemplate.Key");

                    if (null == groupTemplate.HeaderText)
                        throw new BadTemplateException("null == groupTemplate.HeaderText");

                    groups.Add(new ListViewGroup(groupTemplate.Key, groupTemplate.HeaderText));
                }

                Groups.AddRange(groups.ToArray());
                ShowGroups = true;
            }

            var columnHeaders = new List<ColumnHeader>();

            foreach (var columnTemplate in template.Columns)
            {
                if (null == columnTemplate.Name)
                    throw new BadTemplateException("null == columnTemplate.Name");

                if (null == columnTemplate.HeaderText)
                    throw new BadTemplateException("null == columnTemplate.HeaderText");

                if (0 == columnTemplate.Width)
                    throw new BadTemplateException("0 == columnTemplate.Width");

                var columnHeader = new ColumnHeader
                {
                    Name = columnTemplate.Name,
                    Text = columnTemplate.HeaderText,
                    Width = columnTemplate.Width
                };

                columnHeaders.Add(columnHeader);
            }

            Columns.AddRange(columnHeaders.ToArray());

            if (null != template.CommandMenu && 0 != template.CommandMenu.Items.Count)
            {
                _commandMenu.ApplyTemplate(template.CommandMenu);
                ContextMenuStrip = _commandMenu;
            }

            if (template.Icons.Any())
            {
                foreach (var imageTemplate in template.Icons)
                {
                    if (null == imageTemplate.Name)
                        throw new BadTemplateException("null == imageTemplate.Name");

                    var iconHolder = imageTemplate.BuildIconHolder();

                    if (null == iconHolder)
                        throw new BadTemplateException("null == iconHolder");

                    var image = iconHolder.TryBuildImage();

                    if (null != image)
                        _imageList.Images.Add(imageTemplate.Name, image);
                }

                SmallImageList = _imageList;
            }

            if (template.CheckBoxes)
            {
                CheckBoxes = true;
                OwnerDraw = true;
            }

            Enabled = true;
            EndUpdate();
        }

        public void DisplayContent(List<ListItemContent> contentItems, bool clear = true)
        {
            if (null == contentItems)
                throw new ArgumentNullException(nameof(contentItems));

            var listViewItems = new List<ListViewItem>();

            foreach (var contentItem in contentItems)
            {
                var listViewItem = BuildListViewItem(contentItem);

                if (null != listViewItem)
                    listViewItems.Add(listViewItem);
            }

            BeginUpdate();
            Enabled = false;

            if (clear && 0 != Items.Count)
            {
                Items.Clear();
                CurrentEntity = null;
            }

            if (0 != listViewItems.Count)
                Items.AddRange(listViewItems.ToArray());

            Enabled = true;
            EndUpdate();

            if (!clear)
                foreach (var listViewItem in listViewItems)
                {
                    EnsureVisible(listViewItem.Index);
                }
        }

        public void InsertItem(ListItemContent contentItem)
        {
            if (null == contentItem)
                throw new ArgumentNullException(nameof(contentItem));

            var listViewItem = BuildListViewItem(contentItem);

            BeginUpdate();
            Enabled = false;

            Items.Insert(0, listViewItem);

            Enabled = true;
            EndUpdate();

            EnsureVisible(listViewItem.Index);
        }

        public void Reset()
        {
            BeginUpdate();
            Enabled = false;

            HeaderStyle = ColumnHeaderStyle.None;

            CurrentEntity = null;

            Items.Clear();
            Columns.Clear();
            Groups.Clear();

            ShowGroups = false;

            _imageList.Images.Clear();
            SmallImageList = null;

            _commandMenu.Reset();
            ContextMenuStrip = null;

            CheckBoxes = false;
            OwnerDraw = false;

            Enabled = true;
            EndUpdate();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            CurrentEntity = 0 != SelectedItems.Count ? SelectedItems[0].Tag : null;
            base.OnSelectedIndexChanged(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            if (null == CurrentEntity)
                return;

            ServiceCommand?.Invoke(this,
                new CommandEventArgs {Command = CellMouseDoubleClickCommandName, Argument = CurrentEntity});
        }

        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            base.OnColumnClick(e);

            _listViewItemSorter.ColumnIndex = e.Column;

            if (null == ListViewItemSorter)
                ListViewItemSorter = _listViewItemSorter;

            switch (Sorting)
            {
                case SortOrder.None:
                case SortOrder.Ascending:
                    Sorting = SortOrder.Descending;
                    break;
                case SortOrder.Descending:
                    Sorting = SortOrder.Ascending;
                    break;
            }

            // Специальная сортировка
            BeginUpdate();
            Sort();
            EndUpdate();

            ListViewItemSorter = null; // Для ускорения
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            var listViewItem = GetItemAt(e.X, e.Y);

            if (null != listViewItem && 0 != (MouseButtons.Right & e.Button))
                _isCommandMenuAllowed = true;
        }

        private void OnDrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // Только для первой колонки: рисуем CheckBox с надписью.
            if (e.ColumnIndex != 0)
            {
                e.DrawDefault = true;
                return;
            }

            e.DrawBackground();

            bool value = false;

            var tag = e.Header.Tag;

            if (null != tag)
                value = Convert.ToBoolean(tag);

            var checkBoxState = value
                ? CheckBoxState.CheckedNormal
                : CheckBoxState.UncheckedNormal;

            var glyphSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, checkBoxState);

            var textX = e.Bounds.Left + 4 + glyphSize.Width;

            CheckBoxRenderer.DrawCheckBox(e.Graphics,
                new Point(e.Bounds.Left + 4, e.Bounds.Top + 4),
                new Rectangle(textX, e.Bounds.Top + 4, e.Header.Width - textX, FontHeight),
                e.Header.Text,
                Font,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.WordEllipsis |
                TextFormatFlags.LeftAndRightPadding,
                false,
                checkBoxState);
        }

        private void OnColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (!OwnerDraw)
                return;

            if (e.Column != 0)
                return;

            bool value = false;

            var tag = Columns[e.Column].Tag;

            if (null != tag)
                value = Convert.ToBoolean(tag);

            BeginUpdate();
            Columns[e.Column].Tag = !value;

            foreach (ListViewItem item in Items)
                item.Checked = !value;

            EndUpdate();
            Invalidate();
        }

        private ListViewItem BuildListViewItem(ListItemContent contentItem)
        {
            ListViewItem listViewItem = null;

            var entry = contentItem.Entry;

            foreach (ColumnHeader column in Columns)
            {
                var propertyInfo = entry.GetType().GetProperty(column.Name);

                if (null == propertyInfo)
                    throw new BadTemplateException($"Property {column.Name} not found in ListItemContent!");

                var value = propertyInfo.GetValue(entry, null);

                string text = DisplayContentHelper.GetText(propertyInfo, value);

                if (null == listViewItem)
                {
                    listViewItem = new ListViewItem(text) { Tag = entry };
                    listViewItem.SubItems[0].Tag = value;
                }
                else
                {
                    var subItem = new ListViewItem.ListViewSubItem { Text = text, Tag = value };
                    listViewItem.SubItems.Add(subItem);
                }
            }

            if (null != listViewItem)
            {
                if (null != contentItem.Group)
                    listViewItem.Group = Groups[contentItem.Group];

                if (null != contentItem.ImageKey)
                    listViewItem.ImageKey = contentItem.ImageKey;
            }

            return listViewItem;
        }
    }
}