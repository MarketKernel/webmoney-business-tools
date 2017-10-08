using System;
using System.Xml.Serialization;

namespace WebMoney.Services.BusinessObjects
{
    public class ExportableTransfer
    {
        [XmlElement("Id")]
        public int TransferId { get; set; }

        [XmlElement("Destination")]
        public string TargePurse { get; set; }

        [XmlElement("Amount")]
        public decimal Amount { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        internal ExportableTransfer()
        {
        }

        public ExportableTransfer(int transferId, string targePurse, decimal amount, string description)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            TransferId = transferId;
            TargePurse = targePurse ?? throw new ArgumentNullException(nameof(targePurse));
            Amount = amount;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}