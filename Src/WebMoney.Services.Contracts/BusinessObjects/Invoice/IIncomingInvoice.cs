using System;
using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IIncomingInvoice
    {
        long PrimaryId { get; }
        long SecondaryId { get; }
        int OrderId { get; }
        long TargetIdentifier { get; }
        string TargetPurse { get; }
        decimal Amount { get; }
        string Description { get; }
        string Address { get; }
        byte? ProtectionPeriod { get; }
        byte ExpirationPeriod { get; }
        InvoiceState State { get; }
        long? TransferPrimaryId { get; }
        DateTime CreationTime { get; }
        DateTime UpdateTime { get; }
    }
}
