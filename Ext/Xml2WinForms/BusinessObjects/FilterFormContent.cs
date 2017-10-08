using System;
using System.Collections.Generic;

namespace Xml2WinForms
{
    public sealed class FilterFormContent
    {
        public FilterScreenContent ScreenContent { get; set; }
        public List<string> LabelValues { get; }

        public FilterFormContent(FilterScreenContent screenContent)
        {
            ScreenContent = screenContent ?? throw new ArgumentNullException(nameof(screenContent));
            LabelValues = new List<string>();
        }
    }
}