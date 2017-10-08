namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface ITransferBundleNotification : INotification
    {
        ITransferBundle TransferBundle { get; }
    }
}
