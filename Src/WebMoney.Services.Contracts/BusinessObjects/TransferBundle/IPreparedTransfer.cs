using System;
using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IPreparedTransfer
    {
        int Id { get; }
        long? PrimaryId { get; }
        long? SecondaryId { get; }
        int PaymentId { get; }
        string SourcePurse { get; }
        string TargetPurse { get; }
        decimal Amount { get; }
        string Description { get; }
        bool Force { get; }
        PreparedTransferState State { get; }
        DateTime CreationTime { get; }
        DateTime UpdateTime { get; }
        DateTime? TransferCreationTime { get; }
    }
}
