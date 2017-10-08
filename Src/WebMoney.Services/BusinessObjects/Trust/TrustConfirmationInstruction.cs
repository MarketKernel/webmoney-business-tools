using System;
using System.ComponentModel.DataAnnotations;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class TrustConfirmationInstruction : ITrustConfirmationInstruction
    {
        public int Reference { get; }
        public ConfirmationType ConfirmationType { get; }
        public string PublicMessage { get; }

        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long? SlaveIdentifier { get; set; }

        public string SlavePurse { get; set; }
        public string SmsReference { get; }

        public TrustConfirmationInstruction(int reference, ConfirmationType confirmationType, string publicMessage,
            string smsReference)
        {
            Reference = reference;
            ConfirmationType = confirmationType;
            PublicMessage = publicMessage ?? throw new ArgumentNullException(nameof(publicMessage));
            SmsReference = smsReference;
        }
    }
}