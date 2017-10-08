using System;
using System.Windows.Forms;
using ExtensibilityAssistant;
using WebMoney.Services.Contracts.Exceptions;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.Properties;
using Xml2WinForms;

namespace WMBusinessTools.Extensions.DisplayHelpers
{
    internal static class ErrorFormDisplayHelper
    {
        public static Form BuildErrorForm(ExtensionManager extensionManager, string caption, string message,
            ErrorContext.ErrorLevel level = ErrorContext.ErrorLevel.Warning)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == caption)
                throw new ArgumentNullException(nameof(caption));

            if (null == message)
                throw new ArgumentNullException(nameof(message));

            var errorContext = new ErrorContext(caption, message, level);
            return extensionManager.GetErrorFormProvider().GetForm(errorContext);
        }

        public static void ApplyErrorAction(ExtensionManager extensionManager, ListScreen screen)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == screen)
                throw new ArgumentNullException(nameof(screen));

            screen.ErrorAction = GetErrorAction(extensionManager, screen);
        }

        public static void ApplyErrorAction(ExtensionManager extensionManager, FilterScreen screen)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == screen)
                throw new ArgumentNullException(nameof(screen));

            screen.ErrorAction = GetErrorAction(extensionManager, screen);
        }

        public static void ApplyErrorAction(ExtensionManager extensionManager, FilterForm form)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == form)
                throw new ArgumentNullException(nameof(form));

            form.ErrorAction = GetErrorAction(extensionManager, form);
        }

        public static void ApplyErrorAction(ExtensionManager extensionManager, SubmitForm form)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == form)
                throw new ArgumentNullException(nameof(form));

            form.ErrorAction = GetErrorAction(extensionManager, form);
        }

        private static Action<Exception> GetErrorAction(ExtensionManager extensionManager, Control control)
        {
            Form form = null;

            if (null != control)
                form = control.FindForm();

            return GetErrorAction(extensionManager, form);
        }

        private static Action<Exception> GetErrorAction(ExtensionManager extensionManager, Form form)
        {
            return exception =>
            {
                var errorForm = BuildErrorForm(extensionManager, exception);
                errorForm.ShowDialog(form);
            };
        }

        public static Form BuildErrorForm(ExtensionManager extensionManager, Exception exception)
        {
            if (null == exception)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == exception)
                throw new ArgumentNullException(nameof(exception));

            string caption;
            string message;
            string details;
            ErrorContext.ErrorLevel errorLevel;

            var businessException = exception as BusinessException;

            if (null != businessException)
            {
                caption = businessException.Caption;
                message = businessException.Message;
                details = null;
                errorLevel = ErrorContext.ErrorLevel.Warning;
            }
            else
            {
                var invalidOperationException = exception as InvalidOperationException;

                caption = null != invalidOperationException
                    ? Resources.ErrorFormDisplayHelper_GetErrorAction_Operation_failed
                    : Resources.ErrorFormDisplayHelper_GetErrorAction_Unexpected_error;

                message = exception.Message;
                details = exception.ToString();
                errorLevel = ErrorContext.ErrorLevel.Error;
            }

            var errorContext =
                new ErrorContext(caption, message, errorLevel)
                {
                    Details = details
                };

            return extensionManager.GetErrorFormProvider().GetForm(errorContext);
        }
    }
}