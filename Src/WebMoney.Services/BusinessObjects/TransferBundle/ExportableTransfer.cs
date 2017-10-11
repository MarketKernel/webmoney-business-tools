using System;
using System.Xml.Serialization;

namespace WebMoney.Services.BusinessObjects
{
    internal class ExportableTransfer
    {
        [XmlElement("Id")]
        public int TransferId { get; set; }

        [XmlElement("Destination")]
        public string TargetPurse { get; set; }

        [XmlElement("Amount")]
        public decimal Amount { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        internal ExportableTransfer()
        {
        }

        public ExportableTransfer(int transferId, string targetPurse, decimal amount, string description)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            TransferId = transferId;
            TargetPurse = targetPurse ?? throw new ArgumentNullException(nameof(targetPurse));
            Amount = amount;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}