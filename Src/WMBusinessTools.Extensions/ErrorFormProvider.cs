using System;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using Xml2WinForms;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions
{
    public sealed class ErrorFormProvider : IErrorFormProvider
    {
        public bool CheckCompatibility(ErrorContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(ErrorContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var errorFormTemplate =
                new ErrorFormTemplate(context.Caption, context.Message);
            errorFormTemplate.SetTemplateInternals(nameof(ErrorForm), null);

            if (null != context.Details)
                errorFormTemplate.Details = context.Details;

            switch (context.Level)
            {
                case ErrorContext.ErrorLevel.Error:
                    errorFormTemplate.Level = ErrorLevel.Error;
                    break;
                case ErrorContext.ErrorLevel.Warning:
                    errorFormTemplate.Level = ErrorLevel.Warning;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(context));
            }

            return new ErrorForm(errorFormTemplate);
        }
    }
}
