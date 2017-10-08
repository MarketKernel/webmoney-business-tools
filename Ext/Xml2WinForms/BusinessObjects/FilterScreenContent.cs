using System.Collections.Generic;

namespace Xml2WinForms
{
    public sealed class FilterScreenContent
    {
        public List<GridRowContent> RowContentList { get; set; }
        public List<ChartPoint> ChartPoints { get; set; }

        public FilterScreenContent()
        {
            RowContentList = new List<GridRowContent>();
            ChartPoints = new List<ChartPoint>();
        }
    }
}
