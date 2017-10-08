using System;

namespace WMBusinessTools.Extensions.Contracts
{
    public static class EventBroker
    {
        public static event EventHandler<DataChangedEventArgs> PurseChanged;
        public static event EventHandler<DataChangedEventArgs> TrustChanged;
        public static event EventHandler ContractChanged;
        public static event EventHandler TransferBundleCreated;

        public static event EventHandler DatabaseChanged;
        public static event EventHandler LanguageChanged;

        public static void OnPurseChanged(DataChangedEventArgs eventArgs)
        {
            if (null == eventArgs)
                throw new ArgumentNullException(nameof(eventArgs));

            PurseChanged?.Invoke(null, eventArgs);
        }

        public static void OnTrustChanged(DataChangedEventArgs eventArgs)
        {
            if (null == eventArgs)
                throw new ArgumentNullException(nameof(eventArgs));

            TrustChanged?.Invoke(null, eventArgs);
        }

        public static void OnTransferBundleCreated()
        {
            TransferBundleCreated?.Invoke(null, null);
        }

        public static void OnContractChanged()
        {
            ContractChanged?.Invoke(null, null);
        }

        public static void OnDatabaseChanged()
        {
            DatabaseChanged?.Invoke(null, null);
        }

        public static void OnLanguageChanged()
        {
            LanguageChanged?.Invoke(null, null);
        }
    }
}
