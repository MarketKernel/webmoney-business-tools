using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    public sealed class OriginalOutgoingInvoice : IOriginalOutgoingInvoice
    {
        public int OrderId { get; }
        public long ClientIdentifier { get; }
        public string StorePurse { get; }
        public decimal Amount { get; }
        public string Description { get; }
        public string Address { get; set; }
        public byte? ProtectionPeriod { get; set; }
        public byte ExpirationPeriod { get; set; }
        public bool Force { get; set; }
        public int? ShopId { get; set; }

        public OriginalOutgoingInvoice(int orderId, long clientIdentifier, string storePurse, decimal amount,
            string description)
        {
            OrderId = orderId;
            ClientIdentifier = clientIdentifier;
            StorePurse = storePurse ?? throw new ArgumentNullException(nameof(storePurse));
            Amount = amount;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}
