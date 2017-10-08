using System;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IAttachedIdentifierSummary
    {
        long Identifier { get; }
        string IdentifierAlias { get; }
        string Description { get; }
        int? Bl { get; set; }
        int? Tl { get; set; }
        DateTime RegistrationDate { get; }
    }
}
