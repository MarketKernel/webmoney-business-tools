using System;
using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IOutgoingInvoice
    {
        long PrimaryId { get; }
        long SecondaryId { get; }
        int OrderId { get; }
        long ClientIdentifier { get; }

        string SourcePurse { get; }
        string TargetPurse { get; }
        decimal Amount { get; }
        string Description { get; }
        string Address { get; }
        byte ExpirationPeriod { get; }
        byte ProtectionPeriod { get; }
        InvoiceState State { get; }
        long? TransferId { get; }
        DateTime CreationTime { get; }
        DateTime UpdateTime { get; }
    }
}
