using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.Forms;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class MainFormProvider : ITopFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            // Support configuration
            var supportService = context.ExtensionManager.TryCreateExtension<ISupportService>();

            if (null != supportService)
            {
                context.UnityContainer.RegisterInstance(supportService);

                ErrorForm.SupportAction = (exceptionType, message, details) =>
                {
                    context.UnityContainer.Resolve<ISupportService>().SendMessage(exceptionType, message, details);
                };
            }

            return new MainForm(context);
        }
    }
}
