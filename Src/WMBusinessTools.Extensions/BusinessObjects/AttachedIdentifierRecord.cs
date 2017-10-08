using System;
using System.ComponentModel.DataAnnotations;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class AttachedIdentifierRecord
    {

        [DisplayFormat(DataFormatString = "{0:000000000000}")]
        public long Identifier { get; set; }

        public int? BL { get; set; }
        public int? TL { get; set; }
        public string IdentifierAlias { get; set; }
        public string Info { get; set; }
        public DateTime RegistrationDate { get; set; }

        public AttachedIdentifierRecord(IAttachedIdentifierSummary attachedIdentifier)
        {
            if (null == attachedIdentifier)
                throw new ArgumentNullException(nameof(attachedIdentifier));

            Identifier = attachedIdentifier.Identifier;
            BL = attachedIdentifier.Bl;
            TL = attachedIdentifier.Tl;
            IdentifierAlias = attachedIdentifier.IdentifierAlias;
            Info = attachedIdentifier.Description;
            RegistrationDate = attachedIdentifier.RegistrationDate;
        }
    }
}
