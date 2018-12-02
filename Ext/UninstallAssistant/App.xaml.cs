using System;
using System.Windows;
using System.Windows.Threading;

namespace UninstallAssistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            LocalizationUtility.ApplyLanguage();

            Current.DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;
        }

        private void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            void Action()
            {
                MessageBox.Show(e.Exception.ToString(), "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke((Action)Action);
            }
            else
            {
                Action();
            }
        }
    }
}
