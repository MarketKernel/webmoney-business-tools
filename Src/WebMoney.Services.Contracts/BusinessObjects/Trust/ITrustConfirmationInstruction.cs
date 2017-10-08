using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface ITrustConfirmationInstruction
    {
        int Reference { get; }
        ConfirmationType ConfirmationType { get; }
        string PublicMessage { get; }
        long? SlaveIdentifier { get; }
        string SlavePurse { get;  }
        string SmsReference { get; }
    }
}