using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using UninstallAssistant.Properties;

namespace UninstallAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private volatile bool _done;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindowOnLoaded(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    UninstallUtility.Uninstall();
                }
                catch (Exception exception)
                {
                    Debug.Write(exception);
                }

                Thread.Sleep(1000);

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                    new Action(() =>
                    {
                        _done = true;
                        Close();
                    }));
            });

            thread.Start();
        }

        private void MainWindowOnClosing(object sender, CancelEventArgs e)
        {
            if (!_done)
                e.Cancel = true;
        }
    }
}
