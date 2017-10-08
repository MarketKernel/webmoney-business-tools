namespace WMBusinessTools.Extensions.Contracts
{
    public static class ExtensionCatalog
    {
        public static class Tags
        {
            public const string TopExtension = nameof(TopExtension);
            public const string TopScreen = nameof(TopScreen);

            public const string PursesScreenExtension = nameof(PursesScreenExtension);
            public const string PurseExtension = nameof(PurseExtension);

            public const string TransferFilterExtension = nameof(TransferFilterExtension);
            public const string TransferExtension = nameof(TransferExtension);

            public const string IncomingInvoiceFilterExtension = nameof(IncomingInvoiceFilterExtension);
            public const string IncomingInvoiceExtension = nameof(IncomingInvoiceExtension);

            public const string OutgoingInvoiceFilterExtension = nameof(OutgoingInvoiceFilterExtension);
            public const string OutgoingInvoiceExtension = nameof(OutgoingInvoiceExtension);

            public const string CertificateExtension = nameof(CertificateExtension);

            public const string TrustsScreenExtension = nameof(TrustsScreenExtension);
            public const string TrustExtension = nameof(TrustExtension);

            public const string ContractFilterExtension = nameof(ContractFilterExtension);
            public const string ContractExtension = nameof(ContractExtension);

            public const string TransferBundleFilterExtension = nameof(TransferBundleFilterExtension);
            public const string TransferBundleExtension = nameof(TransferBundleExtension);

            public const string PreparedTransferFilterExtension = nameof(PreparedTransferFilterExtension);
            public const string PreparedTransferExtension = nameof(PreparedTransferExtension);

            public const string SettingsExtension = nameof(SettingsExtension);
        }

        public const string Enter = nameof(Enter);
        public const string Registration = nameof(Registration);

        public const string Main = nameof(Main);

        public const string Content = nameof(Content);
        public const string SendMessageToDeveloper = nameof(SendMessageToDeveloper);
        public const string About = nameof(About);

        public const string Error = nameof(Error);

        // Top
        public const string AddIdentifier = nameof(AddIdentifier);
        public const string RemoveIdentifier = nameof(RemoveIdentifier);

        // TopExtension
        public const string FindCertificate = nameof(FindCertificate);
        public const string Certificate = nameof(Certificate);
        public const string FindIdentifier = nameof(FindIdentifier);
        public const string SendMessage = nameof(SendMessage);
        public const string SendSms = nameof(SendSms);
        public const string CreateContract = nameof(CreateContract);
        public const string ContractFilter = nameof(ContractFilter);
        public const string VerifyClient = nameof(VerifyClient);

        // TopScreen
        public const string PursesScreen = nameof(PursesScreen);
        public const string IncomingInvoiceFilterScreen = nameof(IncomingInvoiceFilterScreen);
        public const string TrustsScreen = nameof(TrustsScreen);

        // PursesScreenExtension
        public const string RefreshPurses = nameof(RefreshPurses);
        public const string CreatePurse = nameof(CreatePurse);
        public const string AddPurse = nameof(AddPurse);
        public const string TakeTrust = nameof(TakeTrust);

        // PurseExtension
        public const string CreateTransfer = nameof(CreateTransfer);
        public const string TransferFilter = nameof(TransferFilter);
        public const string TakePayment = nameof(TakePayment);
        public const string CreatePaymentLink = nameof(CreatePaymentLink);
        public const string RedeemPaymer = nameof(RedeemPaymer);
        public const string FindMerchantTransfer = nameof(FindMerchantTransfer);
        public const string TransferRegister = nameof(TransferRegister);
        public const string TransferBundleFilter = nameof(TransferBundleFilter);
        public const string CreateOutgoingInvoice = nameof(CreateOutgoingInvoice);
        public const string OutgoingInvoiceFilter = nameof(OutgoingInvoiceFilter);
        public const string IncomingInvoiceFilter = nameof(IncomingInvoiceFilter);
        public const string SetMerchantKey = nameof(SetMerchantKey);
        public const string ClearMerchantKey = nameof(ClearMerchantKey);
        public const string RemovePurse = nameof(RemovePurse);
        public const string CopyPurseNumber = nameof(CopyPurseNumber);

        // TransferExtension
        public const string Details = nameof(Details);
        public const string Moneyback = nameof(Moneyback);
        public const string RejectProtection = nameof(RejectProtection);
        public const string FinishProtection = nameof(FinishProtection);

        // IncomingInvoiceExtension
        public const string PayInvoice = nameof(PayInvoice);
        public const string RejectInvoice = nameof(RejectInvoice);

        // TrustsScreenExtension
        public const string RefreshTrusts = nameof(RefreshTrusts);
        public const string CreateTrust = nameof(CreateTrust);

        // TrustExtension
        public const string UpdateTrust = nameof(UpdateTrust);

        // ContractExtension
        public const string ContractDetails = nameof(ContractDetails);
        public const string RefreshContract = nameof(RefreshContract);

        // TransferBundleExtension
        public const string PreparedTransferFilter = nameof(PreparedTransferFilter);
        public const string StartTransferBundle = nameof(StartTransferBundle);
        public const string StopTransferBundle = nameof(StopTransferBundle);

        // SettingsExtension
        public const string RequestNumberSettings = nameof(RequestNumberSettings);
        public const string DbSettings = nameof(DbSettings);
        public const string KeySettings = nameof(KeySettings);
        public const string PasswordSettings = nameof(PasswordSettings);
        public const string GeneralSettings = nameof(GeneralSettings);
    }
}