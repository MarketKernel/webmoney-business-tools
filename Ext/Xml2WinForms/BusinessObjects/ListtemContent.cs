using System;

namespace Xml2WinForms
{
    public sealed class ListItemContent
    {
        public string Group { get; set; }
        public string ImageKey { get; set; }
        public object ContentItem { get; }

        public ListItemContent(object contentItem)
        {
            ContentItem = contentItem ?? throw new ArgumentNullException(nameof(contentItem));
        }
    }
}
