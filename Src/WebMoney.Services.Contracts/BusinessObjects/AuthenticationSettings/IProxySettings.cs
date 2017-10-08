namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IProxySettings
    {
        string Host { get; set; }
        int Port { get; set; }
        IProxyCredential Credential { get; set; }
    }
}