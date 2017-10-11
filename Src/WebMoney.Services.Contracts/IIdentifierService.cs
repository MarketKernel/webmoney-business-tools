using System.Collections.Generic;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface IIdentifierService
    {
        void AddSecondaryIdentifier(IIdentifierSummary identifierSummary);
        void RemoveSecondaryIdentifier(long identifier);
        bool IsIdentifierExists(long identifier);
        IEnumerable<IIdentifierSummary> SelectIdentifiers();
        long? FindIdentifier(string purse);
        ICertificate FindCertificate(long identifier, bool levelsRequested, bool claimsRequested, bool attachedLevelsRequested, bool fresh = false);
    }
}