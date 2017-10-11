using System.Globalization;
using System.Threading;
using WebMoney.Services.Contracts.BasicTypes;

namespace WMBusinessTools.Utils
{
    internal sealed class LocalizationUtility
    {
        private const string EnCultureName = "en-US";
        private const string RuCultureName = "ru-RU";

        public static Language GetDefaultLanguage()
        {
            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower())
            {
                case "ru": // Россия
                case "uk": // Украина
                case "be": // Беларусь
                case "kk": // Казахстан
                    return Language.Russian;
                default:
                    return Language.English;
            }
        }

        public static void ApplyLanguage(Language language)
        {
            switch (language)
            {
                case Language.Russian:
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
