using System;
using System.Collections.Generic;
using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IContract
    {
        int Id { get; }
        string Name { get; }
        string Text { get; }
        bool IsPublic { get; }
        DateTime CreationTime { get; }
        ContractState State { get; }
        int AcceptedCount { get; }
        int AccessCount { get; }
        IReadOnlyCollection<IContractSignature> Signatures { get; }
    }
}