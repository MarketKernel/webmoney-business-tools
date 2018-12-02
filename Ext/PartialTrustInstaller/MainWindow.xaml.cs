using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using PartialTrustInstaller.Utils;
using PartialTrustInstaller.Properties;

namespace PartialTrustInstaller
{
    public partial class MainWindow
    {
        private readonly WebClient _webClient;
        private readonly CancellationTokenSource _unzipCancellationTokenSource;
        private readonly CancellationToken _unzipCancellationToken;

        private bool _doNotTouch = true;
        private string _tempFilePath;
        private Task _unzipTask;
        private bool _success;
        
        public MainWindow()
        {
            InitializeComponent();

            _webClient = new WebClient();
            _unzipCancellationTokenSource = new CancellationTokenSource();
            _unzipCancellationToken = _unzipCancellationTokenSource.Token;

            _webClient.DownloadProgressChanged += (o, args) =>
            {
                ProgressBar.Value = args.ProgressPercentage;

                float received = args.BytesReceived;
                float toReceive = args.TotalBytesToReceive;
                var unit = AppResources.MainWindow_MainWindow_B; // Б

                if (toReceive > 1024)
                {
                    received /= 1024;
                    toReceive /= 1024;
                    unit = AppResources.MainWindow_MainWindow_KB; // кбайт

                    if (toReceive > 1024)
                    {
                        received /= 1024;
                        toReceive /= 1024;
                        unit = AppResources.MainWindow_MainWindow_MB; // Мбайт
                    }
                }

                PercentageLabel.Content = string.Format(CultureInfo.InvariantCulture, " {0}% - {1:0.00}/{2:0.00} {3}", args.ProgressPercentage, received, toReceive, unit);
            };
        }

        private async void MainWindowOnLoaded(object sender, RoutedEventArgs e)
        {
            var signedManifest = await TryObtainManifest();

            if (null == signedManifest)
                return;

            // Удаляем, если уже установлено.
            if (Directory.Exists(SetupConsts.AppDirectory))
            {
                var message = string.Format(CultureInfo.InvariantCulture,
                    AppResources
                        .MainWindow_MainWindowOnLoaded_The_program_is_already_installed_on_your_computer__Would_you_like_to_replace_it_with_version__0__of__1__,
                    signedManifest.Version, signedManifest.Date.ToLocalTime().ToShortDateString());

                if (MessageBoxResult.No == MessageBox.Show(
                        message,
                        AppResources.MainWindow_MainWindowOnLoaded_The_program_is_already_installed,
                        MessageBoxButton.YesNo, MessageBoxImage.Question,
                        MessageBoxResult.Yes))
                {
                    Close();
                    return;
                }
            }

            if (Directory.Exists(SetupConsts.AppDirectory))
                try
                {
                    Directory.Delete(SetupConsts.AppDirectory, true);
                }
                catch (Exception exception)
                {
                    Cancel(
                        AppResources
                            .MainWindow_MainWindowOnLoaded_Unable_to_remove_the_previous_installation_,
                        exception.Message);
                    return;
                }

            _doNotTouch = false;

            try
            {
                Directory.CreateDirectory(SetupConsts.AppDirectory);
            }
            catch (Exception exception)
            {
                Cancel(AppResources.MainWindow_MainWindowOnLoaded_Failed_to_create_directory_, exception.Message);
                return;
            }

            // Скачиваем.
            InfoLabel.Text =
                string.Format(CultureInfo.CurrentCulture,
                    AppResources.MainWindow_MainWindowOnLoaded_Download_version__0__of__1,
                    signedManifest.Version,
                    signedManifest.Date.ToLocalTime().ToString("D"));
            SetPercentage(0);

            _tempFilePath = Path.GetTempFileName();

            try
            {
               await  _webClient.DownloadFileAsyncTask(new Uri(signedManifest.PackageUrl), _tempFilePath);
            }
            catch (TaskCanceledException)
            {
                Close();
                return;
            }
            catch (Exception exception)
            {
                Cancel(AppResources.MainWindow_MainWindowOnLoaded_Unable_to_download_file,
                    exception.Message);
                return;
            }

            // Проверяем хеш.
            byte[] expectedHash = HashUtility.FromHexString(signedManifest.Digest);

            var hash = HashUtility.ComputeHash(_tempFilePath);

            if (!expectedHash.SequenceEqual(hash))
            {
                Cancel(AppResources.MainWindow_MainWindowOnLoaded_The_file_is_corrupted);
                return;
            }

            InfoLabel.Text = AppResources.MainWindow_MainWindowOnLoaded_File_successfully_downloaded;
            SetPercentage(100);

            await Delay(500);

            // Распаковываем архив и добавляем ярлыки.
            InfoLabel.Text = AppResources.MainWindow_MainWindowOnLoaded_Archive_unpacking___;
            PercentageLabel.Content = string.Empty;
            ProgressBar.IsIndeterminate = true;

            await Delay(500);

            _unzipTask = Task.Factory.StartNew(() =>
            {
                ExtractToDirectory(_tempFilePath, SetupConsts.AppDirectory);
                File.Delete(_tempFilePath);

                File.WriteAllBytes(Path.Combine(SetupConsts.AppDirectory, SetupConsts.UninstallAssistantFile), AppResources.Uninstall);

                var ruDirectory = Path.Combine(SetupConsts.AppDirectory, "ru");

                if (!Directory.Exists(ruDirectory))
                    Directory.CreateDirectory(ruDirectory);

                File.WriteAllBytes(Path.Combine(ruDirectory, SetupConsts.UninstallAssistantResourcesFile),
                    AppResources.Uninstall_resources);

                var exePath = Path.Combine(SetupConsts.AppDirectory, SetupConsts.AppStartupFile);

                ShortcutUtility.CreateShortcut(exePath, Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));
                ShortcutUtility.CreateShortcut(exePath, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

                var size = CalculateDirectorySize(new DirectoryInfo(SetupConsts.AppDirectory));
                RegistryUtility.AddUninstallRegistryKey(size / 1024, signedManifest.Version);

            }, _unzipCancellationToken);

            try
            {
                await _unzipTask;
            }
            catch (TaskCanceledException)
            {
                Close();
                return;
            }
            catch (Exception exception)
            {
                Cancel(AppResources.MainWindow_MainWindowOnLoaded_Unable_to_unzip_the_archive_,
                    exception.Message);
                return;
            }

            _success = true;
            ProgressBar.IsIndeterminate = false;
            SetPercentage(100);
            InfoLabel.Text = AppResources
                .MainWindow_MainWindowOnLoaded_Program_installation_completed_successfully_;
            CancelButton.Content = AppResources.MainWindow_MainWindowOnLoaded_Finish;
        }

