using System;
using System.Collections.Generic;
using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface ITransferBundle
    {
        int Id { get; }
        string Name { get; }
        string SourceAccountName { get; }
        decimal FailedTotalAmount { get; }
        decimal RegisteredTotalAmount { get; }
        decimal PendedTotalAmount { get; }
        decimal ProcessedTotalAmount { get; }
        decimal InterruptedTotalAmount { get; }
        decimal CompletedTotalAmount { get; }
        int FailedCount { get; }
        int RegisteredCount { get; }
        int PendedCount { get; }
        int ProcessedCount { get; }
        int InterruptedCount { get; }
        int CompletedCount { get; }
        TransferBundleState State { get; }
        DateTime CreationTime { get; }
        DateTime UpdateTime { get; }

        IReadOnlyCollection<IPreparedTransfer> Transfers { get; }
    }
}
