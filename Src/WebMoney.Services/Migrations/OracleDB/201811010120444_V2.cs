namespace WebMoney.Services.Migrations.OracleDB
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class V2 : DbMigration
    {
        internal static string Schema { get; set; }

        public override void Up()
        {
            CreateTable(
                $"{Schema}.Account",
                c => new
                {
                    Number = c.String(nullable: false, maxLength: 13),
                    Identifier = c.Decimal(nullable: false, precision: 19, scale: 0),
                    Name = c.String(nullable: false, maxLength: 255),
                    Amount = c.Decimal(precision: 16, scale: 2),
                    LastIncomingTransferId = c.Decimal(precision: 19, scale: 0),
                    LastOutgoingTransferId = c.Decimal(precision: 19, scale: 0),
                    InvoiceAllowed = c.Decimal(precision: 1, scale: 0),
                    TransferAllowed = c.Decimal(precision: 1, scale: 0),
                    BalanceAllowed = c.Decimal(precision: 1, scale: 0),
                    HistoryAllowed = c.Decimal(precision: 1, scale: 0),
                    DayLimit = c.Decimal(precision: 16, scale: 2),
                    WeekLimit = c.Decimal(precision: 16, scale: 2),
                    MonthLimit = c.Decimal(precision: 16, scale: 2),
                    DayTotalAmount = c.Decimal(precision: 16, scale: 2),
                    WeekTotalAmount = c.Decimal(precision: 16, scale: 2),
                    MonthTotalAmount = c.Decimal(precision: 16, scale: 2),
                    LastTransferTime = c.DateTime(),
                    StoreIdentifier = c.Decimal(precision: 19, scale: 0),
                    MerchantKey = c.String(),
                    SecretKeyX20 = c.String(),
                    IsManuallyAdded = c.Decimal(nullable: false, precision: 1, scale: 0),
                })
                .PrimaryKey(t => t.Number)
                .ForeignKey($"{Schema}.IdentifierSummary", t => t.Identifier, cascadeDelete: true)
                .Index(t => t.Identifier);

            CreateTable(
                $"{Schema}.IdentifierSummary",
                c => new
                {
                    Identifier = c.Decimal(nullable: false, precision: 19, scale: 0),
                    IdentifierAlias = c.String(nullable: false),
                    IsMaster = c.Decimal(nullable: false, precision: 1, scale: 0),
                    IsCapitaller = c.Decimal(nullable: false, precision: 1, scale: 0),
                })
                .PrimaryKey(t => t.Identifier);

            CreateTable(
                $"{Schema}.AttachedIdentifier",
                c => new
                {
                    CertificateId = c.Decimal(nullable: false, precision: 19, scale: 0),
                    Identifier = c.Decimal(nullable: false, precision: 19, scale: 0),
                    IdentifierAlias = c.String(),
                    Description = c.String(),
                    Bl = c.Decimal(precision: 10, scale: 0),
                    Tl = c.Decimal(precision: 10, scale: 0),
                    RegistrationDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.CertificateId, t.Identifier })
                .ForeignKey($"{Schema}.Certificate", t => t.CertificateId, cascadeDelete: true)
                .Index(t => t.CertificateId);

            CreateTable(
                $"{Schema}.Certificate",
                c => new
                {
                    Identifier = c.Decimal(nullable: false, precision: 19, scale: 0),
                    Degree = c.Decimal(nullable: false, precision: 10, scale: 0),
                    Revoked = c.Decimal(nullable: false, precision: 1, scale: 0),
                    CreationTime = c.DateTime(nullable: false),
                    IssuerIdentifier = c.Decimal(nullable: false, precision: 19, scale: 0),
                    IssuerAlias = c.String(),
                    Status = c.Decimal(nullable: false, precision: 10, scale: 0),
                    Bl = c.Decimal(precision: 10, scale: 0),
                    Tl = c.Decimal(precision: 10, scale: 0),
                    PositiveRating = c.Decimal(precision: 10, scale: 0),
                    NegativeRating = c.Decimal(precision: 10, scale: 0),
                    Appointment = c.Decimal(nullable: false, precision: 10, scale: 0),
                    Basis = c.String(),
                    IdentifierAlias = c.String(),
                    Information = c.String(),
                    City = c.String(),
                    Region = c.String(),
                    Country = c.String(),
                    ZipCode = c.String(),
                    Address = c.String(),
                    Surname = c.String(),
                    FirstName = c.String(),
                    Patronymic = c.String(),
                    PassportNumber = c.String(),
                    PassportDate = c.DateTime(),
                    PassportCountry = c.String(),
                    PassportCity = c.String(),
                    PassportIssuer = c.String(),
                    RegistrationCountry = c.String(),
                    RegistrationCity = c.String(),
                    RegistrationAddress = c.String(),
                    Birthplace = c.String(),
                    Birthday = c.DateTime(),
                    OrganizationName = c.String(),
                    OrganizationManager = c.String(),
                    OrganizationAccountant = c.String(),
                    OrganizationTaxId = c.String(),
                    OrganizationId = c.String(),
                    OrganizationActivityId = c.String(),
                    OrganizationAddress = c.String(),
                    OrganizationCountry = c.String(),
                    OrganizationCity = c.String(),
                    OrganizationZipCode = c.String(),
                    OrganizationBankName = c.String(),
                    OrganizationBankId = c.String(),
                    OrganizationCorrAccount = c.String(),
                    OrganizationAccount = c.String(),
                    HomePhone = c.String(),
                    CellPhone = c.String(),
                    Icq = c.String(),
                    Fax = c.String(),
                    Email = c.String(),
                    WebAddress = c.String(),
                    ContactPhone = c.String(),
                    CapitallerParent = c.Decimal(precision: 19, scale: 0),
                    PassportInspection = c.Decimal(nullable: false, precision: 1, scale: 0),
                    TaxInspection = c.Decimal(nullable: false, precision: 1, scale: 0),
                    StatusAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    AppointmentAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    BasisAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    AliasAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    InformationAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    CityAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    RegionAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    CountryAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    ZipCodeAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    AddressAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    SurnameAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    FirstNameAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    PatronymicAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    PassportNumberAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    PassportDateAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    PassportCountryAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    PassportCityAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    PassportIssuerAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    RegistrationCountryAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    RegistrationCityAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    RegistrationAddressAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    BirthplaceAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    BirthdayAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationNameAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationManagerAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationAccountantAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationTaxIdAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationIdAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationActivityIdAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationAddressAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationCountryAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationCityAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationZipCodeAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationBankNameAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationBankIdAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationCorrAccountAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    OrganizationAccountAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    HomePhoneAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    CellPhoneAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    IcqAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    FaxAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    EmailAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    WebAddressAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    ContactPhoneAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    CapitallerParentAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    PassportInspectionAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                    TaxInspectionAspects = c.Decimal(nullable: false, precision: 10, scale: 0),
                })
                .PrimaryKey(t => t.Identifier);

            CreateTable(
                $"{Schema}.Contract",
                c => new
                {
                    Id = c.Decimal(nullable: false, precision: 10, scale: 0),
                    Name = c.String(nullable: false, maxLength: 255),
                    Text = c.String(nullable: false),
                    IsPublic = c.Decimal(nullable: false, precision: 1, scale: 0),
                    CreationTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CreationTime);

            CreateTable(
                $"{Schema}.ContractSignature",
                c => new
                {
                    ContractId = c.Decimal(nullable: false, precision: 10, scale: 0),
                    AcceptorIdentifier = c.Decimal(nullable: false, precision: 19, scale: 0),
                    AcceptTime = c.DateTime(),
                })
                .PrimaryKey(t => new { t.ContractId, t.AcceptorIdentifier })
                .ForeignKey($"{Schema}.Contract", t => t.ContractId, cascadeDelete: true)
                .Index(t => t.ContractId);

            CreateTable(
                $"{Schema}.PreparedTransfer",
                c => new
                {
                    Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                    TransferBundleId = c.Decimal(nullable: false, precision: 10, scale: 0),
                    PrimaryId = c.Decimal(precision: 19, scale: 0),
                    SecondaryId = c.Decimal(precision: 19, scale: 0),
                    PaymentId = c.Decimal(nullable: false, precision: 10, scale: 0),
                    SourcePurse = c.String(nullable: false),
                    TargetPurse = c.String(nullable: false),
                    Amount = c.Decimal(nullable: false, precision: 16, scale: 2),
                    Description = c.String(nullable: false),
                    Force = c.Decimal(nullable: false, precision: 1, scale: 0),
                    State = c.Decimal(nullable: false, precision: 10, scale: 0),
                    ErrorMessage = c.String(),
                    CreationTime = c.DateTime(nullable: false),
                    UpdateTime = c.DateTime(nullable: false),
                    TransferCreationTime = c.DateTime(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{Schema}.TransferBundle", t => t.TransferBundleId, cascadeDelete: true)
                .Index(t => new { t.TransferBundleId, t.CreationTime })
                .Index(t => new { t.TransferBundleId, t.State });

            CreateTable(
                $"{Schema}.TransferBundle",
                c => new
                {
                    Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                    Identifier = c.Decimal(nullable: false, precision: 19, scale: 0),
                    Name = c.String(nullable: false),
                    SourceAccountName = c.String(nullable: false, maxLength: 13),
                    FailedTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    RegisteredTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                    PendedTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    ProcessedTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                    InterruptedTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                    CompletedTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                    FailedCount = c.Decimal(nullable: false, precision: 10, scale: 0),
                    RegisteredCount = c.Decimal(nullable: false, precision: 10, scale: 0),
                    PendedCount = c.Decimal(nullable: false, precision: 10, scale: 0),
                    ProcessedCount = c.Decimal(nullable: false, precision: 10, scale: 0),
                    InterruptedCount = c.Decimal(nullable: false, precision: 10, scale: 0),
                    CompletedCount = c.Decimal(nullable: false, precision: 10, scale: 0),
                    State = c.Decimal(nullable: false, precision: 10, scale: 0),
                    CreationTime = c.DateTime(nullable: false),
                    UpdateTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{Schema}.IdentifierSummary", t => t.Identifier, cascadeDelete: true)
                .Index(t => new { t.Identifier, t.CreationTime })
                .Index(t => t.State);

            CreateTable(
                $"{Schema}.PurseSummary",
                c => new
                {
                    Purse = c.String(nullable: false, maxLength: 13),
                    Identifier = c.Decimal(nullable: false, precision: 19, scale: 0),
                })
                .PrimaryKey(t => t.Purse)
                .Index(t => t.Identifier);

            CreateTable(
                $"{Schema}.Record",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 50),
                    Value = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                $"{Schema}.Transfer",
                c => new
                {
                    Identifier = c.Decimal(nullable: false, precision: 19, scale: 0),
                    PrimaryId = c.Decimal(nullable: false, precision: 19, scale: 0),
                    SecondaryId = c.Decimal(nullable: false, precision: 19, scale: 0),
                    SourcePurse = c.String(nullable: false, maxLength: 13),
                    TargetPurse = c.String(nullable: false, maxLength: 13),
                    Amount = c.Decimal(nullable: false, precision: 16, scale: 2),
                    Commission = c.Decimal(nullable: false, precision: 16, scale: 2),
                    Description = c.String(nullable: false, maxLength: 255),
                    Type = c.Decimal(nullable: false, precision: 10, scale: 0),
                    InvoiceId = c.Decimal(nullable: false, precision: 19, scale: 0),
                    OrderId = c.Decimal(nullable: false, precision: 10, scale: 0),
                    PaymentId = c.Decimal(nullable: false, precision: 10, scale: 0),
                    ProtectionPeriod = c.Decimal(nullable: false, precision: 3, scale: 0),
                    PartnerIdentifier = c.Decimal(nullable: false, precision: 19, scale: 0),
                    Balance = c.Decimal(nullable: false, precision: 16, scale: 2),
                    Locked = c.Decimal(nullable: false, precision: 1, scale: 0),
                    CreationTime = c.DateTime(nullable: false),
                    UpdateTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.Identifier, t.PrimaryId })
                .ForeignKey($"{Schema}.IdentifierSummary", t => t.Identifier, cascadeDelete: true)
                .Index(t => new { t.Identifier, t.SourcePurse, t.UpdateTime })
                .Index(t => new { t.Identifier, t.TargetPurse, t.UpdateTime });

            CreateTable(
                $"{Schema}.Trust",
                c => new
                {
                    MasterIdentifier = c.Decimal(nullable: false, precision: 19, scale: 0),
                    Purse = c.String(nullable: false, maxLength: 13),
                    InvoiceAllowed = c.Decimal(nullable: false, precision: 1, scale: 0),
                    TransferAllowed = c.Decimal(nullable: false, precision: 1, scale: 0),
                    BalanceAllowed = c.Decimal(nullable: false, precision: 1, scale: 0),
                    HistoryAllowed = c.Decimal(nullable: false, precision: 1, scale: 0),
                    DayLimit = c.Decimal(nullable: false, precision: 16, scale: 2),
                    WeekLimit = c.Decimal(nullable: false, precision: 16, scale: 2),
                    MonthLimit = c.Decimal(nullable: false, precision: 16, scale: 2),
                    DayTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                    WeekTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                    MonthTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                    LastTransferTime = c.DateTime(nullable: false),
                    StoreIdentifier = c.Decimal(precision: 19, scale: 0),
                })
                .PrimaryKey(t => new { t.MasterIdentifier, t.Purse });

        }

        public override void Down()
        {
            DropForeignKey($"{Schema}.Transfer", "Identifier", $"{Schema}.IdentifierSummary");
            DropForeignKey($"{Schema}.PreparedTransfer", "TransferBundleId", $"{Schema}.TransferBundle");
            DropForeignKey($"{Schema}.TransferBundle", "Identifier", $"{Schema}.IdentifierSummary");
            DropForeignKey($"{Schema}.ContractSignature", "ContractId", $"{Schema}.Contract");
            DropForeignKey($"{Schema}.AttachedIdentifier", "CertificateId", $"{Schema}.Certificate");
            DropForeignKey($"{Schema}.Account", "Identifier", $"{Schema}.IdentifierSummary");
            DropIndex($"{Schema}.Transfer", new[] { "Identifier", "TargetPurse", "UpdateTime" });
            DropIndex($"{Schema}.Transfer", new[] { "Identifier", "SourcePurse", "UpdateTime" });
            DropIndex($"{Schema}.PurseSummary", new[] { "Identifier" });
            DropIndex($"{Schema}.TransferBundle", new[] { "State" });
            DropIndex($"{Schema}.TransferBundle", new[] { "Identifier", "CreationTime" });
            DropIndex($"{Schema}.PreparedTransfer", new[] { "TransferBundleId", "State" });
            DropIndex($"{Schema}.PreparedTransfer", new[] { "TransferBundleId", "CreationTime" });
            DropIndex($"{Schema}.ContractSignature", new[] { "ContractId" });
            DropIndex($"{Schema}.Contract", new[] { "CreationTime" });
            DropIndex($"{Schema}.AttachedIdentifier", new[] { "CertificateId" });
            DropIndex($"{Schema}.Account", new[] { "Identifier" });
            DropTable($"{Schema}.Trust");
            DropTable($"{Schema}.Transfer");
            DropTable($"{Schema}.Record");
            DropTable($"{Schema}.PurseSummary");
            DropTable($"{Schema}.TransferBundle");
            DropTable($"{Schema}.PreparedTransfer");
            DropTable($"{Schema}.ContractSignature");
            DropTable($"{Schema}.Contract");
            DropTable($"{Schema}.Certificate");
            DropTable($"{Schema}.AttachedIdentifier");
            DropTable($"{Schema}.IdentifierSummary");
            DropTable($"{Schema}.Account");
        }
    }
}
