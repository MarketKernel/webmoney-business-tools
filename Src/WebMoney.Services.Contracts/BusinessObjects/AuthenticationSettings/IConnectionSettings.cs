namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IConnectionSettings
    {
        string ConnectionString { get; }
        string ProviderInvariantName { get; }
    }
}
