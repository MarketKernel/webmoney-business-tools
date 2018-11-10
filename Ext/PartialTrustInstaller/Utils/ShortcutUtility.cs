using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace PartialTrustInstaller.Utils
{
    internal static class ShortcutUtility
    {
        private const string Extension = "url";

        public static void CreateShortcut(string applicationPath, string shortcutDirectory)
        {
            if (null == applicationPath)
                throw new ArgumentNullException(nameof(applicationPath));

            if (null == shortcutDirectory)
                throw new ArgumentNullException(nameof(shortcutDirectory));

            var applicationFileName = Path.GetFileName(applicationPath);
            var shortcutName = Path.ChangeExtension(applicationFileName, Extension);

            if (null == shortcutName)
                throw new InvalidOperationException("null == shortcutName");

            var shortcutPath = Path.Combine(shortcutDirectory, shortcutName);

            if (File.Exists(shortcutPath))
                File.Delete(shortcutPath);

            using (var streamWriter = new StreamWriter(shortcutPath, false, Encoding.ASCII))
            {
                streamWriter.WriteLine("[InternetShortcut]");
                streamWriter.WriteLine(string.Format(CultureInfo.InvariantCulture, "URL=file:///{0}", applicationPath));
                streamWriter.WriteLine("IconIndex=0");
                streamWriter.WriteLine(string.Format(CultureInfo.InvariantCulture, "IconFile={0}", applicationPath));

                streamWriter.Flush();
                streamWriter.Close();
            }
        }

        public static void TryDeleteShortcut(string shortcutDirectory, string appStartupFile)
        {
            if (null == shortcutDirectory)
                throw new ArgumentNullException(nameof(shortcutDirectory));

            if (null == appStartupFile)
                throw new ArgumentNullException(nameof(appStartupFile));

            try
            {
                File.Delete(Path.Combine(shortcutDirectory, Path.ChangeExtension(appStartupFile, "url")));
            }
            catch (Exception exception)
            {
                Debug.Write(exception);
            }
        }
    }
}