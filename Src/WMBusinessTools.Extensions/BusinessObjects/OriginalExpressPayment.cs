using System;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class OriginalExpressPayment : IOriginalExpressPayment
    {
        public int OrderId { get; }
        public string TargetPurse { get; }
        public decimal Amount { get; }
        public string Description { get; }
        public IExtendedIdentifier ExtendedIdentifier { get; }
        public ConfirmationType ConfirmationType { get; set; }
        public int? ShopId { get; set; }

        public OriginalExpressPayment(int orderId, string targetPurse, decimal amount, string description, ExtendedIdentifier extendedIdentifier)
        {
            OrderId = orderId;
            TargetPurse = targetPurse ?? throw new ArgumentNullException(nameof(targetPurse));
            Amount = amount;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            ExtendedIdentifier = extendedIdentifier ?? throw new ArgumentNullException(nameof(extendedIdentifier));

        }
    }
}
