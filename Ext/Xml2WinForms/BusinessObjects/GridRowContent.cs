using System;
using System.Drawing;
using System.Windows.Forms;

namespace Xml2WinForms
{
    public sealed class GridRowContent
    {
        public string Key { get; }
        public Color? ForeColor { get; set; }
        public Color? BackColor { get; set; }
        public Color? SelectionForeColor { get; set; }
        public Color? SelectionBackColor { get; set; }
        public bool Bold { get; set; }
        public bool Strikeout { get; set; }
        public object ContentItem { get; private set; }

        internal DataGridViewRow RowReference { get; set; }

        public GridRowContent(string key, object contentItem)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            ContentItem = contentItem ?? throw new ArgumentNullException(nameof(contentItem));
        }

        internal void CopyTo(GridRowContent other)
        {
            if (null == other)
                throw new ArgumentNullException(nameof(other));

            other.ForeColor = ForeColor;
            other.BackColor = BackColor;
            other.SelectionForeColor = SelectionForeColor;
            other.SelectionBackColor = SelectionBackColor;
            other.Bold = Bold;
            other.Strikeout = Strikeout;
            other.ContentItem = ContentItem;
        }
    }
}