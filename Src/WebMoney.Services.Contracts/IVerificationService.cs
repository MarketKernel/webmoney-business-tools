using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface IVerificationService
    {
        IVerificationReport VerifyClient(ISuspectedClientInfo clientInfo);
    }
}
