using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;

namespace UninstallAssistant
{
    internal static class UninstallUtility
    {
        private const string CompanyName = "MarketKernel";
        private const string UninstallGuid = "{7dff376e-5f12-47d3-becb-ce0f1ffce973}";
        private const string AppStartupFile = "WMBusinessTools.exe";
        private const string UninstallAssistantFile = "Uninstall.exe";
        private const string UninstallAssistantResourcesFile = "Uninstall.resources.dll";

        public static void Uninstall()
        {
            var appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), CompanyName);

            TryDeleteShortcut(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)),
                AppStartupFile);
            TryDeleteShortcut(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)),
                AppStartupFile);

            var files = Directory.GetFiles(appDirectory, "*.*", SearchOption.AllDirectories);

            foreach (var filePath in files)
            {
                var fileName = Path.GetFileName(filePath);

                if (null == fileName)
                    throw new InvalidOperationException("null == fileName");

                if (fileName.Equals(UninstallAssistantFile, StringComparison.OrdinalIgnoreCase) ||
                    fileName.Equals(UninstallAssistantResourcesFile, StringComparison.OrdinalIgnoreCase))
                    continue;

                while (true)
                {
                    try
                    {
                        File.Delete(filePath);
                        break;
                    }
                    catch (Exception)
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                            new Action(() => { MessageBox.Show(fileName); }));

                        Thread.Sleep(500);
                    }
                }
            }

            RemoveUninstallRegistryKey();

            var tempBatchFilePath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), "cmd"));

            string batchScript = "@echo off\n"
                                 + ":loop\n"
                                 + "timeout 1 >nul\n"
                                 + "rd /s /q \"" + appDirectory + "\"\n"
                                 + "if exist \"" + appDirectory + "\" goto loop\n"
                                 + "del %0";

            File.WriteAllText(tempBatchFilePath, batchScript, new UTF8Encoding(false));

            var startInfo = new ProcessStartInfo("cmd.exe")
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = "/c call \"" + tempBatchFilePath + "\""
            };

            Process.Start(startInfo);
        }

        private static void TryDeleteShortcut(string shortcutDirectory, string appStartupFile)
        {
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

        private static void RemoveUninstallRegistryKey()
        {
            using (var parentKey =
                Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                parentKey?.DeleteSubKey(UninstallGuid, false);
            }
        }
    }
}