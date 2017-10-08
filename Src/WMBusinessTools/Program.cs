using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using ExtensibilityAssistant;
using log4net;
using LocalizationAssistant;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Properties;
using WMBusinessTools.Utils;

namespace WMBusinessTools
{
    static class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        private static ExtensionManager _extensionManager;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LocalizationUtility.ApplyLanguage(LocalizationUtility.GetDefaultLanguage());

#if DEBUG
#else
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Application.ThreadException += ApplicationOnThreadException;
#endif
            Application.ApplicationExit += ApplicationOnApplicationExit;

            try
            {
                _extensionManager = new ExtensionManager(Application.StartupPath);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception.Message, exception);

                MessageBox.Show(Resources.Program_Main_An_error_occurred_while_trying_to_load_the_extension_manager_,
                    Resources.Program_Main_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // CustomAssemblyResolving
            _extensionManager.ApplyCustomAssemblyResolving();

            // WebMoney.Services configuration
            var configurationService = _extensionManager.CreateExtension<IConfigurationService>();

            IUnityContainer unityContainer = new UnityContainer();
            configurationService.RegisterServices(unityContainer);

            // Support configuration
            var supportService = _extensionManager.CreateExtension<ISupportService>();
            unityContainer.RegisterInstance(supportService);

            // Set session
            var enterContext = new EntranceContext(_extensionManager, unityContainer);
            var sessionContextProvider = _extensionManager.GetSessionContextProvider();

            SessionContext sessionContext;

            try
            {
                sessionContext = sessionContextProvider.GetIdentifierContext(enterContext);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);

                MessageBox.Show(Resources.Program_Main_An_error_occurred_while_attempting_to_create_the_new_session_, Resources.Program_Main_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (null == sessionContext)
                return;

            var session = sessionContext.Session;
            unityContainer.RegisterInstance(session);

            // Language
            var language = session.SettingsService.GetSettings().Language;
            LocalizationUtility.ApplyLanguage(language);

            var translator = new Translator();
            translator.Apply();

            // Run main form.
            var formProvider = _extensionManager.GetTopFormProvider(ExtensionCatalog.Main);
            var mainForm = formProvider.GetForm(sessionContext);

            Application.Run(mainForm);
        }

#if DEBUG
#else
        private static void ApplicationOnThreadException(object sender,
            ThreadExceptionEventArgs threadExceptionEventArgs)
        {
            var threadException = threadExceptionEventArgs.Exception;

            Logger.Error(threadException.Message, threadException);

            var errorContext =
                new ErrorContext(threadException.GetType().Name, threadException.Message)
                {
                    Details = threadException.ToString()
                };


            bool handled;

            try
            {
                _extensionManager.GetErrorFormProvider().GetForm(errorContext).ShowDialog(null);
                handled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                handled = false;
            }

            if (!handled)
                MessageBox.Show(errorContext.Message, errorContext.Caption, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        private static void CurrentDomainOnUnhandledException(object sender,
            UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var exception = unhandledExceptionEventArgs.ExceptionObject as Exception;

            string message;

            if (exception != null)
            {
                message = exception.Message;
                Logger.Fatal(message, exception);
            }
            else
            {
                var exceptionObject = unhandledExceptionEventArgs.ExceptionObject;

                message = string.Format(CultureInfo.InvariantCulture,
                    "Non-CLS-compliant exception: Type={0}, String={1}.",
                    exceptionObject.GetType(), exceptionObject);

                Logger.Fatal(message);
            }

            if (unhandledExceptionEventArgs.IsTerminating)
                message = $"{message}\r\nThe program will be closed!";

            MessageBox.Show(message, @"Unhandled Exception!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
#endif

        private static void ApplicationOnApplicationExit(object sender, EventArgs eventArgs)
        {
#if DEBUG
            try
            {
                Translator.Instance.Save();
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
#endif
        }
    }
}