namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IVerificationReport
    {
        string Reference { get; }
        string ClientName { get; }
        string ClientMiddleName { get; }
    }
}
