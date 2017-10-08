using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    [Table("TransferBundle")]
    internal sealed class TransferBundle : ITransferBundle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        [Index("IX_Identifier_CreationTime", Order = 0, IsUnique = false)]
        public long Identifier { get; set; }

        [ForeignKey("Identifier")]
        public IdentifierSummary IdentifierSummary { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, MaxLength(13)]
        public string SourceAccountName { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal FailedTotalAmount { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal RegisteredTotalAmount { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal PendedTotalAmount { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal ProcessedTotalAmount { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal InterruptedTotalAmount { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal CompletedTotalAmount { get; set; }

        [Required]
        public int FailedCount { get; set; }

        [Required]
        public int RegisteredCount { get; set; }

        [Required]
        public int PendedCount { get; set; }

        [Required]
        public int ProcessedCount { get; set; }

        [Required]
        public int InterruptedCount { get; set; }

        [Required]
        public int CompletedCount { get; set; }

        [Required]
        [Index(IsUnique = false)]
        public TransferBundleState State { get; set; }

        [Required]
        [Index("IX_Identifier_CreationTime", Order = 1, IsUnique = false)]
        public DateTime CreationTime { get; set; }

        [Required]
        public DateTime UpdateTime { get; set; }

        [InverseProperty("TransferBundle")]
        public List<PreparedTransfer> Transfers { get; }

        [NotMapped]
        IReadOnlyCollection<IPreparedTransfer> ITransferBundle.Transfers => Transfers.Select(t => (IPreparedTransfer) t).ToList();

        internal TransferBundle()
        {
            Transfers = new List<PreparedTransfer>();
        }

        public TransferBundle(long identifier, string name, string sourceAccountName, int registeredCount, decimal registeredTotalAmount)
        {
            Identifier = identifier;
            SourceAccountName = sourceAccountName ?? throw new ArgumentNullException(nameof(sourceAccountName));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            RegisteredCount = registeredCount;
            RegisteredTotalAmount = registeredTotalAmount;

            var creationTime = DateTime.UtcNow;

            CreationTime = creationTime;
            UpdateTime = creationTime;

            Transfers = new List<PreparedTransfer>();
        }
    }
}
