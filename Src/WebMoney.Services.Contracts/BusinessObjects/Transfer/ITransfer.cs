using System;
using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface ITransfer
    {
        long PrimaryId { get; }
        long SecondaryId { get; }
        string SourcePurse { get; }
        string TargetPurse { get; }
        decimal Amount { get; }
        decimal IncomeAmount { get; }
        decimal OutcomeAmount { get; }
        decimal Commission { get; }
        string Description { get; }
        TransferType Type { get; }
        long InvoiceId { get; }
        int OrderId { get; }
        int TransferId { get; }
        byte ProtectionPeriod { get; }
        long PartnerIdentifier { get; }
        decimal Balance { get; }
        bool Locked { get; }
        DateTime CreationTime { get; }
        DateTime UpdateTime { get; }
    }
}
