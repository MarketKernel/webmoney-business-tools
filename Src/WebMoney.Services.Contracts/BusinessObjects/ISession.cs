namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface ISession
    {
        long CurrentIdentifier { get; set; }
        IAuthenticationService AuthenticationService { get; }
        ISettingsService SettingsService { get; }
    }
}
