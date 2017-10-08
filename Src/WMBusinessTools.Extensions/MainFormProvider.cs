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

            ErrorForm.SupportAction = (exceptionType, message, details) =>
            {
                var supportService = context.UnityContainer.Resolve<ISupportService>();
                supportService.SendMessage(exceptionType, message, details);
            };

            return new MainForm(context);
        }
    }
}
