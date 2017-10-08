using System;
using System.ComponentModel.DataAnnotations;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class ExpressTrust : IExpressTrust
    {
        public int TrustId { get; }

        public string SlavePurse { get; }

        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long SlaveIdentifier { get; }

        public string PublicMessage { get; set; }

        public ExpressTrust(int trustId, string slavePurse, long slaveIdentifier, string publicMessage)
        {
            TrustId = trustId;
            SlavePurse = slavePurse ?? throw new ArgumentNullException(nameof(slavePurse));
            SlaveIdentifier = slaveIdentifier;
            PublicMessage = publicMessage ?? throw new ArgumentNullException(nameof(publicMessage));
        }
    }
}
