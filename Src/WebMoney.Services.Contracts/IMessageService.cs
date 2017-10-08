namespace WebMoney.Services.Contracts
{
    public interface IMessageService
    {
        long SendMessage(long toIdentifier, string subject, string message, bool force);
    }
}
