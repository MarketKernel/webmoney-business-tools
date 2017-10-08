using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IOriginalExpressPayment
    {
        int OrderId { get; }
        string TargetPurse { get; }
        decimal Amount { get; }
        string Description { get; }
        IExtendedIdentifier ExtendedIdentifier { get; }
        ConfirmationType ConfirmationType { get; }
        int? ShopId { get; }
    }
}
