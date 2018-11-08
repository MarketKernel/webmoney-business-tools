namespace WebMoney.Services.Migrations.SqlCE
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "LastIncomingTransferId", c => c.Long());
            AddColumn("dbo.Account", "LastOutgoingTransferId", c => c.Long());
            AddColumn("dbo.Account", "SecretKeyX20", c => c.String(maxLength: 4000));
            AddColumn("dbo.Certificate", "OrganizationCorrAccount", c => c.String(maxLength: 4000));
            AddColumn("dbo.Certificate", "OrganizationCorrAccountAspects", c => c.Int(nullable: false));
            AddColumn("dbo.PreparedTransfer", "PaymentId", c => c.Int(nullable: false));
            AddColumn("dbo.Transfer", "PaymentId", c => c.Int(nullable: false));
            DropColumn("dbo.Account", "LastIncomingTransferPrimaryId");
            DropColumn("dbo.Account", "LastOutgoingTransferPrimaryId");
            DropColumn("dbo.Certificate", "OrganizationCorrespondentAccount");
            DropColumn("dbo.Certificate", "OrganizationCorrespondentAccountAspects");
            DropColumn("dbo.PreparedTransfer", "TransferId");
            DropColumn("dbo.Transfer", "TransferId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transfer", "TransferId", c => c.Int(nullable: false));
            AddColumn("dbo.PreparedTransfer", "TransferId", c => c.Int(nullable: false));
            AddColumn("dbo.Certificate", "OrganizationCorrespondentAccountAspects", c => c.Int(nullable: false));
            AddColumn("dbo.Certificate", "OrganizationCorrespondentAccount", c => c.String(maxLength: 4000));
            AddColumn("dbo.Account", "LastOutgoingTransferPrimaryId", c => c.Long());
            AddColumn("dbo.Account", "LastIncomingTransferPrimaryId", c => c.Long());
            DropColumn("dbo.Transfer", "PaymentId");
            DropColumn("dbo.PreparedTransfer", "PaymentId");
            DropColumn("dbo.Certificate", "OrganizationCorrAccountAspects");
            DropColumn("dbo.Certificate", "OrganizationCorrAccount");
            DropColumn("dbo.Account", "SecretKeyX20");
            DropColumn("dbo.Account", "LastOutgoingTransferId");
            DropColumn("dbo.Account", "LastIncomingTransferId");
        }
    }
}
