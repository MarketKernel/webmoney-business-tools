using System;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts.Contexts;

namespace WMBusinessTools.Extensions.Contracts.Internal
{
    internal sealed class ErrorFormProvider : IErrorFormProvider
    {
        public bool CheckCompatibility(ErrorContext context)
        {
            throw new NotSupportedException();
        }

        public Form GetForm(ErrorContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return new ErrorForm(context);
        }
    }
}
