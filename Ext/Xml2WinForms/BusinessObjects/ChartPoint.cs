using System;
using System.Drawing;

namespace Xml2WinForms
{
    public sealed class ChartPoint
    {
        public string Name { get; }
        public double Value { get; }
        public Color? Color { get; set; }
        public Color? FontColor { get; set; }

        public ChartPoint(string name, double value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
        }
    }
}
