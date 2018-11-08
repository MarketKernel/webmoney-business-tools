using System;

namespace Xml2WinForms
{
    public sealed class ListItemContent
    {
        public string Group { get; set; }
        public string ImageKey { get; set; }
        public object Entry { get; }

        public ListItemContent(object entry)
        {
            Entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }
    }
}
