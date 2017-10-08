namespace WebMoney.Services.Contracts
{
    public interface ISmsService
    {
        void SendSms(string payFromPurse, string phoneNumber, string message, bool transliterate = true);
    }
}
