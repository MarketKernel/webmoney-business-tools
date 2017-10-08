using System;
using System.ComponentModel.DataAnnotations;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class LightCertificate : ILightCertificate
    {
        public string SubjectName { get; }
        public string IssuerName { get; }

        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long Identifier { get; }

        public string Thumbprint { get; }

        public DateTime NotBefore { get; }

        public LightCertificate(string subjectName, string issuerName, long identifier, string thumbprint,
            DateTime notBefore)
        {
            SubjectName = subjectName ?? throw new ArgumentNullException(nameof(subjectName));
            IssuerName = issuerName ?? throw new ArgumentNullException(nameof(issuerName));
            Identifier = identifier;
            Thumbprint = thumbprint ?? throw new ArgumentNullException(nameof(thumbprint));
            NotBefore = notBefore;
        }
    }
}