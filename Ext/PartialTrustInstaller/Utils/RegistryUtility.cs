using System;
using System.IO;
using Microsoft.Win32;

namespace PartialTrustInstaller.Utils
{
    internal static class RegistryUtility
    {
        private const string UninstallGuid = "{7dff376e-5f12-47d3-becb-ce0f1ffce973}";
        private const string UninstallAssistantFile = "Uninstall.exe";

        public static void AddUninstallRegistryKey(int size, string version)
        {
            if (null == version)
                throw new ArgumentNullException(nameof(version));

            var uninstallAssistantPath = Path.Combine(SetupConsts.AppDirectory, UninstallAssistantFile);

            using (var parentKey =
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
            {
                using (var key = parentKey?.CreateSubKey(UninstallGuid))
                {
                    if (null == key)
                        return;

                    key.SetValue("Contact", SetupConsts.ContactEmail);
                    key.SetValue("DisplayIcon", $"\"{uninstallAssistantPath}\",0");
                    key.SetValue("DisplayName", SetupConsts.ProductName);
                    key.SetValue("DisplayVersion", version);
                    key.SetValue("EstimatedSize", size, RegistryValueKind.DWord);
                    key.SetValue("HelpLink", SetupConsts.HelpUrl);
                    key.SetValue("HelpTelephone", SetupConsts.HelpTelephone);
                    key.SetValue("Publisher", SetupConsts.CompanyName);
                    key.SetValue("UninstallString", $"\"{uninstallAssistantPath}\"");
                    key.SetValue("URLInfoAbout", SetupConsts.AboutUrl);
                }
            }
        }

        public static void RemoveUninstallRegistryKey()
        {
            using (var parentKey =
                Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                parentKey?.DeleteSubKey(UninstallGuid, false);
            }
        }
    }
}