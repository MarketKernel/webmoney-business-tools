using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    [Table("AttachedIdentifier")]
    internal sealed class AttachedIdentifierSummary : IAttachedIdentifierSummary
    {
        [Key, Column(Order = 0)]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long CertificateId { get; set; }

        [Key, Column(Order = 1)]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long Identifier { get; set; }

        [ForeignKey("CertificateId")]
        public Certificate Certificate { get; set; }

        public string IdentifierAlias { get; set; }
        public string Description { get; set; }
        public int? Bl { get; set; }
        public int? Tl { get; set; }
        public DateTime RegistrationDate { get; set; }

        internal AttachedIdentifierSummary()
        {
        }

        public AttachedIdentifierSummary(long identifier, string identifierAlias, string description,
            DateTime registrationDate)
        {
            Identifier = identifier;
            IdentifierAlias = identifierAlias;
            Description = description;
            RegistrationDate = registrationDate;
        }
    }
}