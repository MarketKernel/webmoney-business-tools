using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class PaymentLinkRequest : IPaymentLinkRequest
    {
        public int OrderId { get; }
        public string StorePurse { get; }
        public decimal Amount { get; }
        public string Description { get; }
        public int Lifetime { get; set; }

        public PaymentLinkRequest(int orderId, string storePurse, decimal amount, string description)
        {
            OrderId = orderId;
            StorePurse = storePurse ?? throw new ArgumentNullException(nameof(storePurse));
            Amount = amount;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}