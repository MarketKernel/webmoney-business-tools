using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface ISettings
    {
        int TransferId { get; set; }
        int OrderId { get; set; }
        IRequestSettings RequestSettings { get; set; }
        Language Language { get; set; }

        ITransferSettings TransferSettings { get; set; }
        IOperationSettings OperationSettings { get; set; }
        IIncomingInvoiceSettings IncomingInvoiceSettings { get; set; }
        IOutgoingInvoiceSettings OutgoingInvoiceSettings { get; set; }
        IContractSettings ContractSettings { get; set; }
        ITransferBundleSettings TransferBundleSettings { get; set; }
        IPreparedTransferSettings PreparedTransferSettings { get; set; }

        bool AllowUnauthorizedExtensions { get; set; }
    }
}