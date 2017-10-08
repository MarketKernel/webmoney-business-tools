namespace WebMoney.Services.Contracts.BasicTypes
{
    public enum PreparedTransferState
    {
        Failed = -1,
        Registered = 0,
        Pended,
        Processed,
        Interrupted,
        Completed
    }
}
