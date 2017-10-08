namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface ITrustConfirmation
    {
        int Reference { get; }
        string ConfirmationCode { get; }
    }
}
