using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    [Table("Contract")]
    public class Contract : IContract
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        [Index]
        public DateTime CreationTime { get; set; }

        [NotMapped]
        public ContractState State
        {
            get
            {
                if (0 == AcceptedCount)
                    return ContractState.Created;

                if (IsPublic)
                    return ContractState.Signed;

                if (AcceptedCount == AccessCount)
                    return ContractState.Completed;

                return ContractState.Signed;
            }
        }

        [NotMapped]
        public int AcceptedCount
        {
            get { return Signatures.Count(s => null != s.AcceptTime); }
        }

        [NotMapped]
        public int AccessCount
        {
            get
            {
                if (IsPublic)
                    return 0;

                return Signatures.Count;
            }
        }

        [InverseProperty("Contract")]
        public List<ContractSignature> Signatures { get; }

        [NotMapped]
        IReadOnlyCollection<IContractSignature> IContract.Signatures => Signatures?.Select(s => (IContractSignature) s).ToList();

        internal Contract()
        {
            Signatures = new List<ContractSignature>();
        }

        public Contract(string name, string text, DateTime creationTime)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            CreationTime = creationTime;
            Signatures = new List<ContractSignature>();
        }
    }
}