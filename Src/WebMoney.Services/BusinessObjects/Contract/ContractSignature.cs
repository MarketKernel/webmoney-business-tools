using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    [Table("ContractSignature")]
    public class ContractSignature : IContractSignature
    {
        [Key, Column(Order = 0)]
        public int ContractId { get; set; }

        [Key, Column(Order = 1)]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long AcceptorIdentifier { get; set; }

        [ForeignKey("ContractId")]
        public Contract Contract { get; set; }

        public DateTime? AcceptTime { get; set; }

        internal ContractSignature()
        {
        }

        public ContractSignature(long acceptorIdentifier)
        {
            AcceptorIdentifier = acceptorIdentifier;
        }
    }
}