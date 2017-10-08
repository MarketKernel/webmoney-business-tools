using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace WebMoney.Services.Utils
{
    internal static class SettingsUtility
    {
        private const string BaseDirectoryName = "WMBusinessTools";
        private const string AuthenticationSettingsFileTemplate = "{0:000000000000}.dat";
        private const string SettingsFileTemplate = "{0:000000000000}.settings";

        private static readonly object Anchor = new object();

        private static string _baseDirectoryPath;

        public static string GetAuthenticationSettingsFilePath(long identifier)
        {
            return Path.Combine(GetBaseDirectoryPath(), GetAuthenticationSettingsFileName(identifier));
        }

        public static string GetSettingsFilePath(long identifier)
        {
            return Path.Combine(GetBaseDirectoryPath(), GetSettingsFileName(identifier));
        }

        public static string GetBaseDirectoryPath()
        {
            if (null != _baseDirectoryPath)
                return _baseDirectoryPath;

            lock (Anchor)
            {
                if (null != _baseDirectoryPath)
                    return _baseDirectoryPath;

                var baseDirectoryPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), BaseDirectoryName);

                if (!Directory.Exists(baseDirectoryPath))
                    Directory.CreateDirectory(baseDirectoryPath);

                Thread.MemoryBarrier();
                _baseDirectoryPath = baseDirectoryPath;
            }

            return _baseDirectoryPath;
        }

        private static string GetAuthenticationSettingsFileName(long identifier)
        {
            return string.Format(CultureInfo.InvariantCulture, AuthenticationSettingsFileTemplate, identifier);
        }

        private static string GetSettingsFileName(long identifier)
        {
            return string.Format(CultureInfo.InvariantCulture, SettingsFileTemplate, identifier);
        }
    }
}