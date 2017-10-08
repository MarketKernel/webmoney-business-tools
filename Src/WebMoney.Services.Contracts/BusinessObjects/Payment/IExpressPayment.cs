using System;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IExpressPayment
    {
        int TransferPrimaryId { get; }
        int InvoicePrimaryId { get; }
        decimal Amount { get; }
        DateTime TransferCreateTime { get; }
        string SourcePurse { get; }
        long SourceIdentifier { get; }
    }
}
