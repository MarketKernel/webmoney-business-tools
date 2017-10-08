using System;
using System.Collections.Generic;
using System.Globalization;
using ExtensibilityAssistant;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.Contracts.Internal;
using WMBusinessTools.Extensions.Contracts.Properties;

namespace WMBusinessTools.Extensions.Contracts
{
    public static class ExtensionManagerExtensions
    {
        private static readonly object Anchor = new object();
        private static readonly SortedSet<string> DisabledPlugins = new SortedSet<string>();

        // Enter
        public static ISessionContextProvider GetSessionContextProvider(this ExtensionManager extensionManager)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            return extensionManager.CreateExtension<ISessionContextProvider>(ExtensionCatalog.Enter);
        }

        // Registration
        public static IRegistrationFormProvider TryGetRegistrationFormProvider(
            this ExtensionManager extensionManager)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            return extensionManager.TryCreateExtension<IRegistrationFormProvider>(ExtensionCatalog.Registration);
        }

        // Error
        public static IErrorFormProvider GetErrorFormProvider(this ExtensionManager extensionManager)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            IErrorFormProvider errorFormProvider;

            try
            {
                errorFormProvider = extensionManager.CreateExtension<IErrorFormProvider>(ExtensionCatalog.Error);
            }
            catch (ExtensionNotFoundException)
            {
                errorFormProvider = new ErrorFormProvider();

                var message = $"Extension \"{ExtensionCatalog.Error}\" not found!";
                var errorContext = new ErrorContext("Extension not found!", message);

                errorFormProvider.GetForm(errorContext).ShowDialog(null);
            }

            return errorFormProvider;
        }

        // TopFormProvider/TopActionProvider/TopScreenProvider
        public static ITopFormProvider GetTopFormProvider(this ExtensionManager extensionManager, string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.CreateExtension<ITopFormProvider>(extensionId);
        }

        public static ITopFormProvider TryGetTopFormProvider(this ExtensionManager extensionManager, string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<ITopFormProvider>(extensionId);
        }

        public static ITopActionProvider TryGetTopActionProvider(this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<ITopActionProvider>(extensionId);
        }

        public static ITopScreenProvider TryGetTopScreenProvider(this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<ITopScreenProvider>(extensionId);
        }

        // IdentifierFormProvider
        public static IIdentifierFormProvider TryGetIdentifierFormProvider(this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<IIdentifierFormProvider>(extensionId);
        }

        // PurseNumberFormProvider
        public static IPurseNumberFormProvider TryGetPurseNumberFormProvider(this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<IPurseNumberFormProvider>(extensionId);
        }

        // PurseFormProvider/PurseActionProvider
        public static IPurseFormProvider TryGetPurseFormProvider(this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<IPurseFormProvider>(extensionId);
        }

        public static IPurseActionProvider TryGetPurseActionProvider(this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<IPurseActionProvider>(extensionId);
        }

        // TransferFormProvider
        public static ITransferFormProvider TryGetTransferFormProvider(this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<ITransferFormProvider>(extensionId);
        }

        // IncomingInvoiceFormProvider
        public static IIncomingInvoiceFormProvider TryGetIncomingInvoiceFormProvider(
            this ExtensionManager extensionManager, string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<IIncomingInvoiceFormProvider>(extensionId);
        }

        // OutgoingInvoiceFormProvider
        public static IOutgoingInvoiceFormProvider TryGetOutgoingInvoiceFormProvider(
            this ExtensionManager extensionManager, string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<IOutgoingInvoiceFormProvider>(extensionId);
        }

        // CertificateFormProvider
        public static ICertificateFormProvider TryGetCertificateFormProvider(
            this ExtensionManager extensionManager, string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<ICertificateFormProvider>(extensionId);
        }

        // TrustFormProvider/TrustActionProvider
        public static ITrustFormProvider TryGetTrustFormProvider(this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<ITrustFormProvider>(extensionId);
        }

        public static ITrustActionProvider TryGetTrustActionProvider(this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<ITrustActionProvider>(extensionId);
        }

        // ContractFormProvider/ContractActionProvider
        public static IContractFormProvider TryGetContractFormProvider(this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<IContractFormProvider>(extensionId);
        }

        public static IContractActionProvider TryGetContractActionProvider(this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<IContractActionProvider>(extensionId);
        }

        // TransferBundleFormProvider/TransferBundleActionProvider
        public static ITransferBundleFormProvider TryGetTransferBundleFormProvider(this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<ITransferBundleFormProvider>(extensionId);
        }

        public static ITransferBundleActionProvider TryGetTransferBundleActionProvider(
            this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<ITransferBundleActionProvider>(extensionId);
        }

        // PreparedTransferFormProvider
        public static IPreparedTransferFormProvider TryGetPreparedTransferFormProvider(
            this ExtensionManager extensionManager,
            string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            return extensionManager.TryCreateExtension<IPreparedTransferFormProvider>(extensionId);
        }

        private static TExtension TryCreateExtension<TExtension>(this ExtensionManager extensionManager,
            string extensionId)
            where TExtension : class
        {
            lock (Anchor)
            {
                if (DisabledPlugins.Contains(extensionId))
                    return null;
            }

            try
            {
                return extensionManager.CreateExtension<TExtension>(extensionId);
            }
            catch (ExtensionException exception)
            {
                lock (Anchor)
                {
                    if (DisabledPlugins.Contains(extensionId))
                        return null;

                    DisabledPlugins.Add(extensionId);
                }

                var errorContext = new ErrorContext(
                    Resources.ExtensionManagerExtensions_TryCreateExtension_Plugin_loading_error,
                    string.Format(CultureInfo.InvariantCulture,
                        Resources.ExtensionManagerExtensions_TryCreateExtension_The_plugin___0___could_not_be_loaded,
                        extensionId),
                    ErrorContext.ErrorLevel.Warning)
                {
                    Details = exception.ToString()
                };

                var errorForm = new ErrorForm(errorContext);
                errorForm.ShowDialog();

                return null;
            }
        }
    }
}