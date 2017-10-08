using System;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IContractSignature
    {
        long AcceptorIdentifier { get; }
        DateTime? AcceptTime { get; }
    }
}