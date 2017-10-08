using System;
using System.Windows.Forms;
using WebMoney.Services.Contracts;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;

namespace WMBusinessTools.Extensions.DisplayHelpers
{
    internal static class IdentifierDisplayHelper
    {
        public static string FormatIdentifierWithAlias(IFormattingService formattingService, long identifier, string identifierAlias)
        {
            if (null == formattingService)
                throw new ArgumentNullException(nameof(formattingService));

            if (null == identifierAlias)
                identifierAlias = "[Master]";

            var identifierValue = formattingService.FormatIdentifier(identifier);

            return $"{identifierAlias} WMID#{identifierValue}";
        }

        public static void ShowFindCertificateForm(IWin32Window owner, SessionContext sessionContext, string identifierValue)
        {
            if (null == sessionContext)
                throw new ArgumentNullException(nameof(sessionContext));

            var identifierContext =
                new IdentifierContext(sessionContext, identifierValue ?? string.Empty);

            sessionContext.ExtensionManager.TryGetIdentifierFormProvider(ExtensionCatalog.FindCertificate)
                ?.GetForm(identifierContext)
                .Show(owner);
        }

        public static void ShowFindIdentifierForm(IWin32Window owner, SessionContext sessionContext, string purseNumber)
        {
            if (null == sessionContext)
                throw new ArgumentNullException(nameof(sessionContext));

            var purseNumberContext =
                new PurseNumberContext(sessionContext, purseNumber ?? string.Empty);

            sessionContext.ExtensionManager.TryGetPurseNumberFormProvider(ExtensionCatalog.FindIdentifier)
                ?.GetForm(purseNumberContext)
                .Show(owner);
        }
    }
}
