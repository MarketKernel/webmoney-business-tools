namespace WebMoney.Services.Contracts.BasicTypes
{
    public enum SmsState
    {
        Buffered,
        Sending,
        Sended,
        Delivered,
        NonDelivered,
        Suspended,
        HlrPending,
        HlrMismatch
    }
}
