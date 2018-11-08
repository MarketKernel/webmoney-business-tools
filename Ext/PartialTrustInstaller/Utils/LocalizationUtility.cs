using System.Globalization;
using System.Threading;

namespace PartialTrustInstaller.Utils
{
    internal static class LocalizationUtility
    {
        private const string EnCultureName = "en-US";
        private const string RuCultureName = "ru-RU";

        public static void ApplyLanguage()
        {
            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower())
            {
                case "ru": // Россия
                case "uk": // Украина
                case "be": // Беларусь
                case "kk": // Казахстан
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(RuCultureName);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(RuCultureName);
                    break;
                default:
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(EnCultureName);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(EnCultureName);
                    break;
            }
        }
    }
}
