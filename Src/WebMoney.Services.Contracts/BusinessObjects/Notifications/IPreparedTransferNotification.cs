namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IPreparedTransferNotification : INotification
    {
        IPreparedTransfer PreparedTransfer { get; }
    }
}
