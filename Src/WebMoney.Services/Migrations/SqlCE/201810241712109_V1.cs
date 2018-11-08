namespace WebMoney.Services.Migrations.SqlCE
{
    using System.Data.Entity.Migrations;
    
    public partial class V1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Number = c.String(nullable: false, maxLength: 13),
                        Identifier = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Amount = c.Decimal(precision: 16, scale: 2),
                        LastIncomingTransferPrimaryId = c.Long(),
                        LastOutgoingTransferPrimaryId = c.Long(),
                        InvoiceAllowed = c.Boolean(),
                        TransferAllowed = c.Boolean(),
                        BalanceAllowed = c.Boolean(),
                        HistoryAllowed = c.Boolean(),
                        DayLimit = c.Decimal(precision: 16, scale: 2),
                        WeekLimit = c.Decimal(precision: 16, scale: 2),
                        MonthLimit = c.Decimal(precision: 16, scale: 2),
                        DayTotalAmount = c.Decimal(precision: 16, scale: 2),
                        WeekTotalAmount = c.Decimal(precision: 16, scale: 2),
                        MonthTotalAmount = c.Decimal(precision: 16, scale: 2),
                        LastTransferTime = c.DateTime(),
                        StoreIdentifier = c.Long(),
                        MerchantKey = c.String(maxLength: 4000),
                        IsManuallyAdded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Number)
                .ForeignKey("dbo.IdentifierSummary", t => t.Identifier, cascadeDelete: true)
                .Index(t => t.Identifier);
            
            CreateTable(
                "dbo.IdentifierSummary",
                c => new
                    {
                        Identifier = c.Long(nullable: false),
                        IdentifierAlias = c.String(nullable: false, maxLength: 4000),
                        IsMaster = c.Boolean(nullable: false),
                        IsCapitaller = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Identifier);
            
            CreateTable(
                "dbo.AttachedIdentifier",
                c => new
                    {
                        CertificateId = c.Long(nullable: false),
                        Identifier = c.Long(nullable: false),
                        IdentifierAlias = c.String(maxLength: 4000),
                        Description = c.String(maxLength: 4000),
                        Bl = c.Int(),
                        Tl = c.Int(),
                        RegistrationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.CertificateId, t.Identifier })
                .ForeignKey("dbo.Certificate", t => t.CertificateId, cascadeDelete: true)
                .Index(t => t.CertificateId);
            
            CreateTable(
                "dbo.Certificate",
                c => new
                    {
                        Identifier = c.Long(nullable: false),
                        Degree = c.Int(nullable: false),
                        Revoked = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        IssuerIdentifier = c.Long(nullable: false),
                        IssuerAlias = c.String(maxLength: 4000),
                        Status = c.Int(nullable: false),
                        Bl = c.Int(),
                        Tl = c.Int(),
                        PositiveRating = c.Int(),
                        NegativeRating = c.Int(),
                        Appointment = c.Int(nullable: false),
                        Basis = c.String(maxLength: 4000),
                        IdentifierAlias = c.String(maxLength: 4000),
                        Information = c.String(maxLength: 4000),
                        City = c.String(maxLength: 4000),
                        Region = c.String(maxLength: 4000),
                        Country = c.String(maxLength: 4000),
                        ZipCode = c.String(maxLength: 4000),
                        Address = c.String(maxLength: 4000),
                        Surname = c.String(maxLength: 4000),
                        FirstName = c.String(maxLength: 4000),
                        Patronymic = c.String(maxLength: 4000),
                        PassportNumber = c.String(maxLength: 4000),
                        PassportDate = c.DateTime(),
                        PassportCountry = c.String(maxLength: 4000),
                        PassportCity = c.String(maxLength: 4000),
                        PassportIssuer = c.String(maxLength: 4000),
                        RegistrationCountry = c.String(maxLength: 4000),
                        RegistrationCity = c.String(maxLength: 4000),
                        RegistrationAddress = c.String(maxLength: 4000),
                        Birthplace = c.String(maxLength: 4000),
                        Birthday = c.DateTime(),
                        OrganizationName = c.String(maxLength: 4000),
                        OrganizationManager = c.String(maxLength: 4000),
                        OrganizationAccountant = c.String(maxLength: 4000),
                        OrganizationTaxId = c.String(maxLength: 4000),
                        OrganizationId = c.String(maxLength: 4000),
                        OrganizationActivityId = c.String(maxLength: 4000),
                        OrganizationAddress = c.String(maxLength: 4000),
                        OrganizationCountry = c.String(maxLength: 4000),
                        OrganizationCity = c.String(maxLength: 4000),
                        OrganizationZipCode = c.String(maxLength: 4000),
                        OrganizationBankName = c.String(maxLength: 4000),
                        OrganizationBankId = c.String(maxLength: 4000),
                        OrganizationCorrespondentAccount = c.String(maxLength: 4000),
                        OrganizationAccount = c.String(maxLength: 4000),
                        HomePhone = c.String(maxLength: 4000),
                        CellPhone = c.String(maxLength: 4000),
                        Icq = c.String(maxLength: 4000),
                        Fax = c.String(maxLength: 4000),
                        Email = c.String(maxLength: 4000),
                        WebAddress = c.String(maxLength: 4000),
                        ContactPhone = c.String(maxLength: 4000),
                        CapitallerParent = c.Long(),
                        PassportInspection = c.Boolean(nullable: false),
                        TaxInspection = c.Boolean(nullable: false),
                        StatusAspects = c.Int(nullable: false),
                        AppointmentAspects = c.Int(nullable: false),
                        BasisAspects = c.Int(nullable: false),
                        AliasAspects = c.Int(nullable: false),
                        InformationAspects = c.Int(nullable: false),
                        CityAspects = c.Int(nullable: false),
                        RegionAspects = c.Int(nullable: false),
                        CountryAspects = c.Int(nullable: false),
                        ZipCodeAspects = c.Int(nullable: false),
                        AddressAspects = c.Int(nullable: false),
                        SurnameAspects = c.Int(nullable: false),
                        FirstNameAspects = c.Int(nullable: false),
                        PatronymicAspects = c.Int(nullable: false),
                        PassportNumberAspects = c.Int(nullable: false),
                        PassportDateAspects = c.Int(nullable: false),
                        PassportCountryAspects = c.Int(nullable: false),
                        PassportCityAspects = c.Int(nullable: false),
                        PassportIssuerAspects = c.Int(nullable: false),
                        RegistrationCountryAspects = c.Int(nullable: false),
                        RegistrationCityAspects = c.Int(nullable: false),
                        RegistrationAddressAspects = c.Int(nullable: false),
                        BirthplaceAspects = c.Int(nullable: false),
                        BirthdayAspects = c.Int(nullable: false),
                        OrganizationNameAspects = c.Int(nullable: false),
                        OrganizationManagerAspects = c.Int(nullable: false),
                        OrganizationAccountantAspects = c.Int(nullable: false),
                        OrganizationTaxIdAspects = c.Int(nullable: false),
                        OrganizationIdAspects = c.Int(nullable: false),
                        OrganizationActivityIdAspects = c.Int(nullable: false),
                        OrganizationAddressAspects = c.Int(nullable: false),
                        OrganizationCountryAspects = c.Int(nullable: false),
                        OrganizationCityAspects = c.Int(nullable: false),
                        OrganizationZipCodeAspects = c.Int(nullable: false),
                        OrganizationBankNameAspects = c.Int(nullable: false),
                        OrganizationBankIdAspects = c.Int(nullable: false),
                        OrganizationCorrespondentAccountAspects = c.Int(nullable: false),
                        OrganizationAccountAspects = c.Int(nullable: false),
                        HomePhoneAspects = c.Int(nullable: false),
                        CellPhoneAspects = c.Int(nullable: false),
                        IcqAspects = c.Int(nullable: false),
                        FaxAspects = c.Int(nullable: false),
                        EmailAspects = c.Int(nullable: false),
                        WebAddressAspects = c.Int(nullable: false),
                        ContactPhoneAspects = c.Int(nullable: false),
                        CapitallerParentAspects = c.Int(nullable: false),
                        PassportInspectionAspects = c.Int(nullable: false),
                        TaxInspectionAspects = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Identifier);
            
            CreateTable(
                "dbo.Contract",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Text = c.String(nullable: false, maxLength: 4000),
                        IsPublic = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CreationTime);
            
            CreateTable(
                "dbo.ContractSignature",
                c => new
                    {
                        ContractId = c.Int(nullable: false),
                        AcceptorIdentifier = c.Long(nullable: false),
                        AcceptTime = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.ContractId, t.AcceptorIdentifier })
                .ForeignKey("dbo.Contract", t => t.ContractId, cascadeDelete: true)
                .Index(t => t.ContractId);
            
            CreateTable(
                "dbo.PreparedTransfer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransferBundleId = c.Int(nullable: false),
                        PrimaryId = c.Long(),
                        SecondaryId = c.Long(),
                        TransferId = c.Int(nullable: false),
                        SourcePurse = c.String(nullable: false, maxLength: 4000),
                        TargetPurse = c.String(nullable: false, maxLength: 4000),
                        Amount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        Description = c.String(nullable: false, maxLength: 4000),
                        Force = c.Boolean(nullable: false),
                        State = c.Int(nullable: false),
                        ErrorMessage = c.String(maxLength: 4000),
                        CreationTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        TransferCreationTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransferBundle", t => t.TransferBundleId, cascadeDelete: true)
                .Index(t => new { t.TransferBundleId, t.CreationTime })
                .Index(t => new { t.TransferBundleId, t.State });
            
            CreateTable(
                "dbo.TransferBundle",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identifier = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 4000),
                        SourceAccountName = c.String(nullable: false, maxLength: 13),
                        FailedTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegisteredTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        PendedTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProcessedTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        InterruptedTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        CompletedTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        FailedCount = c.Int(nullable: false),
                        RegisteredCount = c.Int(nullable: false),
                        PendedCount = c.Int(nullable: false),
                        ProcessedCount = c.Int(nullable: false),
                        InterruptedCount = c.Int(nullable: false),
                        CompletedCount = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentifierSummary", t => t.Identifier, cascadeDelete: true)
                .Index(t => new { t.Identifier, t.CreationTime })
                .Index(t => t.State);
            
            CreateTable(
                "dbo.PurseSummary",
                c => new
                    {
                        Purse = c.String(nullable: false, maxLength: 13),
                        Identifier = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Purse)
                .Index(t => t.Identifier);
            
            CreateTable(
                "dbo.Record",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50),
                        Value = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transfer",
                c => new
                    {
                        Identifier = c.Long(nullable: false),
                        PrimaryId = c.Long(nullable: false),
                        SecondaryId = c.Long(nullable: false),
                        SourcePurse = c.String(nullable: false, maxLength: 13),
                        TargetPurse = c.String(nullable: false, maxLength: 13),
                        Amount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        Commission = c.Decimal(nullable: false, precision: 16, scale: 2),
                        Description = c.String(nullable: false, maxLength: 255),
                        Type = c.Int(nullable: false),
                        InvoiceId = c.Long(nullable: false),
                        OrderId = c.Int(nullable: false),
                        TransferId = c.Int(nullable: false),
                        ProtectionPeriod = c.Byte(nullable: false),
                        PartnerIdentifier = c.Long(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 16, scale: 2),
                        Locked = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Identifier, t.PrimaryId })
                .ForeignKey("dbo.IdentifierSummary", t => t.Identifier, cascadeDelete: true)
                .Index(t => new { t.Identifier, t.SourcePurse, t.UpdateTime })
                .Index(t => new { t.Identifier, t.TargetPurse, t.UpdateTime });
            
            CreateTable(
                "dbo.Trust",
                c => new
                    {
                        MasterIdentifier = c.Long(nullable: false),
                        Purse = c.String(nullable: false, maxLength: 13),
                        InvoiceAllowed = c.Boolean(nullable: false),
                        TransferAllowed = c.Boolean(nullable: false),
                        BalanceAllowed = c.Boolean(nullable: false),
                        HistoryAllowed = c.Boolean(nullable: false),
                        DayLimit = c.Decimal(nullable: false, precision: 16, scale: 2),
                        WeekLimit = c.Decimal(nullable: false, precision: 16, scale: 2),
                        MonthLimit = c.Decimal(nullable: false, precision: 16, scale: 2),
                        DayTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        WeekTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        MonthTotalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        LastTransferTime = c.DateTime(nullable: false),
                        StoreIdentifier = c.Long(),
                    })
                .PrimaryKey(t => new { t.MasterIdentifier, t.Purse });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transfer", "Identifier", "dbo.IdentifierSummary");
            DropForeignKey("dbo.PreparedTransfer", "TransferBundleId", "dbo.TransferBundle");
            DropForeignKey("dbo.TransferBundle", "Identifier", "dbo.IdentifierSummary");
            DropForeignKey("dbo.ContractSignature", "ContractId", "dbo.Contract");
            DropForeignKey("dbo.AttachedIdentifier", "CertificateId", "dbo.Certificate");
            DropForeignKey("dbo.Account", "Identifier", "dbo.IdentifierSummary");
            DropIndex("dbo.Transfer", new[] { "Identifier", "TargetPurse", "UpdateTime" });
            DropIndex("dbo.Transfer", new[] { "Identifier", "SourcePurse", "UpdateTime" });
            DropIndex("dbo.PurseSummary", new[] { "Identifier" });
            DropIndex("dbo.TransferBundle", new[] { "State" });
            DropIndex("dbo.TransferBundle", new[] { "Identifier", "CreationTime" });
            DropIndex("dbo.PreparedTransfer", new[] { "TransferBundleId", "State" });
            DropIndex("dbo.PreparedTransfer", new[] { "TransferBundleId", "CreationTime" });
            DropIndex("dbo.ContractSignature", new[] { "ContractId" });
            DropIndex("dbo.Contract", new[] { "CreationTime" });
            DropIndex("dbo.AttachedIdentifier", new[] { "CertificateId" });
            DropIndex("dbo.Account", new[] { "Identifier" });
            DropTable("dbo.Trust");
            DropTable("dbo.Transfer");
            DropTable("dbo.Record");
            DropTable("dbo.PurseSummary");
            DropTable("dbo.TransferBundle");
            DropTable("dbo.PreparedTransfer");
            DropTable("dbo.ContractSignature");
            DropTable("dbo.Contract");
            DropTable("dbo.Certificate");
            DropTable("dbo.AttachedIdentifier");
            DropTable("dbo.IdentifierSummary");
            DropTable("dbo.Account");
        }
    }
}
