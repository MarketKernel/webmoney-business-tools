
namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IPaymentLinkRequest
    {
        int OrderId { get; }
        string StorePurse { get; }
        decimal Amount { get; }
        string Description { get; }
        int Lifetime { get; }
    }
}
