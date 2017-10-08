namespace WebMoney.Services.Contracts
{
    public interface ISupportService
    {
        void SendMessage(string exceptionType, string message, string details);
    }
}