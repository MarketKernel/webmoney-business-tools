using System.Drawing;

namespace WMBusinessTools.Extensions.Utils
{
    internal static class ColorUtility
    {
        private const float Multiplier = 0.87F;

        public static Color CalculateSelectionColor(Color color)
        {
            return Color.FromArgb((byte) (color.R * Multiplier), (byte) (color.G * Multiplier),
                (byte) (color.B * Multiplier));
        }
    }
}