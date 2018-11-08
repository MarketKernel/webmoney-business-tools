using System;
using System.Globalization;

namespace WMBusinessTools.Utils
{
    internal static class InitializationUtility
    {
        private static readonly Random Random = new Random();

        public static string BuildInstallationReference()
        {
            var now = DateTime.UtcNow;
            var genesis = new DateTime(2018, 10, 12, 0, 0, 0, DateTimeKind.Utc);

            var days = now.Subtract(genesis).Days;
            var hours = now.Subtract(genesis).Hours;
            var minutes = now.Subtract(genesis).Minutes;
            var noise = Random.Next(0, 10000);

            return string.Format(CultureInfo.InvariantCulture, "{0}-{1:0000}-{2:0000}", days, hours * 60 + minutes, noise);
        }
    }
}