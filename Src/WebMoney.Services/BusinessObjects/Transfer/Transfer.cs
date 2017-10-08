using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    [Table("Transfer")]
    internal sealed class Transfer : ITransfer
    {
        [Key, Column(Order = 0)]
        [Index("IX_Identifier_SourcePurse_UpdateTime", Order = 0, IsUnique = false)]
        [Index("IX_Identifier_TargetPurse_UpdateTime", Order = 0, IsUnique = false)]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long Identifier { get; set; }

        [ForeignKey("Identifier")]
        public IdentifierSummary IdentifierSummary { get; set; }

        [Key, Column(Order = 1)]
        public long PrimaryId { get; set; }

        [Required]
        public long SecondaryId { get; set; }

        [Required, MaxLength(13)]
        [Index("IX_Identifier_SourcePurse_UpdateTime", Order = 1, IsUnique = false)]
        public string SourcePurse { get; set; }

        [Required, MaxLength(13)]
        [Index("IX_Identifier_TargetPurse_UpdateTime", Order = 1, IsUnique = false)]
        public string TargetPurse { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal Amount { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal IncomeAmount { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal OutcomeAmount { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal Commission { get; set; }

        [Required, MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public TransferType Type { get; set; }

        public long InvoiceId { get; set; }
        public int OrderId { get; set; }
        public int TransferId { get; set; }
        public byte ProtectionPeriod { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long PartnerIdentifier { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal Balance { get; set; }

        [Required]
        public bool Locked { get; set; }

        public DateTime CreationTime { get; set; }

        [Required]
        [Index("IX_Identifier_SourcePurse_UpdateTime", Order = 2, IsUnique = false)]
        [Index("IX_Identifier_TargetPurse_UpdateTime", Order = 2, IsUnique = false)]
        public DateTime UpdateTime { get; set; }

        internal Transfer()
        {
        }

        public Transfer(long identifier, long primaryId, long secondaryId, string sourcePurse, string targetPurse, decimal amount,
            decimal commission, TransferType type, string description, long partnerIdentifier, decimal balance,
            DateTime creationTime, DateTime updateTime)
        {
            Identifier = identifier;
            PrimaryId = primaryId;
            SecondaryId = secondaryId;
            SourcePurse = sourcePurse ?? throw new ArgumentNullException(nameof(sourcePurse));
            TargetPurse = targetPurse ?? throw new ArgumentNullException(nameof(targetPurse));
            Amount = amount;
            Commission = commission;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Type = type;
            PartnerIdentifier = partnerIdentifier;
            Balance = balance;
            CreationTime = creationTime;
            UpdateTime = updateTime;
        }
    }
}
