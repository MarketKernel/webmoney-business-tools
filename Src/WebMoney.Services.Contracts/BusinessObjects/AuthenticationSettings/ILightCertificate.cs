using System;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface ILightCertificate
    {
        string SubjectName { get; }
        string IssuerName { get; }
        long Identifier { get; }
        string Thumbprint { get; }
        DateTime NotBefore { get; }
    }
}
