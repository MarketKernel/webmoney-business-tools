using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    [Table("IdentifierSummary")]
    public sealed class IdentifierSummary : IIdentifierSummary
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long Identifier { get; set; }

        [Required]
        public string IdentifierAlias { get; set; }

        [Required]
        public bool IsMaster { get; set; }

        [Required]
        public bool IsCapitaller { get; set; }

        internal IdentifierSummary()
        {
        }

        public IdentifierSummary(long identifier, string identifierAlias)
        {
            Identifier = identifier;
            IdentifierAlias = identifierAlias ?? throw new ArgumentNullException(nameof(identifierAlias));
        }

        public static IdentifierSummary Create(IIdentifierSummary identifierSummary)
        {
            if (null == identifierSummary)
                throw new ArgumentNullException(nameof(identifierSummary));

            return new IdentifierSummary(identifierSummary.Identifier, identifierSummary.IdentifierAlias)
            {
                IsMaster = identifierSummary.IsMaster
            };
        }
    }
}
