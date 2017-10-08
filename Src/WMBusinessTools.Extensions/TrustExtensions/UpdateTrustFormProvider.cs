using System;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;

namespace WMBusinessTools.Extensions
{
    public sealed class UpdateTrustFormProvider : ITrustFormProvider
    {
        public bool CheckCompatibility(TrustContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(TrustContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return TrustDisplayHelper.CreateForm(context, context.Trust);
        }
    }
}
