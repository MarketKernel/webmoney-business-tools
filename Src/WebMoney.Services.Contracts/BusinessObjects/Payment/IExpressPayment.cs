using System;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IExpressPayment
    {
        long TransferId { get; }
        long InvoiceId { get; }
        decimal Amount { get; }
        DateTime TransferCreateTime { get; }
        string SourcePurse { get; }
        long SourceIdentifier { get; }
    }
}