        private async Task<SignedManifest> TryObtainManifest()
        {
            InfoLabel.Text = AppResources.MainWindow_MainWindowOnLoaded_Downloading_manifest;
            PercentageLabel.Content = string.Empty;
            ProgressBar.IsIndeterminate = true;

            string manifest;

            try
            {
                manifest = await Task.Factory.StartNew(() =>
                    new WebClient().DownloadString(new Uri(SetupConsts.ManifestUrl)));
                ProgressBar.IsIndeterminate = false;
            }
            catch (Exception exception)
            {
                ProgressBar.IsIndeterminate = false;
                Cancel(AppResources.MainWindow_MainWindowOnLoaded_Unable_to_download_manifest,
                    exception.Message);

                return null;
            }

            var xDocument = XDocument.Parse(manifest);

            var signedManifest = new SignedManifest(xDocument, Environment.OSVersion.Version.Major);

            bool isValid;

            try
            {
                isValid = signedManifest.CheckSignature(SetupConsts.PublicKey);
            }
            catch (CryptographicException exception)
            {
                Debug.Write(exception);
                isValid = false;
            }

            SetPercentage(100);

            if (!isValid)
            {
                Cancel(AppResources.MainWindow_MainWindowOnLoaded_Wrong_digital_signature_of_the_manifest);
                return null;
            }

            InfoLabel.Text = AppResources.MainWindow_MainWindowOnLoaded_Manifest_successfully_downloaded;

            await Delay(500);

            return signedManifest;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (_doNotTouch)
                return;

            if (_success)
                return;

            if (_webClient.IsBusy)
            {
                _webClient.CancelAsync();
                e.Cancel = true;
                return;
            }

            if (null != _unzipTask && !_unzipTask.IsCompleted && !_unzipTask.IsCanceled && !_unzipTask.IsFaulted)
            {
                _unzipCancellationTokenSource.Cancel();
                e.Cancel = true;
                return;
            }

            TryDeleteAllFiles();
        }

        private void CancelOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Cancel(string message, string errorMessage = null)
        {
            var text = $"{message}";

            if (null != errorMessage)
                text = $"{text} {errorMessage}";

            InfoLabel.Text = text;
            ProgressBar.IsIndeterminate = false;
            ProgressBar.Value = 100;
            ProgressBar.Foreground = Brushes.Red;
            CancelButton.Content = AppResources.MainWindow_Cancel_Close;
        }

        private void SetPercentage(int value)
        {
            ProgressBar.Value = value;
            PercentageLabel.Content = $"{value}%";
        }

        private static Task Delay(int timeout)
        {
            return Task.Factory.StartNew(() => { Thread.Sleep(timeout); });
        }

        private void ExtractToDirectory(string zipFilePath, string directory)
        {
            using (var unzip = Unzip.Open(zipFilePath, FileAccess.Read))
            {
                var zipEntries = unzip.ReadCentralDir();

                foreach (var entry in zipEntries)
                {
                    if (_unzipCancellationToken.IsCancellationRequested)
                        _unzipCancellationToken.ThrowIfCancellationRequested();

                    var filePath = Path.Combine(directory, entry.FilenameInZip);
                    var fileDirectory = Path.GetDirectoryName(filePath);

                    if (null == fileDirectory)
                        throw new InvalidOperationException("null == fileDirectory");

                    if (!Directory.Exists(fileDirectory))
                        Directory.CreateDirectory(fileDirectory);

                    unzip.ExtractFile(entry, filePath);
                }
            }
        }

        public static int CalculateDirectorySize(DirectoryInfo directoryInfo)
        {
            int size = 0;

            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                size += (int) fileInfo.Length;
            }

            foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
            {
                size += CalculateDirectorySize(subDirectoryInfo);
            }

            return size;
        }

        private void TryDeleteAllFiles()
        {
            try
            {
                if (null != _tempFilePath && File.Exists(_tempFilePath))
                    File.Delete(_tempFilePath);
            }
            catch (Exception exception)
            {
                Debug.Write(exception);
            }

            try
            {
                if (Directory.Exists(SetupConsts.AppDirectory))
                    Directory.Delete(SetupConsts.AppDirectory, true);
            }
            catch (Exception exception)
            {
                Debug.Write(exception);
            }

            ShortcutUtility.TryDeleteShortcut(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)),
                SetupConsts.AppStartupFile);

            ShortcutUtility.TryDeleteShortcut(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)),
                SetupConsts.AppStartupFile);

            RegistryUtility.RemoveUninstallRegistryKey();
        }
    }
}
