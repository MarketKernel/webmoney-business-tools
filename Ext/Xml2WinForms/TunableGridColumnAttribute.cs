using System;
using System.Windows.Forms;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TunableGridColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public ColumnKind Kind { get; set; }

        public string HeaderText { get; set; }

        public int MaxInputLength { get; set; }

        public bool Fill { get; set; }

        public int Width { get; set; }

        public int MinimumWidth { get; set; }

        public bool Sortable { get; set; }

        public int Order { get; set; }

        public SortOrder SortGlyphDirection { get; set; }

        public bool Visible { get; set; } = true;

        public TunableGridColumnAttribute()
        {
            Kind = ColumnKind.TextBox;
            HeaderText = "empty";
            MaxInputLength = 32767;
            Fill = false;
            Width = 100;
            MinimumWidth = 5;
            Sortable = false;
            SortGlyphDirection = SortOrder.None;
        }
    }
}