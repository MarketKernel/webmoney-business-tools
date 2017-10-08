using System;
using System.Net;
using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IMerchantPayment
    {
        long SystemTransferId { get; }
        long SystemInvoiceId { get; }
        decimal Amount { get; }
        DateTime CreationTime { get; }
        string Description { get; }
        long SourceIdentifier { get; }
        string SourcePurse { get; }
        bool IsCapitaller { get; }
        byte IsEnum { get; }
        IPAddress IPAddress { get; }
        string TelepatPhone { get; }
        TelepatMethod? TelepatMethod { get; }
        string PaymerNumber { get; }
        string PaymerEmail { get; }
        PaymerType? PaymerType { get; }
        string CashierNumber { get; }
        DateTime? CashierDate { get; }
        decimal? CashierAmount { get; }
        byte? InvoicingMethod { get; }
    }
}
