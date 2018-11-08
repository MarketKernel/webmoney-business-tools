using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    [Table("PreparedTransfer")]
    internal sealed class PreparedTransfer : IPreparedTransfer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Index("IX_TransferBundleId_CreationTime", Order = 0, IsUnique = false)]
        [Index("IX_TransferBundleId_State", Order = 0, IsUnique = false)]
        public int TransferBundleId { get; set; }

        [ForeignKey("TransferBundleId")]
        public TransferBundle TransferBundle { get; set; }

        public long? PrimaryId { get; set; }
        public long? SecondaryId { get; set; }

        [Required]
        public int PaymentId { get; set; }

        [Required]
        public string SourcePurse { get; set; }

        [Required]
        public string TargetPurse { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.AmountTemplate)]
        public decimal Amount { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool Force { get; set; }

        [Required]
        [Index("IX_TransferBundleId_State", Order = 1, IsUnique = false)]
        public PreparedTransferState State { get; set; }

        public string ErrorMessage { get; set; }

        [Required]
        [Index("IX_TransferBundleId_CreationTime", Order = 1, IsUnique = false)]
        public DateTime CreationTime { get; set; }

        [Required]
        public DateTime UpdateTime { get; set; }

        public DateTime? TransferCreationTime { get; set; }

        internal PreparedTransfer()
        {
        }

        public PreparedTransfer(int paymentId, string sourcePurse, string targetPurse, decimal amount, string description)
        {
            PaymentId = paymentId;
            SourcePurse = sourcePurse ?? throw new ArgumentNullException(nameof(sourcePurse));
            TargetPurse = targetPurse ?? throw new ArgumentNullException(nameof(targetPurse));
            Amount = amount;
            Description = description;
            State = PreparedTransferState.Registered;

            var creationTime = DateTime.UtcNow;

            CreationTime = creationTime;
            UpdateTime = creationTime;
        }
    }
}
