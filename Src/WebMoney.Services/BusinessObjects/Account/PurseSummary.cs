using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMoney.Services.BusinessObjects
{
    [Table("PurseSummary")]
    internal sealed class PurseSummary
    {
        [Key, MaxLength(13)]
        public string Purse { get; set; }

        [Index(IsUnique = false), Required]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long Identifier { get; set; }

        internal PurseSummary()
        {
        }

        public PurseSummary(string purse, long identifier)
        {
            Purse = purse ?? throw new ArgumentNullException(nameof(purse));
            Identifier = identifier;
        }
    }
}