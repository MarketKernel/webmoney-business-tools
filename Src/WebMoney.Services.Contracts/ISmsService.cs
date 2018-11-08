namespace WebMoney.Services.Contracts
{
    public interface ISmsService
    {
        int SendSms(string payFromPurse, string phoneNumber, string message, bool transliterate = true);
    }
}
