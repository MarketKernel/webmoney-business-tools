using System;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;

namespace WMBusinessTools.Extensions
{
    public sealed class CopyPurseNumberActionProvider : IPurseActionProvider, ITrustActionProvider
    {
        public bool CheckCompatibility(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public bool CheckCompatibility(TrustContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public void RunAction(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            Copy(context.Account.Number);
        }

        public void RunAction(TrustContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            Copy(context.Trust.Purse);
        }

        private void Copy(string number)
        {
            Clipboard.SetText(number, TextDataFormat.UnicodeText);
        }
    }
}
