namespace WebMoney.Services.Migrations.MySql
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Number = c.String(nullable: false, maxLength: 13, storeType: "nvarchar"),
                        Identifier = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Amount = c.Decimal(precision: 16, scale: 2),
                        LastIncomingTransferId = c.Long(),
                        LastOutgoingTransferId = c.Long(),
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
                        LastTransferTime = c.DateTime(precision: 0),
                        StoreIdentifier = c.Long(),
                        MerchantKey = c.String(unicode: false),
                        SecretKeyX20 = c.String(unicode: false),
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
                        IdentifierAlias = c.String(nullable: false, unicode: false),
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
                        IdentifierAlias = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        Bl = c.Int(),
                        Tl = c.Int(),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
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
                        CreationTime = c.DateTime(nullable: false, precision: 0),
                        IssuerIdentifier = c.Long(nullable: false),
                        IssuerAlias = c.String(unicode: false),
                        Status = c.Int(nullable: false),
                        Bl = c.Int(),
                        Tl = c.Int(),
                        PositiveRating = c.Int(),
                        NegativeRating = c.Int(),
                        Appointment = c.Int(nullable: false),
                        Basis = c.String(unicode: false),
                        IdentifierAlias = c.String(unicode: false),
                        Information = c.String(unicode: false),
                        City = c.String(unicode: false),
                        Region = c.String(unicode: false),
                        Country = c.String(unicode: false),
                        ZipCode = c.String(unicode: false),
                        Address = c.String(unicode: false),
                        Surname = c.String(unicode: false),
                        FirstName = c.String(unicode: false),
                        Patronymic = c.String(unicode: false),
                        PassportNumber = c.String(unicode: false),
                        PassportDate = c.DateTime(precision: 0),
                        PassportCountry = c.String(unicode: false),
                        PassportCity = c.String(unicode: false),
                        PassportIssuer = c.String(unicode: false),
                        RegistrationCountry = c.String(unicode: false),
                        RegistrationCity = c.String(unicode: false),
                        RegistrationAddress = c.String(unicode: false),
                        Birthplace = c.String(unicode: false),
                        Birthday = c.DateTime(precision: 0),
                        OrganizationName = c.String(unicode: false),
                        OrganizationManager = c.String(unicode: false),
                        OrganizationAccountant = c.String(unicode: false),
                        OrganizationTaxId = c.String(unicode: false),
                        OrganizationId = c.String(unicode: false),
                        OrganizationActivityId = c.String(unicode: false),
                        OrganizationAddress = c.String(unicode: false),
                        OrganizationCountry = c.String(unicode: false),
                        OrganizationCity = c.String(unicode: false),
                        OrganizationZipCode = c.String(unicode: false),
                        OrganizationBankName = c.String(unicode: false),
                        OrganizationBankId = c.String(unicode: false),
                        OrganizationCorrAccount = c.String(unicode: false),
                        OrganizationAccount = c.String(unicode: false),
                        HomePhone = c.String(unicode: false),
                        CellPhone = c.String(unicode: false),
                        Icq = c.String(unicode: false),
                        Fax = c.String(unicode: false),
                        Email = c.String(unicode: false),
                        WebAddress = c.String(unicode: false),
                        ContactPhone = c.String(unicode: false),
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
                        OrganizationCorrAccountAspects = c.Int(nullable: false),
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
                        Name = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Text = c.String(nullable: false, unicode: false),
                        IsPublic = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CreationTime);
            
            CreateTable(
                "dbo.ContractSignature",
                c => new
                    {
                        ContractId = c.Int(nullable: false),
                        AcceptorIdentifier = c.Long(nullable: false),
                        AcceptTime = c.DateTime(precision: 0),
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
                        PaymentId = c.Int(nullable: false),
                        SourcePurse = c.String(nullable: false, unicode: false),
                        TargetPurse = c.String(nullable: false, unicode: false),
                        Amount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        Description = c.String(nullable: false, unicode: false),
                        Force = c.Boolean(nullable: false),
                        State = c.Int(nullable: false),
                        ErrorMessage = c.String(unicode: false),
                        CreationTime = c.DateTime(nullable: false, precision: 0),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        TransferCreationTime = c.DateTime(precision: 0),
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
                        Name = c.String(nullable: false, unicode: false),
                        SourceAccountName = c.String(nullable: false, maxLength: 13, storeType: "nvarchar"),
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
                        CreationTime = c.DateTime(nullable: false, precision: 0),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentifierSummary", t => t.Identifier, cascadeDelete: true)
                .Index(t => new { t.Identifier, t.CreationTime })
                .Index(t => t.State);
            
            CreateTable(
                "dbo.PurseSummary",
                c => new
                    {
                        Purse = c.String(nullable: false, maxLength: 13, storeType: "nvarchar"),
                        Identifier = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Purse)
                .Index(t => t.Identifier);
            
            CreateTable(
                "dbo.Record",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Value = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transfer",
                c => new
                    {
                        Identifier = c.Long(nullable: false),
                        PrimaryId = c.Long(nullable: false),
                        SecondaryId = c.Long(nullable: false),
                        SourcePurse = c.String(nullable: false, maxLength: 13, storeType: "nvarchar"),
                        TargetPurse = c.String(nullable: false, maxLength: 13, storeType: "nvarchar"),
                        Amount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        Commission = c.Decimal(nullable: false, precision: 16, scale: 2),
                        Description = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Type = c.Int(nullable: false),
                        InvoiceId = c.Long(nullable: false),
                        OrderId = c.Int(nullable: false),
                        PaymentId = c.Int(nullable: false),
                        ProtectionPeriod = c.Byte(nullable: false),
                        PartnerIdentifier = c.Long(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 16, scale: 2),
                        Locked = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false, precision: 0),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
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
                        Purse = c.String(nullable: false, maxLength: 13, storeType: "nvarchar"),
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
                        LastTransferTime = c.DateTime(nullable: false, precision: 0),
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
