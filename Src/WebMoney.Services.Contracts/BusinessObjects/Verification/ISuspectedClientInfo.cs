using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface ISuspectedClientInfo
    {
        ExchangeType ExchangeType { get; }
        bool Output { get; set; }
        string Currency { get; }
        decimal Amount { get; }
        long Identifier { get; }
        string PassportNumber { get; }
        string SecondName { get; }
        string FirstName { get; }
        string BankName { get; }
        string BankAccount { get; }
        string CardNumber { get; }
        PaymentSystem PaymentSystem { get; }
        string PaymentSystemClientId { get; }
        string Phone { get; }
        CryptoCurrency CryptoCurrency { get; }
        string CryptoCurrencyAddress { get; }
    }
}