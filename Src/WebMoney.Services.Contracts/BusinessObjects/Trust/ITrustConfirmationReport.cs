using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface ITrustConfirmationReport
    {
        IExpressTrust Trust { get; }
        string PublicMessage { get; }
        SmsState SmsState { get; }
    }
}
