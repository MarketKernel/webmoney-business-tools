using System.Collections.Generic;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface ITrustService
    {
        void CreateTrust(IOriginalTrust trust);
        ITrustConfirmationInstruction RequestTrust(IOriginalExpressTrust originalExpressTrust);
        IExpressTrust ConfirmTrust(ITrustConfirmation trustConfirmation);
        IEnumerable<ITrust> SelectTrusts(bool fresh = false);
    }
}
